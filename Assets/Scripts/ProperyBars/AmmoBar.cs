using UnityEngine;

public class AmmoBar : PropertyBar
{
    [SerializeField] private Color _notEnoughAmmoColor;
    [SerializeField] private SpriteRenderer _barSR;
    private Color _basicColor;

    private void OnEnable()
    {
        _basicColor = _barSR.color;
    }

    protected override void AdditionalUpdate()
    {
        if ((_property as AmmoStorage).AmmoNotEnough)
            _barSR.color = _notEnoughAmmoColor;
        else
            if (_barSR.color != _basicColor)
            _barSR.color = _basicColor;

    }
}
