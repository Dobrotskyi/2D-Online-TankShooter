public static class DBManager
{
    public const int MAX_NAME_LENGTH = 10;
    public const int MIN_NAME_LENGTH = 5;
    public const int MIN_PASSW_LENGTH = 5;

    public const string LOGIN_URL = "http://localhost/topdowntankshooter/login.php";
    public const string REG_URL = "http://localhost/topdowntankshooter/register.php";
    public const string SELECTED_TURRET_URL = "http://localhost/topdowntankshooter/get_selected_turret.php";
    public const string SELECTED_MAIN_URL = "http://localhost/topdowntankshooter/get_selected_main.php";
    public const string TURRETS_IN_STORE_URL = "http://localhost/topdowntankshooter/get_all_turrets.php";
    public const string MAIN_IN_STORE = "";

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
        set => s_money = value;
    }

    public static bool IsLogged() => s_userName != null;

    public static void Logout()
    {
        s_userName = null;
        s_money = -1;
    }

}
