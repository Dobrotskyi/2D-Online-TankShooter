using UnityEngine;
using System.Collections.Generic;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject _itemTemplate;
    [SerializeField] private GameObject _content;

    private const char DETERM = '/';
    private const string PHP_URL = "http://localhost/topdowntankshooter/get_all_turrets.php";

    private void Start()
    {
        PHPCaller caller = new(PHP_URL);
        StartCoroutine(caller.MakeCallWithNickname(DBManager.LoginedUserName));
        DisplayAllParts(caller.Result, new TurretDataBuilder());

    }

    private void DisplayAllParts(string[] info, ObjectFromDBBuilder builder)
    {
        List<string> fields = new();
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i] == DETERM.ToString())
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
