using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTemplateFiller : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _description;
    [SerializeField] private Text _title;
    public void Fill(PartData data)
    {
        _image.sprite = data.Sprite;
        _title.text = data.Name;
        _description.text = data.GetDescription();
    }
}
