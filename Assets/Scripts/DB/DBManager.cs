using System.Collections;
using System.Globalization;
using UnityEngine;

public static class DBManager
{
    public const int MAX_NAME_LENGTH = 10;
    public const int MIN_NAME_LENGTH = 5;
    public const int MIN_PASSW_LENGTH = 5;

    public const string LOGIN_URL = "http://localhost/topdowntankshooter/login.php";
    public const string REG_URL = "http://localhost/topdowntankshooter/register.php";

    public const string SELECTED_TURRET_URL = "http://localhost/topdowntankshooter/get_selected_turret.php";
    public const string SELECTED_MAIN_URL = "http://localhost/topdowntankshooter/get_selected_main.php";

    public const string GET_TURRETS_BY_CATEGORY_URL = "http://localhost/topdowntankshooter/get_turrets_by_category.php";
    public const string GET_MAINS_BY_CATEGORY_URL = "http://localhost/topdowntankshooter/get_mains_by_category.php";

    public const string ADD_PART_TO_USER_URL = "http://localhost/topdowntankshooter/add_part_to_user.php";
    public const string SELECT_NEW_PART_URL = "http://localhost/topdowntankshooter/select_new_part.php";

    public const string GET_SELECTED_IDS_URL = "http://localhost/topdowntankshooter/get_selected_ids.php";

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

    public static int SelectedTurretID { get; private set; }

    public static int SelectedMainID { get; private set; }

    public static bool IsLogged() => s_userName != null;

    public static IEnumerator MakeCallGetSelectedIDs()
    {
        PHPCaller caller = new(GET_SELECTED_IDS_URL);
        yield return caller.MakeCallWithNickname(s_userName);
        while (caller.ResultStatus == UnityEngine.Networking.UnityWebRequest.Result.InProgress)
        {
            Debug.Log("InProgress");
            yield return null;
        }

        SelectedTurretID = int.Parse(caller.Result[0], CultureInfo.InvariantCulture);
        SelectedMainID = int.Parse(caller.Result[1], CultureInfo.InvariantCulture);
    }

    public static void Logout()
    {
        s_userName = null;
        s_money = -1;
        SelectedTurretID = -1;
        SelectedMainID = -1;
    }
}
