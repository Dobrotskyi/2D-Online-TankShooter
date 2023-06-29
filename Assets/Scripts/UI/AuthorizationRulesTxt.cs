using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class AuthorizationRulesTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    private void Start()
    {
        StringBuilder sb = new();
        sb.AppendLine($"- Minimum nickname length = {Authorization.MIN_NAME_LENGTH}");
        sb.AppendLine($"- Maximum nickname length = {Authorization.MAX_NAME_LENGTH}");
        sb.AppendLine($"- Minimum password length = {Authorization.MIN_PASSW_LENGTH}");
        _textField.text += sb.ToString();
    }
}
