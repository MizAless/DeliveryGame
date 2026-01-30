using JetBrains.Annotations;
using UnityEngine;
using DG.Tweening;

public class NavigationArrow : Component<NavigationArrow>, ITickable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _disableDistance;
    [SerializeField] private float _disableDuration = 0.3f;

    [CanBeNull] private Transform _target;

    private Transform _transform;
    
    private bool _isDisabling = false;

    private void Awake()
    {
        _transform = transform;
    }

    public void SetTarget(Transform target)
    {
        _spriteRenderer.DOKill();
        _spriteRenderer.DOFade(1f, 0f);
        _isDisabling = false;
        _target = target;
    }
    
    public void Tick()
    {
        if (_target == null)
            return;
        
        if (!_isDisabling && Vector3.Distance(_transform.position, _target.position) <= _disableDistance)
            Disable();
        
        _transform.forward = Vector3.ProjectOnPlane(_target.position - _transform.position, Vector3.up);
    }

    private void Disable()
    {
        _isDisabling = true;

        var tween = _spriteRenderer.DOFade(0, _disableDuration);
        tween.onComplete += () => { _isDisabling = false; };
    }
}