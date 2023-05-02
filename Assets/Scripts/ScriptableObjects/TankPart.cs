using UnityEngine;

public class TankPart : ScriptableObject
{
    [SerializeField] protected GameObject[] variants;
    protected MonoBehaviour _activeMonoBehaviour;
    protected GameObject _model;
    public GameObject InstantiatedModel => _model;

    public virtual void SpawnPart(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this._activeMonoBehaviour = activeMonoBehaviour;
        System.Random rnd = new System.Random();
        _model = Instantiate(variants[rnd.Next(0, variants.Length - 1)]);
        _model.transform.SetParent(parent);
    }
}
