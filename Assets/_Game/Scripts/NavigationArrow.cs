using UnityEngine;

public class NavigationArrow : MonoBehaviour, ITickable
{
    [SerializeField] private GameObject _arrow;
    
    private Camera _camera;
    
    private DeliveryObject _deliveryObject; 
    
    public void Tick()
    {
        transform.right = -(_camera.transform.position - transform.position).normalized;
        
        
        _arrow.transform.forward = transform.position - _deliveryObject.transform.position;
    }
}