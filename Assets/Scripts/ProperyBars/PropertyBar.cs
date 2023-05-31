using UnityEngine;

public class PropertyBar : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    private float _yOffset = 0.8f;
    private Transform _followedObj;
    private TankProperty _tankProp;


    public PropertyBar SetProperty(TankProperty tankProp)
    {
        if (_tankProp != null)
            return this;

        _tankProp = tankProp;
        _tankProp.ValueChanged += UpdateBar;
        return this;
    }

    public PropertyBar SetFollowObject(Transform obj)
    {
        if (_followedObj == null)
            _followedObj = obj;
        return this;
    }

    public PropertyBar SetYOffset(float yOffset)
    {
        _yOffset = yOffset;
        return this;
    }

    private void UpdateBar()
    {
        Vector3 updatedLocalScale = _bar.localScale;
        updatedLocalScale.x = _tankProp.CurrentInPercents;
        _bar.localScale = updatedLocalScale;
    }

    private void LateUpdate()
    {
        if (_tankProp == null)
            return;
        transform.rotation = Quaternion.identity;
        Vector2 newPos = _followedObj.position;
        newPos.y += _yOffset;
        transform.position = newPos;
    }

    private void OnDisable()
    {
        if (_tankProp != null)
            _tankProp.ValueChanged -= UpdateBar;
    }
}
