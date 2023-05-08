using UnityEngine;

public class PropertyBar : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    private TankProperty _tankProp;

    public void SetProperty(TankProperty tankProp)
    {
        _tankProp = tankProp;
        _tankProp.ValueChanged += UpdateBar;
    }

    private void UpdateBar()
    {
        Vector3 updatedLocalScale = _bar.localScale;
        updatedLocalScale.x = _tankProp.CurrentInPercents;
        _bar.localScale = updatedLocalScale;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        if (_tankProp != null)
            _tankProp.ValueChanged -= UpdateBar;
    }
}
