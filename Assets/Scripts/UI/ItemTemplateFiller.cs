using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemTemplateFiller : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Color _turretItemColor;
    private PartData _part;
    public PartData PartData => _part;

    public void Fill(PartData data)
    {
        if (data is TurretData)
            GetComponent<Image>().color = _turretItemColor;

        _image.rectTransform.sizeDelta = ImageConverter.CountSizeToKeepAspectRation(data.Sprite.bounds.size, _image.rectTransform.sizeDelta);
        _image.sprite = data.Sprite;
        _title.text = data.Name;
        _description.text = data.GetDescription();
        _part = data;
    }
}
