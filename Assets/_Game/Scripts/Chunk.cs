using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private const float MainSideLength = 10f;
    private const float RoadSideWidth = 3f;
    
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private GameObject _main;

    [SerializeField] private List<GameObject> _roads = new List<GameObject>();
    
    private Dictionary<Sides, GameObject> _roadsDict = new Dictionary<Sides, GameObject>();
    
    public Sides ActiveSides { get; private set; }
    
    [Flags]
    public enum Sides
    {
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
        
        _roads.Add(Instantiate(_roadPrefab, mainPosition + new Vector3(-halfMainLength - halfRoadLength, 0, 0), Quaternion.identity));
        _roads.Add(Instantiate(_roadPrefab, mainPosition + new Vector3(halfMainLength + halfRoadLength, 0, 0), Quaternion.identity));
        _roads.Add(Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, -halfMainLength - halfRoadLength), Quaternion.Euler(0, 90,0)));
        _roads.Add(Instantiate(_roadPrefab, mainPosition + new Vector3(0, 0, halfMainLength + halfRoadLength), Quaternion.Euler(0, 90,0)));

        foreach (var road in _roads)
        {
            road.transform.parent = transform;
            road.SetActive(false);
        }
    }
    
    [ContextMenu(nameof(Init))]
    public void Init()
    {
        if (Enum.GetValues(typeof(Sides)).Length != _roads.Count)
            Debug.LogError($"[CHUNK] Enum.GetValues(typeof(Sides)).Length != _roads.Count");

        var arr = Enum.GetValues(typeof(Sides));
        
        for (int i = 0; i < arr.Length; i++)
        {
            _roadsDict[(Sides)arr.GetValue(i)] = _roads[i];
        }
    }

    private void InitRoads()
    {
        foreach (var pair in _roadsDict)
        {
            Sides side = pair.Key;
            GameObject road = pair.Value;

            road.SetActive(IsSideActive(side));
        }
    }

    private bool IsSideActive(Sides side)
    {
        return (ActiveSides & side) != 0;
    }

}