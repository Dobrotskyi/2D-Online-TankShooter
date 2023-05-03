using UnityEngine;

[CreateAssetMenu(fileName = "MainPart", menuName = "Tank/Part/MainPart", order = 0)]
public class MainPart : TankPart
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _angularSpeed;
    private Rigidbody2D _rb;
    
    public void Move(float direction)
    {
        _rb.AddForce(_rb.transform.up * direction * _acceleration, ForceMode2D.Force);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxSpeed);
        Vector3 lookDirection = _rb.transform.rotation.eulerAngles;
        lookDirection.z += 90;
    }

    public void Rotate(float side)
    {
        _rb.AddTorque(-side * _angularSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public override void SpawnPart(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        base.SpawnPart(parent, activeMonoBehaviour);
        _rb = _model.GetComponent<Rigidbody2D>();
    }
}
