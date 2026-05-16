using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskOfferView : BaseUIElement
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private Image _fromImage;
    [SerializeField] private Image _toImage;
    [SerializeField] private Image _arrowImage;
    
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Slider _timeSlider;
    
    public void Init(DeliveryTask deliveryTask, Action callback)
    {
        _acceptButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.AddListener(() =>
        {
            callback?.Invoke();
            Close();
        });
    }

    protected override void OnShow()
    {
        ShowFromRight();
    }

    protected override void OnClose()
    {
        HideToRight();
    }
}