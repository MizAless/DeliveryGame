using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveTaskView : BaseUIElement
{
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private Image _fromImage;
    [SerializeField] private Image _toImage;
    [SerializeField] private Image _arrowImage;
    
    protected override void OnShow()
    {
        ShowFromLeft();
    }

    protected override void OnClose()
    {
        throw new System.NotImplementedException();
    }
}