using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTemplateFiller : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Color _turretItemColor;
    [SerializeField] private Button _submitBtn;

    [Header("Can leave empty")]
    [SerializeField] private TextMeshProUGUI _priceDisplayer;

    private PartData _part;
    public PartData PartData => _part;
    public Button SubmitButton => _submitBtn;

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

        if (_priceDisplayer != null)
            _priceDisplayer.text = _part is IBuyable ? (_part as IBuyable).Price.ToString() : "0";
    }

    public void UpdateSelected()
    {
        if (_part is TurretData)
        {
            if (_part.Id == DBManager.SelectedTurretID)
                _submitBtn.interactable = false;
            else
                _submitBtn.interactable = true;
        }
        else
            if (_part.Id == DBManager.SelectedMainID) _submitBtn.interactable = false;
        else
            _submitBtn.interactable = true;
    }

    private void OnEnable()
    {
        if (TryGetComponent(out UserSelectHandler selectHandler))
        {
            UserSelectHandler.SelectTransactionFinished += UpdateSelected;
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent(out UserSelectHandler selectHandler))
        {
            UserSelectHandler.SelectTransactionFinished -= UpdateSelected;
        }
    }
}
