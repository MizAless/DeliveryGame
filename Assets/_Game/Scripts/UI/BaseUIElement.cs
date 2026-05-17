using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class BaseUIElement : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _borderImage;

    private int pixelOffsetForAnimation = 1000;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Canvas _canvas;
    
    // Направления для анимации
    private enum Direction
    {
        Right,
        Left,
        Bottom,
        Top
    }
    
    private enum AnimationType
    {
        Show,
        Hide
    }
    
    protected virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_rectTransform != null)
        {
            _originalPosition = _rectTransform.anchoredPosition;
        }
    }
    
    public void Show()
    {
        // gameObject.SetActive(true);
        OnShow();
    }
    
    public void Close()
    {
        // gameObject.SetActive(false);
        OnClose();
    }
    
    protected abstract void OnShow();
    protected abstract void OnClose();
    
    protected void ShowFromRight() => AnimateFromDirection(Direction.Right, AnimationType.Show);
    protected void ShowFromLeft() => AnimateFromDirection(Direction.Left, AnimationType.Show);
    protected void ShowFromBottom() => AnimateFromDirection(Direction.Bottom, AnimationType.Show);
    protected void ShowFromTop() => AnimateFromDirection(Direction.Top, AnimationType.Show);

    protected void HideToRight() => AnimateFromDirection(Direction.Right, AnimationType.Hide);
    protected void HideToLeft() => AnimateFromDirection(Direction.Left, AnimationType.Hide);
    protected void HideToBottom() => AnimateFromDirection(Direction.Bottom, AnimationType.Hide);
    protected void HideToTop() => AnimateFromDirection(Direction.Top, AnimationType.Hide);

    private void AnimateFromDirection(Direction direction, AnimationType animationType)
    {
        if (_rectTransform == null) return;
        
        // Сохраняем оригинальную позицию если нужно
        if (_originalPosition == Vector2.zero && _rectTransform.anchoredPosition != Vector2.zero)
        {
            _originalPosition = _rectTransform.anchoredPosition;
        }
        
        bool isShow = animationType == AnimationType.Show;
        
        // Получаем позицию для анимации
        Vector2 targetPosition = isShow ? _originalPosition : GetOffScreenPosition(direction);
        
        if (isShow)
        {
            // Телепортируем за экран перед показом
            _rectTransform.anchoredPosition = GetOffScreenPosition(direction);
        }
        
        // Настройки анимации
        float duration = isShow ? 0.5f : 0.4f;
        Ease ease = isShow ? Ease.OutBack : Ease.InBack;
        
        // Запускаем анимацию
        Tween tween = _rectTransform.DOAnchorPos(targetPosition, duration)
            .SetEase(ease, 1.2f);
        
        // Если это анимация скрытия - деактивируем объект по завершению
        if (!isShow)
        {
            tween.OnComplete(() => gameObject.SetActive(false));
        }
    }
    
    private Vector2 GetOffScreenPosition(Direction direction)
    {
        Vector2 offScreenPos = _originalPosition;
        
        switch (direction)
        {
            case Direction.Right:
                offScreenPos.x += GetScreenWidth() + pixelOffsetForAnimation;
                break;
            case Direction.Left:
                offScreenPos.x -= GetScreenWidth() + pixelOffsetForAnimation;
                break;
            case Direction.Bottom:
                offScreenPos.y -= GetScreenHeight() + pixelOffsetForAnimation;
                break;
            case Direction.Top:
                offScreenPos.y += GetScreenHeight() + pixelOffsetForAnimation;
                break;
        }
        
        return offScreenPos;
    }

    public void SetBackgroundColor(Color color)
    {
        if (_backgroundImage != null)
            _backgroundImage.color = color;
    }
    
    public void SetBorderColor(Color color)
    {
        if (_borderImage != null)
            _borderImage.color = color;
    }
    
    private float GetScreenWidth()
    {
        if (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return Screen.width;
        }
        
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        return canvasRect != null ? canvasRect.rect.width : Screen.width;
    }
    
    private float GetScreenHeight()
    {
        if (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return Screen.height;
        }
        
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        return canvasRect != null ? canvasRect.rect.height : Screen.height;
    }
    
    protected virtual void OnDestroy()
    {
        if (_rectTransform != null)
            _rectTransform.DOKill();
    }
}