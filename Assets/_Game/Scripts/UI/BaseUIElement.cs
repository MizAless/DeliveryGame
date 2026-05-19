using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class BaseUIElement : MonoBehaviour
{
    [SerializeField] protected Image _backgroundImage;
    [SerializeField] protected Image _borderImage;

    private int pixelOffsetForAnimation = 1000;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Canvas _canvas;
    private Direction _lastHideDirection; // Запоминаем последнее направление скрытия
    
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
        OnShow();
    }
    
    public void Close()
    {
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
        
        bool isShow = animationType == AnimationType.Show;
        
        if (isShow)
        {
            // При показе - стартуем с позиции, соответствующей направлению входа
            // (это может быть позиция после последнего скрытия или стандартная заэкранная)
            Vector2 startPosition;
            
            // Если последнее скрытие было с тем же направлением, используем текущую позицию
            if (_lastHideDirection == direction && !gameObject.activeSelf)
            {
                startPosition = _rectTransform.anchoredPosition;
            }
            else
            {
                startPosition = GetOffScreenPosition(direction);
            }
            
            _rectTransform.anchoredPosition = startPosition;
            
            // Анимируем к оригинальной позиции
            Tween showTween = _rectTransform.DOAnchorPos(_originalPosition, 0.5f)
                .SetEase(Ease.OutBack, 1.2f);
        }
        else
        {
            // При скрытии - запоминаем направление и анимируем за экран
            _lastHideDirection = direction;
            Vector2 targetPosition = GetOffScreenPosition(direction);
            
            Tween hideTween = _rectTransform.DOAnchorPos(targetPosition, 0.4f)
                .SetEase(Ease.InBack, 1.2f);
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
    
    // Опционально: метод для принудительного сброса позиции
    public void ResetPosition()
    {
        if (_rectTransform != null)
        {
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }
    
    protected virtual void OnDestroy()
    {
        if (_rectTransform != null)
            _rectTransform.DOKill();
    }
}