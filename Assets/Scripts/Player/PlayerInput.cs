using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Vector2 _inputThreshold = Vector2.zero;
    [SerializeField]
    public bool Shoot { get; private set; }
    public bool Move { get; private set; }
    public float MoveDirection { get; private set; }
    public bool Rotate { get; private set; }
    public float RotationSide { get; private set; }

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Rotate = (Mathf.Abs(horizontalInput) > _inputThreshold.x) ? true : false;
        RotationSide = Rotate ? horizontalInput : 0;

        Move = (Mathf.Abs(verticalInput) > _inputThreshold.y) ? true : false;
        MoveDirection = Move ? verticalInput : 0;

        Shoot = (Input.GetMouseButton(0) && !MouseOnUIClickChecker.IsMouseOverUI()) ? true : false;
    }
}
