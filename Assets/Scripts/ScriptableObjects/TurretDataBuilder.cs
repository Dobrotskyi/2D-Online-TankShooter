using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System.Text;

public class TurretDataBuilder
{
    private static string PHP_URL = "http://localhost/topdowntankshooter/get_selected_turret.php";

    private float _rotationSpeed = -1;
    private Vector2 _spread;
    private float _fireRate = -1;
    private float _shotForce = -1;
    private float _durabilityMultiplier = -1;
    private string _name;
    private Sprite _sprite;

    public TurretDataBuilder() { }

    public TurretDataBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public TurretDataBuilder SetSprite(Sprite sprite)
    {
        _sprite = sprite;
        return this;
    }

    public TurretDataBuilder SetRotationSpeed(float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
        return this;
    }

    public TurretDataBuilder SetSpread(Vector2 spread)
    {
        _spread = spread;
        return this;
    }

    public TurretDataBuilder SetFireRate(float fireRate)
    {
        _fireRate = fireRate;
        return this;
    }

    public TurretDataBuilder SetShotForce(float shotForce)
    {
        _shotForce = shotForce;
        return this;
    }

    public TurretDataBuilder SetDM(float dm)
    {
        _durabilityMultiplier = dm;
        return this;
    }

    public static IEnumerator GetSelectedByUserTurret(TurretDataBuilder builder)
    {
        PHPCaller caller = new(PHP_URL);
        yield return caller.MakeCallWithNickname(DBManager.LoginedUserName);
        while (caller.Result == null)
            yield return null;

        Debug.Log("Turret part data was sucessfuly readed");
        string[] info = caller.Result;
        Vector2 spread = new Vector2(float.Parse(info[2], CultureInfo.InvariantCulture), float.Parse(info[3], CultureInfo.InvariantCulture));
        builder.SetName(info[0]).SetRotationSpeed(float.Parse(info[1], CultureInfo.InvariantCulture))
               .SetSpread(spread).SetFireRate(float.Parse(info[4], CultureInfo.InvariantCulture))
               .SetShotForce(float.Parse(info[5], CultureInfo.InvariantCulture))
               .SetDM(float.Parse(info[6], CultureInfo.InvariantCulture)).SetSprite(ImageLoader.MakeSprite(info[7], new Vector2(0.5f, 0.2f)));

    }

    public TurretData Build()
    {
        if (Verify())
            return new TurretData(_name, _rotationSpeed, _spread, _fireRate, _shotForce, _durabilityMultiplier, _sprite);
        else throw new System.Exception("Data was not full");
    }

    private bool Verify()
    {
        if (_rotationSpeed == -1)
            return false;
        if (_spread == null)
            return false;
        if (_fireRate == -1)
            return false;
        if (_shotForce == -1)
            return false;
        if (_durabilityMultiplier == -1)
            return false;
        if (_name == null)
            return false;
        if (_sprite == null)
            return false;

        return true;
    }

}
