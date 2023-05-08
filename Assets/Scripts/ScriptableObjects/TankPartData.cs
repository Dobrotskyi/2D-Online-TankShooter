using UnityEngine;

public class TankPartData : ScriptableObject
{
    [SerializeField] protected GameObject[] variants;

    public virtual GameObject SpawnPart(Transform parent)
    {
        System.Random rnd = new System.Random();
        GameObject part = Instantiate(variants[rnd.Next(0, variants.Length - 1)], parent.position, parent.rotation);
        part.transform.SetParent(parent);
        return part;
    }
}
