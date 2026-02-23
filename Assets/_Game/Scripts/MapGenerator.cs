using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapGenerator
{
    private ChunkView _chunkPrefab; 
    
    private Dictionary<Vector2, ChunkView.Sides> _directions = new Dictionary<Vector2, ChunkView.Sides>()
    {
        { Vector2.up, ChunkView.Sides.up },
        { Vector2.down, ChunkView.Sides.down },
        { Vector2.left, ChunkView.Sides.left },
        { Vector2.right, ChunkView.Sides.right },
    };
    
    List<ChunkData> _chunks = new List<ChunkData>();
    
    private int _chunkCount;
    private float _biasToCenter = 1f; // коэффициент влияния расстояния (можно настраивать)

    public MapGenerator(int chunkCount, ChunkView chunkPrefab)
    {
        _chunkCount = chunkCount;
        _chunkPrefab = chunkPrefab;
    }

    public void Generate()
    {
        _chunks.Clear();
        
        Vector2 currentPosition = Vector2.zero;
        Vector2 previousDirection = Vector2.zero;

        var startChunk = new ChunkData()
        {
            Position = currentPosition,
            ActiveSides = ChunkView.Sides.none,
        };

        InitChunkSides(startChunk);
        
        _chunks.Add(startChunk);
        
        for (int i = 0; i < _chunkCount - 1; i++)
        {
            var randomDirection = GetRandomDirection(currentPosition, previousDirection);
            
            currentPosition += randomDirection;
            
            var newChunk = new ChunkData()
            {
                Position = currentPosition,
                ActiveSides = ChunkView.Sides.none,
            };
            
            previousDirection = randomDirection;
            InitChunkSides(newChunk);
            _chunks.Add(newChunk);
        }

        foreach (var chunk in _chunks)
        {
            var position = new Vector3(chunk.Position.x, 0, chunk.Position.y) * ChunkView.ChunkOffset;
            var chunkView = Object.Instantiate(_chunkPrefab, position, Quaternion.identity);
            chunkView.BindData(chunk);
        }
    }
    
    private void InitChunkSides(ChunkData chunkData)
    {
        var activeSides = ChunkView.Sides.down | ChunkView.Sides.up | ChunkView.Sides.left | ChunkView.Sides.right;
        
        // foreach (var direction in _directions.Keys)
        // {
        //     var neighborPos = chunkData.Position + direction;
        //     if (_chunks.Any(chunk => chunk.Position == neighborPos))
        //         activeSides |= _directions[direction];
        // }
        
        chunkData.ActiveSides = activeSides;
    }

    private Vector2 GetRandomDirection(Vector2 currentPos, Vector2 previousDir)
    {
        // Собираем все возможные направления
        var possibleDirs = _directions.Keys.ToList();
        
        // Исключаем противоположное предыдущему, чтобы не ходить назад
        var opposite = previousDir * -1;
        if (possibleDirs.Contains(opposite))
            possibleDirs.Remove(opposite);
        
        // Если направлений не осталось (крайний случай), возвращаем случайное из всех
        if (possibleDirs.Count == 0)
            possibleDirs = _directions.Keys.ToList();

        // Отдельно соберём свободные направления (ведущие на пустые клетки)
        var freeDirs = new List<Vector2>();
        var occupiedDirs = new List<Vector2>();
        
        foreach (var dir in possibleDirs)
        {
            var newPos = currentPos + dir;
            if (_chunks.Any(c => c.Position == newPos))
                occupiedDirs.Add(dir);
            else
                freeDirs.Add(dir);
        }

        // Выбираем, из какого списка будем выбирать: предпочитаем свободные, если они есть
        var candidates = freeDirs.Count > 0 ? freeDirs : occupiedDirs;
        
        // Если кандидатов нет (невозможно), возвращаем первое попавшееся
        if (candidates.Count == 0)
            return possibleDirs[UnityEngine.Random.Range(0, possibleDirs.Count)];

        // Рассчитываем веса для каждого кандидата на основе расстояния до центра
        float[] weights = new float[candidates.Count];
        float totalWeight = 0f;
        
        for (int i = 0; i < candidates.Count; i++)
        {
            Vector2 newPos = currentPos + candidates[i];
            float distance = Vector2.Distance(newPos, Vector2.zero);
            // Вес = 1 / (1 + distance * bias) – чем дальше, тем меньше вес
            weights[i] = 1f / (1f + distance * _biasToCenter);
            totalWeight += weights[i];
        }

        // Выбор случайного направления с учётом весов
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < candidates.Count; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
                return candidates[i];
        }

        // fallback
        return candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}

public class ChunkData
{
    private ChunkView.Sides _activeSides;

    public Vector2 Position;
    public ChunkView.Sides ActiveSides
    {
        get => _activeSides;
        set
        {
            _activeSides = value;
            ActiveSidesChanged?.Invoke(value);
        }
    }

    public event Action<ChunkView.Sides> ActiveSidesChanged; 
}