using UnityEngine;
using System;
using Unity;

public class PlayerInput : MonoBehaviour
{
    public event Action Shoot;
    public event Action<float> Move;
    public event Action<float> Rotate;
    [SerializeField] private Vector2 _threshold = Vector2.zero;

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > _threshold.x)
            Rotate?.Invoke(horizontalInput);
        if (Mathf.Abs(verticalInput) > _threshold.y)
            Move?.Invoke(verticalInput);

        if (Input.GetMouseButton(0))
            Shoot?.Invoke();
    }
}
