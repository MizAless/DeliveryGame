using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveTaskView : BaseUIElement
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private Image _fromImage;
    [SerializeField] private Image _toImage;
    [SerializeField] private Image _arrowImage;
    
    private Tween _currentTween;
    private Color _originalFromColor;
    private Color _originalArrowColor;
    private Color _originalBackgroundColor;
    
    protected override void OnShow()
    {
        ShowFromLeft();
        CacheOriginalColors();
    }

    protected override void OnClose()
    {
        KillAllTweens();
        HideToLeft();
    }
    
    private void CacheOriginalColors()
    {
        if (_fromImage != null)
            _originalFromColor = _fromImage.color;
        if (_arrowImage != null)
            _originalArrowColor = _arrowImage.color;
        if (_backgroundImage != null)
            _originalBackgroundColor = _backgroundImage.color;
    }
    
    private void KillAllTweens()
    {
        _currentTween?.Kill();
        
        if (_fromImage != null)
            _fromImage.DOKill();
        if (_arrowImage != null)
            _arrowImage.DOKill();
        if (_backgroundImage != null)
            _backgroundImage.DOKill();
    }

    public void Init(DeliveryTask deliveryTask)
    {
        deliveryTask.Completed += OnDeliveryTaskCompleted;
        deliveryTask.StateChanged += UpdateState;
    }

    private void OnDeliveryTaskCompleted(DeliveryTask deliveryTask)
    {
        deliveryTask.Completed -= OnDeliveryTaskCompleted; // Fixed: unsubscribe, not subscribe
        Close();
    }
    
    private void UpdateState(DeliveryTask.TaskState taskState)
    {
        switch (taskState)
        {
            case DeliveryTask.TaskState.GrabPackage:
                StopArrowBlinking();
                StopBackgroundBlinking();
                StartFromImageBlinking();
                break;
                
            case DeliveryTask.TaskState.GivePackage:
                StopFromImageBlinking();
                StopBackgroundBlinking();
                StartArrowBlinking();
                break;
                
            case DeliveryTask.TaskState.Completed:
                StopFromImageBlinking();
                StopArrowBlinking();
                StartBackgroundGoldBlinking();
                break;
                
            case DeliveryTask.TaskState.Cancelled:
            case DeliveryTask.TaskState.WaitingForAccept:
            case DeliveryTask.TaskState.InProgress:
            default:
                StopAllBlinking();
                ResetToOriginalColors();
                break;
        }
    }
    
    private void StartFromImageBlinking()
    {
        if (_fromImage == null) return;
        
        StopFromImageBlinking();
        
        Color grayColor = Color.gray;
        _currentTween = _fromImage.DOColor(grayColor, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    private void StopFromImageBlinking()
    {
        if (_fromImage == null) return;
        
        _fromImage.DOKill();
        _fromImage.color = _originalFromColor;
    }
    
    private void StartArrowBlinking()
    {
        if (_arrowImage == null) return;
        
        StopArrowBlinking();
        
        Color grayColor = Color.gray;
        _currentTween = _arrowImage.DOColor(grayColor, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    private void StopArrowBlinking()
    {
        if (_arrowImage == null) return;
        
        _arrowImage.DOKill();
        _arrowImage.color = _originalArrowColor;
    }
    
    private void StartBackgroundGoldBlinking()
    {
        if (_backgroundImage == null) return;
        
        StopBackgroundBlinking();
        
        Color goldColor = new Color(1f, 0.84f, 0f, 1f); // Gold color
        _currentTween = _backgroundImage.DOColor(goldColor, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    private void StopBackgroundBlinking()
    {
        if (_backgroundImage == null) return;
        
        _backgroundImage.DOKill();
        _backgroundImage.color = _originalBackgroundColor;
    }
    
    private void StopAllBlinking()
    {
        StopFromImageBlinking();
        StopArrowBlinking();
        StopBackgroundBlinking();
    }
    
    private void ResetToOriginalColors()
    {
        if (_fromImage != null)
            _fromImage.color = _originalFromColor;
        if (_arrowImage != null)
            _arrowImage.color = _originalArrowColor;
        if (_backgroundImage != null)
            _backgroundImage.color = _originalBackgroundColor;
    }
}