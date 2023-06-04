using TMPro;
using UnityEngine;

public class UserCoinsDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _labelField;

    public void UpdateCoinsAmt()
    {
        _labelField.text = DBManager.Money.ToString();
    }

    private void Start()
    {
        UpdateCoinsAmt();
        DBManager.MoneyChanged += UpdateCoinsAmt;
    }

    private void OnDisable()
    {
        DBManager.MoneyChanged -= UpdateCoinsAmt;
    }
}
