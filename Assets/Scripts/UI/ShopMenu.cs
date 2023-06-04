using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    private const char DETERM = '\t';

    public int SelectedMain { get; private set; }
    public int SelectedTurret { get; private set; }

    [SerializeField] private GameObject[] _itemTemplates;
    [SerializeField] private GameObject _content;
    [SerializeField] private Toggle[] _toggles;
    [SerializeField] private ToggleGroup _group;
    [SerializeField] private TextMeshProUGUI _textTMP;
    private Toggle _lastToggled;

    public void ChangeContent()
    {
        Toggle selected = _group.GetFirstActiveToggle();
        if (_lastToggled == null)
            _lastToggled = selected;
        else if (_lastToggled == selected)
            return;
        _lastToggled = selected;
        ChangeContentWithoutCheck();
    }

    public void ChangeContentWithoutCheck()
    {
        if (_lastToggled == _toggles[0] && _toggles[0].isOn)
            StartCoroutine(DBManager.MakeCallGetSelectedIDs());
        ClearContent();
        ShowParts();
    }

    public void UpdateSelectedCategorie()
    {
        if (_toggles[0].isOn)
            _textTMP.text = "Items you own";
        else
            _textTMP.text = "Items at store";
    }

    private void ClearContent()
    {
        foreach (Transform child in _content.transform)
            Destroy(child.gameObject);
    }

    private void Start()
    {
        ShowParts();
        UpdateSelectedCategorie();
    }

    private void ShowParts()
    {
        StartCoroutine(MakeCallToDB(new TurretDataBuilder(), DBManager.GET_TURRETS_BY_CATEGORY_URL));
        StartCoroutine(MakeCallToDB(new MainPartDataBuilder(), DBManager.GET_MAINS_BY_CATEGORY_URL));
    }

    private IEnumerator MakeCallToDB(ObjectFromDBBuilder builder, string url)
    {
        PHPCaller caller = new(url);
        Dictionary<string, string> parameters = new Dictionary<string, string>() { { "nickname", DBManager.LoginedUserName },
                                                                                        {"purchased", _toggles[0].isOn.ToString() } };
        StartCoroutine(caller.MakeCallWithParameters(parameters));
        while (caller.ResultStatus == UnityWebRequest.Result.InProgress)
            yield return null;
        if (caller.ResultStatus != UnityWebRequest.Result.Success)
            yield return new System.Exception("\"Error occurred while making call to the server: \" + uwr.error");
        DisplayAllParts(caller.Result, builder);

    }

    private void DisplayAllParts(string[] info, ObjectFromDBBuilder builder)
    {
        List<string> fields = new();
        for (int i = 0; i < info.Length; i++)
        {
            if (fields.Count != 0 && info[i] == DETERM.ToString())
            {
                builder.ParseData(fields.ToArray());
                fields.Clear();
                DisplayItemOnScreen(builder);
                continue;
            }
            fields.Add(info[i]);
        }
    }

    private void DisplayItemOnScreen(ObjectFromDBBuilder builder)
    {
        GameObject template;
        if (_group.GetFirstActiveToggle() == _toggles[0])
            template = _itemTemplates[0];
        else
            template = _itemTemplates[1];

        GameObject item = Instantiate(template, _content.transform);
        item.GetComponent<ItemTemplateFiller>().Fill(builder.Build());
    }
}
