using UnityEngine;

public class PropertyBar : MonoBehaviour
{
    private float _yOffset = 0.8f;
    private Transform _followedObj;
    [SerializeField] protected Transform _bar;
    protected TankProperty _property;

    public PropertyBar SetProperty(TankProperty tankProp)
    {
        if (_property != null)
            return this;

        _property = tankProp;
        _property.ValueChanged += UpdateBar;
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
        updatedLocalScale.x = _property.CurrentInPercents;
        _bar.localScale = updatedLocalScale;

        AdditionalUpdate();
    }

    protected virtual void AdditionalUpdate()
    { }

    private void LateUpdate()
    {
        if (_property == null)
            return;
        transform.rotation = Quaternion.identity;
        Vector2 newPos = _followedObj.position;
        newPos.y += _yOffset;
        transform.position = newPos;
    }

    private void OnDisable()
    {
        if (_property != null)
            _property.ValueChanged -= UpdateBar;
    }
}
