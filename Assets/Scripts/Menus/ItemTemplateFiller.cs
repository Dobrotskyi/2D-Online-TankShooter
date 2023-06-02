using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTemplateFiller : MonoBehaviour
{
    private int IMG_SIZE_MULT = 400;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _title;
    public void Fill(PartData data)
    {
        _image.rectTransform.sizeDelta = data.Sprite.bounds.size * IMG_SIZE_MULT;

        _image.sprite = data.Sprite;
        _title.text = data.Name;
        _description.text = data.GetDescription();
    }
}
