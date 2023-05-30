using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.ComponentModel;
using UnityEngine.UIElements;

public class TurretDataBuilder
{
    private static string PHP_URL = "http://localhost/topdowntankshooter/get_selected_turret.php";
    public enum Status
    {
        Success,
        Failed,
        OnGoing
    }

    private Status _status = Status.OnGoing;

    public Status GetStatus => _status;

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

    public static TurretData GetSelectedByUser(MonoBehaviour sender)
    {
        TurretDataBuilder builder = new();
        sender.StartCoroutine(MakeCall(builder));

        return builder.Build();
    }

    private static IEnumerator MakeCall(TurretDataBuilder builder)
    {
        WWWForm form = new();
        form.AddField("nickname", DBManager.LoginedUserName);

        UnityWebRequest uwr = UnityWebRequest.Post(PHP_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            Debug.Log(uwr.downloadHandler.text);
            builder._status = Status.Failed;
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] != '0')
        {
            Debug.Log("Smth went wrong");
            Debug.Log(result);
            builder._status = Status.Failed;
        }

        else
        {
            Debug.Log("Got result");
            result.Remove(0, 1);
            string[] info = result.Split(",");
            Vector2 spread = new Vector2(float.Parse(info[2]), float.Parse(info[3]));
            builder.SetName(info[0]).SetRotationSpeed(float.Parse(info[1]))
                   .SetSpread(spread).SetFireRate(float.Parse(info[4]))
                   .SetShotForce(float.Parse(info[5]))
                   .SetDM(float.Parse(info[6])).SetSprite(ImageLoader.LoadImage(info[7]));
            Debug.Log(builder.Build().Sprite);
            builder._status = Status.Success;
        }

        uwr.Dispose();
        yield break;
    }

    private TurretData Build()
    {
        if (Verify())
            return new TurretData(_name, _rotationSpeed, _spread, _fireRate, _shotForce, _durabilityMultiplier, _sprite);
        else throw new System.Exception("Data was not full");
    }

    private bool Verify()
    {
        //Debug.Log(_rotationSpeed);
        //Debug.Log(_spread);
        //Debug.Log(_fireRate);
        //Debug.Log(_shotForce);
        //Debug.Log(_durabilityMultiplier);
        //Debug.Log(_name);
        //Debug.Log(_sprite);
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
