using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class MainDataBuilder
{
    private static string PHP_URL = "http://localhost/topdowntankshooter/get_selected_main.php";

    private string _name;
    private Sprite _sprite;
    private float _acceleration = -1;
    private float _maxSpeed = -1;
    private float _angularSpeed = -1;
    private float _durability = -1;
    private int _ammoStorage = -1;
    private Vector2 _turretPlacement;

    public MainDataBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public MainDataBuilder SetSprite(Sprite sprite)
    {
        _sprite = sprite;
        return this;
    }

    public MainDataBuilder SetAcceleration(float acceler)
    {
        _acceleration = acceler;
        return this;
    }

    public MainDataBuilder SetMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
        return this;
    }

    public MainDataBuilder SetAngularSpeed(float angularSpeed)
    {
        _angularSpeed = angularSpeed;
        return this;
    }

    public MainDataBuilder SetDurability(float durability)
    {
        _durability = durability;
        return this;
    }

    public MainDataBuilder SetCapacity(int capacity)
    {
        _ammoStorage = capacity;
        return this;
    }

    public MainDataBuilder SetTurretPlace(Vector2 turretPlacement)
    {
        _turretPlacement = turretPlacement;
        return this;
    }

    public static IEnumerator GetSelectedByUserPart(MainDataBuilder builder)
    {
        WWWForm form = new();
        form.AddField("nickname", DBManager.LoginedUserName);

        UnityWebRequest uwr = UnityWebRequest.Post(PHP_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            Debug.Log(uwr.downloadHandler.text);
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] != '0')
        {
            Debug.Log("Smth went wrong");
            Debug.Log(result);
        }
        else
        {
            Debug.Log("Main part data was sucessfuly readed");
            string[] info = result.Split(',');
            info[0].Substring(1);

            Vector2 turretPlacement = new Vector2(float.Parse(info[6], CultureInfo.InvariantCulture), float.Parse(info[7], CultureInfo.InvariantCulture));
            int i = 0;
            builder.SetName(info[i++]).SetAcceleration(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                    SetMaxSpeed(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                    SetAngularSpeed(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                    SetDurability(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                    SetCapacity(int.Parse(info[i++], CultureInfo.InvariantCulture)).
                    SetTurretPlace(turretPlacement).SetSprite(ImageLoader.MakeSprite(info[8], new Vector2(0.5f, 0.5f)));
        }

        uwr.Dispose();
        yield break;
    }

    public MainData Build()
    {
        if (Verify())
            return new MainData(_name, _acceleration, _maxSpeed, _angularSpeed,
                _durability, _ammoStorage, _turretPlacement, _sprite);
        else throw new System.Exception("Data was not full");
    }

    private bool Verify()
    {
        if (_name == null)
            return false;
        if (_sprite == null)
            return false;
        if (_acceleration == -1)
            return false;
        if (_maxSpeed == -1)
            return false;
        if (_angularSpeed == -1)
            return false;
        if (_durability == -1)
            return false;
        if (_ammoStorage == -1)
            return false;
        if (_turretPlacement == null)
            return false;
        return true;
    }

}
