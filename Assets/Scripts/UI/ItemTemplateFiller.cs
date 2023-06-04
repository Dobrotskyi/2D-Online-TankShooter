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
    [SerializeField] private Button _submitBtn;
    private PartData _part;
    public PartData PartData => _part;

    public void Fill(PartData data)
    {
        if (data is TurretData)
        {
            GetComponent<Image>().color = _turretItemColor;
            if (data.Id == DBManager.SelectedTurretID)
                _submitBtn.interactable = false;
        }
        else
            if (data.Id == DBManager.SelectedMainID) _submitBtn.interactable = false;

        _image.rectTransform.sizeDelta = ImageConverter.CountSizeToKeepAspectRation(data.Sprite.bounds.size, _image.rectTransform.sizeDelta);
        _image.sprite = data.Sprite;
        _title.text = data.Name;
        _description.text = data.GetDescription();
        _part = data;
    }
}
