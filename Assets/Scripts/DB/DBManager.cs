using UnityEngine;

public static class DBManager
{
    public const int MAX_NAME_LENGTH = 10;
    public const int MIN_NAME_LENGTH = 5;
    public const int MIN_PASSW_LENGTH = 5;

    public const string LOGIN_URL = "http://localhost/topdowntankshooter/login.php";
    public const string REG_URL = "http://localhost/topdowntankshooter/register.php";

    private static string s_userName;
    private static int s_money;

    public static string Nickname
    {
        get => s_userName;
        set
        {
            if (s_userName == null)
                s_userName = value;
        }
    }

    public static int Money
    {
        get => s_money;
        set => s_money = value;
    }

    public static bool IsLogged() => s_userName != null;

    public static void Logout()
    {
        s_userName = null;
        s_money = -1;
    }


}
