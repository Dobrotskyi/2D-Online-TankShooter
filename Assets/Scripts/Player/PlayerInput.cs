using UnityEngine;
using System;
using Unity;

public class PlayerInput : MonoBehaviour
{
    public event Action Shoot;
    public event Action<float> Move;
    public event Action<float> Rotate;
    [SerializeField] private Vector2 _inputThreshold = Vector2.zero;

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > _inputThreshold.x)
            Rotate?.Invoke(horizontalInput);
        if (Mathf.Abs(verticalInput) > _inputThreshold.y)
            Move?.Invoke(verticalInput);

        if (Input.GetMouseButton(0))
            Shoot?.Invoke();
    }
}
