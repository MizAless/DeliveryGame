using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TaskOfferView _taskOfferView;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // _taskOfferView.Close();
    }

    public void InstantiateOffer(DeliveryTask deliveryTask, Action acceptCallback)
    {
        _taskOfferView.Show();
        _taskOfferView.Init(deliveryTask, acceptCallback);
    }
}