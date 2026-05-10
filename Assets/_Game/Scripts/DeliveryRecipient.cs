using UnityEngine;

public class DeliveryRecipient : MonoBehaviour
{
    public void Init(IHorizontalAngleOffset _horizontalAngleOffset)
    {
        transform.rotation = Quaternion.Euler(0, _horizontalAngleOffset.HorizontalAngle + 180, 0);
    }
}