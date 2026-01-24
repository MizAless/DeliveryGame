using System.Collections;
using UnityEngine;

public class DeliveryMan : MonoBehaviour, ITickable
{
    [SerializeField] private float _grabDistance = 1f;
    [SerializeField] private float _alreadyGrabbedDistance = 0.05f;
    [SerializeField] private float _grabSpeed = 3f;
    
    [Header("GrabFlight")]
    [SerializeField] float _flightTime = 0.7f;
    [SerializeField] float _maxHeight = 2f; 
    [SerializeField] float _smoothness = 1.0f;  

    private Transform _transform;
    private DeliveryObject _deliveryObject;
    
    private bool _isGrabbing = false;

    private void Awake()
    {
        _transform = transform;
    }
    
    public void Init(DeliveryObject deliveryObject)
    {
        _deliveryObject = deliveryObject;
    }

    public void Tick()
    {
        if (_deliveryObject == null)
        {
            _deliveryObject = FindAnyObjectByType<DeliveryObject>();
            return;
        }
        
        if (!_isGrabbing && (_transform.position - _deliveryObject.transform.position).sqrMagnitude < _grabDistance * _grabDistance)
        {
            _isGrabbing = true;
            StartCoroutine(GrabParabolic());
        }
    }
    
    private IEnumerator Grab()
    {
        while ((_transform.position - _deliveryObject.transform.position).sqrMagnitude >
               _alreadyGrabbedDistance * _alreadyGrabbedDistance)
        {
            _deliveryObject.transform.position = Vector3.Lerp(_deliveryObject.transform.position, _transform.position, _grabSpeed * Time.deltaTime);
            yield return null;
        }
        
        Destroy(_deliveryObject.gameObject);
        _deliveryObject = null;
        
        _isGrabbing = false;
    }
    
    private IEnumerator GrabParabolic()
    {
        Vector3 startPos = _deliveryObject.transform.position;
        
        float elapsedTime = 0f;
    
        while (elapsedTime < _flightTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _flightTime;
            
            Vector3 endPos = _transform.position;
        
            Vector3 midPoint = (startPos + endPos) * 0.5f;
            midPoint.y += _maxHeight;
        
            // Применяем easing для гладкости
            float easedTime = Mathf.SmoothStep(0f, 1f, Mathf.Pow(normalizedTime, 1f / _smoothness));
        
            // Квадратичная интерполяция по Безье для параболы
            Vector3 position = CalculateQuadraticBezierPoint(
                startPos, 
                midPoint, 
                _transform.position, 
                easedTime
            );
        
            _deliveryObject.transform.position = position;
            yield return null;
        }
    
        _deliveryObject.transform.position = _transform.position;
    
        Destroy(_deliveryObject.gameObject);
        _deliveryObject = null;
        
        _isGrabbing = false;
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
        Gizmos.DrawSphere(transform.position, _grabDistance);
    }
}