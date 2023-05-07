using UnityEngine;

public class MainPartBehav : MonoBehaviour
{
    private const int MAX_SPEED_MULT = 10;
    private const int ACCELERATION_MULT = 10;

    private Rigidbody2D _rb;
    private MainPartData _data;

    public void Move(float direction)
    {
        _rb.AddForce(_rb.transform.up * direction * _data.Acceleration * ACCELERATION_MULT, ForceMode2D.Force);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _data.MaxSpeed * MAX_SPEED_MULT);
    }

    public void Rotate(float side)
    {
        _rb.AddTorque(-side * _data.AngularSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void SetData(MainPartData data)
    {
        if (_data == null)
            _data = data;
        else
            throw new System.Exception("Scriptable Object data was already set for this object");
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
