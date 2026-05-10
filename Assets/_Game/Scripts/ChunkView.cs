using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoadSide
{
    public ChunkView.Sides Side;
    public GameObject Road;

    public RoadSide(ChunkView.Sides side, GameObject road)
    {
        Side = side;
        Road = road;
    }
}

public class ChunkView : MonoBehaviour
{
    public static float MainSideLength = 10f;
    public static float RoadSideWidth = 3f;
    
    public static float ChunkOffset => MainSideLength + RoadSideWidth;

    [SerializeField] private SpawnPointsContainer _spawnPointsContainer;
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private GameObject _main;

    [SerializeField] private List<GameObject> _randomSidesParents;
    [SerializeField] private List<RoadSide> _roads = new List<RoadSide>();
    
    private ChunkData _chunkData;
    
    public Sides ActiveSides { get; private set; }
    
    [Flags]
    public enum Sides
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }
    
    [ContextMenu(nameof(EnableAllRoads))]
    private void EnableAllRoads()
    {
        ActiveSides = Sides.Up | Sides.Down | Sides.Left | Sides.Right;

        InitRoads();
    }
    
    [ContextMenu(nameof(CreateRoads))]
    private void CreateRoads()
    {
        Vector3 mainPosition = _main.transform.position;
        
        var halfRoadLength = RoadSideWidth * 0.5f;
        var halfMainLength = MainSideLength * 0.5f;
        
        _roads.Add(new RoadSide( Sides.Down,Instantiate(_roadPrefab, mainPosition + new Vector3(-halfMainLength - halfRoadLength, 0, 0), Quaternion.identity)));
        _roads.Add(new RoadSide( Sides.Up,Instantiate(_roadPrefab, mainPosition + new Vector3(halfMainLength + halfRoadLength, 0, 0), Quaternion.identity)));
        _roads.Add(new RoadSide( Sides.Left,Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, -halfMainLength - halfRoadLength), Quaternion.Euler(0, 90,0))));
        _roads.Add(new RoadSide( Sides.Right,Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, halfMainLength + halfRoadLength), Quaternion.Euler(0, 90,0))));

        foreach (var road in _roads)
        {
            road.Road.transform.parent = transform;
            road.Road.SetActive(false);
        }
    }
    
    public void Init(ChunkData chunkData)
    {
        chunkData.SpawnPointsContainer = _spawnPointsContainer;
        _chunkData = chunkData;
        
        OnActiveSidesChanged(_chunkData.ActiveSides);
        SetRandomObjects();
        _chunkData.ActiveSidesChanged += OnActiveSidesChanged;
    }
    
    private void SetRandomObjects()
    {
        foreach (var randomParent in _randomSidesParents)
        {
            foreach (Transform child in randomParent.transform)
                child.gameObject.SetActive(false);
            
            var randomIndex = UnityEngine.Random.Range(0, randomParent.transform.childCount);
            
            randomParent.transform.GetChild(randomIndex).gameObject.SetActive(true);
        }
    }

    private void OnActiveSidesChanged(Sides sides)
    {
        ActiveSides = sides;
        
        InitRoads();
    }

    private void InitRoads()
    {
        foreach (var pair in _roads)
        {
            Sides side = pair.Side;
            GameObject road = pair.Road;

            road.SetActive(IsSideActive(side));
        }
    }

    private bool IsSideActive(Sides side)
    {
        return (ActiveSides & side) != 0;
    }
}