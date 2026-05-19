using System;
using System.Collections;
using UnityEngine;

public class DeliveryMan : MonoBehaviour, ITickable
{
    [SerializeField] private float _grabDistance = 1f;
    [SerializeField] private float _giveDistance = 1f;
    [SerializeField] private float _alreadyGrabbedDistance = 0.05f;
    [SerializeField] private float _grabSpeed = 3f;
    
    [Header("GrabFlight")]
    [SerializeField] float _flightTime = 0.7f;
    [SerializeField] float _maxHeight = 2f; 
    [SerializeField] float _smoothness = 1.0f;  
    
    private DeliveryObjectFactory _deliveryObjectFactory;

    private Transform _transform;
    private DeliveryPackage _deliveryPackage;
    private DeliveryRecipient _deliveryRecipient;
    
    private enum DeliveryState
    {
        Waiting,
        ToDeliveryObject,
        Grabbing,
        ToDeliveryRecipient,
        Throwing,
    }
    
    private DeliveryState _deliveryState = DeliveryState.Waiting;
    
    public event Action<DeliveryPackage> GrabStarted;
    public event Action<DeliveryPackage> GrabEnded;
    public event Action<DeliveryPackage> ThrowEnded;

    private void Awake()
    {
        _transform = transform;
    }
    
    public void Init(DeliveryObjectFactory factory)
    {
        _deliveryObjectFactory = factory;
    }
    
    public void SetTarget(DeliveryPackage deliveryPackage)
    {
        _deliveryState = DeliveryState.ToDeliveryObject;
        _deliveryPackage = deliveryPackage;
        
        GlobalEvents.Send(new DeliveryManGoingToDeliveryPackageEvent
        {
            DeliveryPackage = _deliveryPackage,
        });
    }
    
    public void SetTarget(DeliveryRecipient deliveryRecipient)
    {
        _deliveryState = DeliveryState.ToDeliveryRecipient;
        _deliveryRecipient = deliveryRecipient;
        
        GlobalEvents.Send(new DeliveryManGoingToDeliveryRecipientEvent()
        {
            DeliveryRecipient = _deliveryRecipient,
        });
    }

    public void Tick()
    {
        if (_deliveryState == DeliveryState.Waiting)
            return;
        
        if (_deliveryState == DeliveryState.ToDeliveryObject && 
            _transform.position.Closer(_deliveryPackage.transform.position, _grabDistance))
            StartCoroutine(Grab());

        if (_deliveryState == DeliveryState.ToDeliveryRecipient &&
            _transform.position.Closer(_deliveryRecipient.transform.position, _giveDistance))
            StartCoroutine(Throw());
    }
    
    private IEnumerator MoveParabolic(Transform from, Transform to)
    {
        Vector3 startPos = from.position;
        
        float elapsedTime = 0f;
    
        while (elapsedTime < _flightTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _flightTime;
            
            Vector3 endPos = to.position;
        
            Vector3 midPoint = (startPos + endPos) * 0.5f;
            midPoint.y += _maxHeight;
        
            float easedTime = Mathf.SmoothStep(0f, 1f, Mathf.Pow(normalizedTime, 1f / _smoothness));
        
            Vector3 position = CalculateQuadraticBezierPoint(
                startPos, 
                midPoint, 
                endPos, 
                easedTime
            );
        
            from.position = position;
            yield return null;
        }
    
        from.position = to.position;
        yield return null;
    }
    
    private IEnumerator Grab()
    {
        _deliveryState = DeliveryState.Grabbing;
        
        GrabStarted?.Invoke(_deliveryPackage);
        
        yield return MoveParabolic(_deliveryPackage.transform, _transform);
    
        _deliveryState = DeliveryState.ToDeliveryRecipient;

        GrabEnded?.Invoke(_deliveryPackage);
        Destroy(_deliveryPackage.gameObject);
        _deliveryPackage = null;
    }
    
    private IEnumerator Throw()
    {
        _deliveryState = DeliveryState.Throwing;
        
        _deliveryPackage = _deliveryObjectFactory.Create();
        
        _deliveryPackage.transform.position = _transform.position;
        
        yield return MoveParabolic(_deliveryPackage.transform, _deliveryRecipient.transform);
        
        _deliveryState = DeliveryState.Waiting;
        
        ThrowEnded?.Invoke(_deliveryPackage);
        
        Destroy(_deliveryPackage.gameObject);
        _deliveryPackage = null;
    }

    private Vector3 CalculateQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // Формула квадратичной кривой Безье: (1-t)²P0 + 2(1-t)tP1 + t²P2
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
    
        Vector3 point = uu * p0;          // (1-t)² * P0
        point += 2 * u * t * p1;          // 2(1-t)t * P1
        point += tt * p2;                 // t² * P2
    
        return point;
    }

    private void OnDrawGizmos()
    {
        var color = Color.blue;
        color.a = 0.3f;
        
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, _giveDistance);
    }
}

public static class Vector3Extensions
{
    public static float SqrtDistance(this Vector3 position, Vector3 other)
    {
        return (other - position).sqrMagnitude;
    }
    
    public static bool Closer(this Vector3 position, Vector3 other, float distance)
    {
        return position.SqrtDistance(other) < distance;
    }
}