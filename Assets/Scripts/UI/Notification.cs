using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;

    public void DisplayNotification(string text)
    {
        _textField.text = text;
    }
}
