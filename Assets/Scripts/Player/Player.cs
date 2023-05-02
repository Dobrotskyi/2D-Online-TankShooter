using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private MainPart _mainTankPart;

    public MainPart GetMainPart()
    {
        return _mainTankPart;
    }

    private void OnEnable()
    {
        _mainTankPart.SpawnPart(this.transform, this);
    }

}
