using Photon.Pun;
using UnityEngine;

public abstract class PartData
{
    protected string _name = null;
    protected Sprite _sprite;
    protected readonly int _id;

    public string Name => _name.Replace("_", " ");
    public Sprite Sprite => _sprite;
    public int Id => _id;

    public PartData(int id, string name, Sprite sprite)
    {
        _id = id;
        _name = name;
        _sprite = sprite;
    }

    public virtual GameObject SpawnInstance(Transform parent)
    {
        GameObject part = new();
        part.name = _name;
        part.transform.SetParent(parent);
        part.transform.localPosition = Vector3.zero;
        part.transform.localRotation = parent.localRotation;
        SpriteRenderer sr = part.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        sr.sortingOrder = 0;
        sr.transform.localScale = new Vector2(1.3f, 1.3f);

        PhotonView phView = part.AddComponent<PhotonView>();
        PhotonTransformViewClassic viewClassic = part.AddComponent<PhotonTransformViewClassic>();
        if (phView.ObservedComponents == null)
            phView.ObservedComponents = new();
        phView.ObservedComponents.Add(viewClassic);
        phView.Synchronization = ViewSynchronization.Unreliable;


        viewClassic.m_PositionModel.SynchronizeEnabled = true;
        viewClassic.m_RotationModel.SynchronizeEnabled = true;
        viewClassic.m_PositionModel.InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.Lerp;
        viewClassic.m_PositionModel.InterpolateLerpSpeed = 5;
        viewClassic.m_RotationModel.InterpolateOption = PhotonTransformViewRotationModel.InterpolateOptions.Lerp;
        viewClassic.m_RotationModel.InterpolateLerpSpeed = 5;

        if (parent.TryGetComponent(out PhotonView view))
            view.ObservedComponents.Add(viewClassic);

        return part;
    }

    public abstract string GetDescription();
}
