using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryRecipientView : MonoBehaviour
{
    [SerializeField] private Transform _modelParent;
    [SerializeField] private List<GameObject> _modelsPool;

    private void Awake()
    {
        InitModel();
    }

    private void InitModel()
    {
        var model = Instantiate(_modelsPool[Random.Range(0, _modelsPool.Count)], Vector3.zero, Quaternion.identity);
        model.transform.SetParent(_modelParent, false);
    }
}