using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour, IUseLoading
{
    private const char DETERM = '\t';
    private const int CallsAmt = 2;

    public int SelectedMain { get; private set; }
    public int SelectedTurret { get; private set; }

    [SerializeField] private GameObject[] _itemTemplates;
    [SerializeField] private GameObject _content;
    [SerializeField] private Toggle[] _toggles;
    [SerializeField] private ToggleGroup _group;
    [SerializeField] private TextMeshProUGUI _textTMP;
    [SerializeField] private GameObject _loadingScreenPanel;

    private Toggle _lastToggled;

    public event Action StartLoading;
    public event Action EndLoading;
    private int _finishedDisplayingCount = 0;

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

    public void UpdateSelectedCategory()
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
        UpdateSelectedCategory();
    }

    private void ShowParts()
    {
        _finishedDisplayingCount = 0;
        StartLoading?.Invoke();
        StartCoroutine(MakeCallToDB(new TurretDataBuilder(), DBManager.ServerURLS.GET_TURRETS_BY_CATEGORY_URL));
        StartCoroutine(MakeCallToDB(new MainPartDataBuilder(), DBManager.ServerURLS.GET_MAINS_BY_CATEGORY_URL));
    }

    private IEnumerator MakeCallToDB(ObjectFromDBBuilder builder, string url)
    {
#if UNITY_EDITOR
        yield return new WaitForSeconds(1f);
#endif

        PHPCaller caller = new(url);
        Dictionary<string, string> parameters = new Dictionary<string, string>() { { "nickname", DBManager.LoginedUserName },
                                                                                        {"purchased", _toggles[0].isOn.ToString() } };
        StartCoroutine(caller.MakeCallWithParameters(parameters));
        while (caller.ResultStatus == UnityWebRequest.Result.InProgress)
            yield return null;
        if (caller.ResultStatus != UnityWebRequest.Result.Success)
            yield return new System.Exception($"\"Error occurred while making call to the server: \"");
        DisplayAllParts(caller.Result, builder);

        _finishedDisplayingCount++;
        if(_finishedDisplayingCount == CallsAmt)
           EndLoading?.Invoke();
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
                DisplayPartOnScreen(builder);
                continue;
            }
            fields.Add(info[i]);
        }
    }

    private void DisplayPartOnScreen(ObjectFromDBBuilder builder)
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
