using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameField;
    [SerializeField] private TextMeshProUGUI _scoreField;
    [SerializeField] private List<GameObject> _medals = new();
    private int _kills = 0;
    public int Kills => _kills;

    public void Initialize(Photon.Realtime.Player player)
    {
        _nicknameField.text = player.NickName;
        _scoreField.text = "0";
    }

    public void AddKill()
    {
        _kills++;
        _scoreField.text = _kills.ToString();
    }

    public void SetSiblingIndex(int index)
    {
        transform.SetSiblingIndex(index);

        if (DeservesMedal(index))
            _medals[index].gameObject.SetActive(true);
        else
            TurnOffMedals();
    }

    private void Start()
    {
        TurnOffMedals();
    }

    private bool DeservesMedal(int index) => index < _medals.Count && index + 1 < transform.parent.childCount && _kills > 0;

    private void TurnOffMedals()
    {
        foreach (var item in _medals)
        {
            if (item.gameObject.activeSelf)
                item.SetActive(false);
        }
    }
}
