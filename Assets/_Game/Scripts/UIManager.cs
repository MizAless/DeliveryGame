using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TaskOfferView _taskOfferView;
    [SerializeField] private ActiveTaskView _activeTaskView;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // _taskOfferView.Close();
        
        _activeTaskView.Close();    
    }

    public void InstantiateOffer(DeliveryTask deliveryTask, Action acceptCallback)
    {
        _taskOfferView.Show();
        _taskOfferView.Init(deliveryTask, acceptCallback);
    }
    
    public void InstantiateActiveTask(DeliveryTask deliveryTask)
    {
        _activeTaskView.Show();
        _activeTaskView.Init(deliveryTask);
    }
}