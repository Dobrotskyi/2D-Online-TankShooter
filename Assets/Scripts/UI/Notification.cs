using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;

    public void SetText(string text)
    {
        _textField.text = text;
    }
}
