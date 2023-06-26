using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DBManager
{
    public const int REWARD_FOR_KILL = 50;
    public const int MAX_NAME_LENGTH = 10;
    public const int MIN_NAME_LENGTH = 5;
    public const int MIN_PASSW_LENGTH = 5;

    public static event Action MoneyChanged;
    public static ServerURLS ServerURLS
    {
        get
        {
#if UNITY_EDITOR
            return new LocalServerURLS();
#endif

#if UNITY_WEBGL
            return new WebServerURLS();
#endif
            return null;
        }
    }

    private static string s_userName = "admin1";
    private static int s_money;

    public static string LoginedUserName
    {
        get => s_userName;
        set => s_userName = value;
    }

    public static int Money
    {
        get => s_money;
        set
        {
            s_money = value;
            Debug.Log("MoneyChanged?.Invoke();");
            MoneyChanged?.Invoke();
        }
    }

    public static int SelectedTurretID { get; private set; }

    public static int SelectedMainID { get; private set; }

    public static bool IsLogged() => s_userName != null;

    public static IEnumerator MakeCallGetSelectedIDs()
    {
        PHPCaller caller = new(ServerURLS.GET_SELECTED_IDS_URL);
        yield return caller.MakeCallWithNickname(s_userName);
        while (caller.ResultStatus == UnityEngine.Networking.UnityWebRequest.Result.InProgress)
            yield return null;

        SelectedTurretID = int.Parse(caller.Result[0], CultureInfo.InvariantCulture);
        SelectedMainID = int.Parse(caller.Result[1], CultureInfo.InvariantCulture);
    }

    public static IEnumerator UpdateMoneyAmt()
    {
        PHPCaller caller = new(ServerURLS.GET_MONEY_URL);
        yield return caller.MakeCallWithNickname(s_userName);
        while (caller.ResultStatus == UnityEngine.Networking.UnityWebRequest.Result.InProgress)
            yield return null;

        Money = int.Parse(caller.Result[0], CultureInfo.InvariantCulture);
    }

    public static IEnumerator AddMoney(int amt)
    {
        PHPCaller caller = new(ServerURLS.ADD_MONEY_URL);
        Dictionary<string, string> parameters = new() { {"nickname", s_userName },
                                                        {"add_money", amt.ToString()} };
        yield return caller.MakeCallWithParameters(parameters);
    }

    public static void Logout()
    {
        s_userName = null;
        s_money = -1;
        SelectedTurretID = -1;
        SelectedMainID = -1;
    }
}
