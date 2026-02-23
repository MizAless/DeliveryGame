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
    private const float MainSideLength = 10f;
    private const float RoadSideWidth = 3f;
    
    public static float ChunkOffset = (MainSideLength + RoadSideWidth);
    
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private GameObject _main;

    [SerializeField] private List<GameObject> _randomSidesParents;
    // [SerializeField] private List<GameObject> _roads = new List<GameObject>();
    // private Dictionary<Sides, GameObject> _roadsDict = new Dictionary<Sides, GameObject>();
    
    [SerializeField] private List<RoadSide> _roads = new List<RoadSide>();
    
    private ChunkData _chunkData;
    
    public Sides ActiveSides { get; private set; }
    
    [Flags]
    public enum Sides
    {
        none = 0,
        up = 1,
        down = 2,
        left = 4,
        right = 8
    }
    
    [ContextMenu(nameof(EnableAllRoads))]
    private void EnableAllRoads()
    {
        ActiveSides = Sides.up | Sides.down | Sides.left | Sides.right;

        InitRoads();
    }
    
    [ContextMenu(nameof(CreateRoads))]
    private void CreateRoads()
    {
        Vector3 mainPosition = _main.transform.position;
        
        var halfRoadLength = RoadSideWidth * 0.5f;
        var halfMainLength = MainSideLength * 0.5f;
        
        _roads.Add(new RoadSide( Sides.down,Instantiate(_roadPrefab, mainPosition + new Vector3(-halfMainLength - halfRoadLength, 0, 0), Quaternion.identity)));
        _roads.Add(new RoadSide( Sides.up,Instantiate(_roadPrefab, mainPosition + new Vector3(halfMainLength + halfRoadLength, 0, 0), Quaternion.identity)));
        _roads.Add(new RoadSide( Sides.left,Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, -halfMainLength - halfRoadLength), Quaternion.Euler(0, 90,0))));
        _roads.Add(new RoadSide( Sides.right,Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, halfMainLength + halfRoadLength), Quaternion.Euler(0, 90,0))));

        foreach (var road in _roads)
        {
            road.Road.transform.parent = transform;
            road.Road.SetActive(false);
        }
    }
    
    // [ContextMenu(nameof(Init))]
    // public void Init()
    // {
    //     if (Enum.GetValues(typeof(Sides)).Length != _roads.Count)
    //         Debug.LogError($"[CHUNK] Enum.GetValues(typeof(Sides)).Length != _roads.Count");
    //
    //     var arr = Enum.GetValues(typeof(Sides));
    //     
    //     for (int i = 0; i < arr.Length; i++)
    //     {
    //         _roadsDict[(Sides)arr.GetValue(i)] = _roads[i];
    //     }
    // }

    public void BindData(ChunkData chunkData)
    {
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