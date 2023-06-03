using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject _itemTemplate;
    [SerializeField] private GameObject _content;
    [SerializeField] private Toggle[] _toggles;

    private const char DETERM = '\t';

    public void ChangeContent()
    {
        ClearContent();
        ShowParts();

        if (_toggles[1].isOn)
        {
            _toggles[1].interactable = false;
            _toggles[0].interactable = true;
        }
        else if (_toggles[0].isOn)
        {
            _toggles[0].interactable = false;
            _toggles[1].interactable = true;
        }
    }

    private void ClearContent()
    {
        foreach (Transform child in _content.transform)
            Destroy(child.gameObject);
    }

    private void Start()
    {
        ShowParts();
    }

    private void ShowParts()
    {
        StartCoroutine(MakeCallToDB(new TurretDataBuilder(), DBManager.TURRETS_IN_STORE_URL));
        StartCoroutine(MakeCallToDB(new MainPartDataBuilder(), DBManager.MAIN_IN_STORE_URL));
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
        GameObject item = Instantiate(_itemTemplate, _content.transform);
        item.GetComponent<ItemTemplateFiller>().Fill(builder.Build());
    }
}
