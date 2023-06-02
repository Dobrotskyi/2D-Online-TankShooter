using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject _itemTemplate;
    [SerializeField] private GameObject _content;
    [SerializeField] private Toggle[] _toggles;

    private const char DETERM = '\t';

    public void ChangeContent()
    {
        if (_toggles[0].gameObject.name == "InStoreToggle")
        {

        }
    }

    private void Start()
    {
        StartCoroutine(MakeCallToDB(new TurretDataBuilder(), DBManager.TURRETS_IN_STORE_URL));
    }

    private IEnumerator MakeCallToDB(ObjectFromDBBuilder builder, string url)
    {
        PHPCaller caller = new(url);
        StartCoroutine(caller.MakeCallWithNickname(DBManager.LoginedUserName));
        while (caller.ResultStatus == UnityWebRequest.Result.InProgress)
            yield return null;
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
