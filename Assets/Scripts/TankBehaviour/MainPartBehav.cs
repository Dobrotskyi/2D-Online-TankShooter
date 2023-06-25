using Photon.Pun;
using UnityEngine;

public class MainPartBehav : MonoBehaviourPun
{
    public Vector3 Velocity
    {
        get
        {
            return _rb.velocity;
        }
    }

    private const int ACCELERATION_MULT = 500;

    private Rigidbody2D _rb;
    private MainPartData _data;

    public void ChangeTowards(Vector3 pos, Quaternion rotation)
    {
        _rb.position = Vector3.MoveTowards(_rb.position, pos, Time.fixedDeltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.fixedDeltaTime * 100.0f);
    }

    public void ChangeVelocity(Vector3 newVelocity)
    {
        _rb.velocity = newVelocity;
    }

    public void Move(float direction)
    {
        _rb.AddForce(_rb.transform.up * direction * _data.Acceleration * ACCELERATION_MULT * Time.deltaTime, ForceMode2D.Force);
        if (_rb.velocity.magnitude > _data.MaxSpeed)
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _data.MaxSpeed);
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
            throw new System.Exception("Data was already set for this object");
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
