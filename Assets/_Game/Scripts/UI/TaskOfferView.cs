using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
        StartCoroutine(AnimateSlider(deliveryTask.ExpiredDuration));
        
        deliveryTask.Cancelled += OnCancelledDeliveryTask;
        
        _acceptButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.AddListener(() =>
        {
            callback?.Invoke();
            Close();
        });
    }

    private void OnCancelledDeliveryTask(DeliveryTask task)
    {
        task.Cancelled -= OnCancelledDeliveryTask;
        Close();
    }

    private IEnumerator AnimateSlider(float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            _timeSlider.value = elapsedTime / duration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _timeSlider.value = 1;
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