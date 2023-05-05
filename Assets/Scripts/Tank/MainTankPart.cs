using UnityEngine;

public class MainTankPart : MonoBehaviour
{
    private Rigidbody2D _rb;
    private MainPartData _data;

    public void Move(float direction)
    {
        _rb.AddForce(_rb.transform.up * direction * _data.Acceleration, ForceMode2D.Force);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _data.MaxSpeed);
        Vector3 lookDirection = _rb.transform.rotation.eulerAngles;
        lookDirection.z += 90;
    }

    public void Rotate(float side)
    {
        _rb.AddTorque(-side * _data.AngularSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void SetData(MainPartData data)
    {
        _data = data;
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
