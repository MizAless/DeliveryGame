using UnityEngine;

public abstract class AbstractFactory<T> : IFactory<T>
    where T : MonoBehaviour
{
    private T _prefab;

    protected AbstractFactory(T prefab)
    {
        _prefab = prefab;
    }
    
    public virtual T Create()
    {
        var @object  = Object.Instantiate(_prefab);

        return @object;
    }
}

public class RandomPlacer
{
    private Vector3 _point;
    private float _radius;

    public RandomPlacer(Vector3 point, float radius)
    {
        _point = point;
        _radius = radius;
    }

    public void Place(Transform target)
    {
        Vector2 offset2D = Random.insideUnitCircle * _radius;
        target.position = _point + new Vector3(offset2D.x, 0f, offset2D.y);
    }
} 