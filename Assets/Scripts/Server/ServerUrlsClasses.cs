public abstract class ServerURLS
{
    public abstract string LOGIN_URL { get; }
    public abstract string REG_URL { get; }

    public abstract string SELECTED_TURRET_URL { get; }
    public abstract string SELECTED_MAIN_URL { get; }

    public abstract string GET_TURRETS_BY_CATEGORY_URL { get; }
    public abstract string GET_MAINS_BY_CATEGORY_URL { get; }

    public abstract string ADD_PART_TO_USER_URL { get; }
    public abstract string SELECT_NEW_PART_URL { get; }

    public abstract string GET_SELECTED_IDS_URL { get; }
    public abstract string GET_MONEY_URL { get; }

    public abstract string ADD_MONEY_URL { get; }
}

public class LocalServerURLS : ServerURLS
{
    public override string LOGIN_URL => "http://localhost/topdowntankshooter/phps/login.php";

    public override string REG_URL => "http://localhost/topdowntankshooter/phps/register.php";

    public override string SELECTED_TURRET_URL => "http://localhost/topdowntankshooter/phps/get_selected_turret.php";

    public override string SELECTED_MAIN_URL => "http://localhost/topdowntankshooter/phps/get_selected_main.php";

    public override string GET_TURRETS_BY_CATEGORY_URL => "http://localhost/topdowntankshooter/phps/get_turrets_by_category.php";

    public override string GET_MAINS_BY_CATEGORY_URL => "http://localhost/topdowntankshooter/phps/get_mains_by_category.php";

    public override string ADD_PART_TO_USER_URL => "http://localhost/topdowntankshooter/phps/add_part_to_user.php";

    public override string SELECT_NEW_PART_URL => "http://localhost/topdowntankshooter/phps/select_new_part.php";

    public override string GET_SELECTED_IDS_URL => "http://localhost/topdowntankshooter/phps/get_selected_ids.php";

    public override string GET_MONEY_URL => "http://localhost/topdowntankshooter/phps/get_money.php";

    public override string ADD_MONEY_URL => "http://localhost/topdowntankshooter/phps/add_money.php";
}

public class WebServerURLS : ServerURLS
{
    public override string LOGIN_URL => "https://tankshooter.000webhostapp.com/phps/login.php";
    public override string REG_URL => "https://tankshooter.000webhostapp.com/phps/register.php";

    public override string SELECTED_TURRET_URL => "https://tankshooter.000webhostapp.com/phps/get_selected_turret.php";
    public override string SELECTED_MAIN_URL => "https://tankshooter.000webhostapp.com/phps/get_selected_main.php";

    public override string GET_TURRETS_BY_CATEGORY_URL => "https://tankshooter.000webhostapp.com/phps/get_turrets_by_category.php";
    public override string GET_MAINS_BY_CATEGORY_URL => "https://tankshooter.000webhostapp.com/phps/get_mains_by_category.php";

    public override string ADD_PART_TO_USER_URL => "https://tankshooter.000webhostapp.com/phps/add_part_to_user.php";
    public override string SELECT_NEW_PART_URL => "https://tankshooter.000webhostapp.com/phps/select_new_part.php";

    public override string GET_SELECTED_IDS_URL => "https://tankshooter.000webhostapp.com/phps/get_selected_ids.php";
    public override string GET_MONEY_URL => "https://tankshooter.000webhostapp.com/phps/get_money.php";

    public override string ADD_MONEY_URL => "https://tankshooter.000webhostapp.com/phps/add_money.php";
}
