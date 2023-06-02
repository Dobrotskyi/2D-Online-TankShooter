using System.Globalization;
using UnityEngine;

public class MainPartDataBuilder : ObjectFromDBBuilder
{
    protected override string PHP_URL => "http://localhost/topdowntankshooter/get_selected_main.php";

    private string _name;
    private Sprite _sprite;
    private float _acceleration = -1;
    private float _maxSpeed = -1;
    private float _angularSpeed = -1;
    private float _durability = -1;
    private int _ammoStorage = -1;
    private Vector2 _turretPlacement;

    public MainPartDataBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public MainPartDataBuilder SetSprite(Sprite sprite)
    {
        _sprite = sprite;
        return this;
    }

    public MainPartDataBuilder SetAcceleration(float acceler)
    {
        _acceleration = acceler;
        return this;
    }

    public MainPartDataBuilder SetMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
        return this;
    }

    public MainPartDataBuilder SetAngularSpeed(float angularSpeed)
    {
        _angularSpeed = angularSpeed;
        return this;
    }

    public MainPartDataBuilder SetDurability(float durability)
    {
        _durability = durability;
        return this;
    }

    public MainPartDataBuilder SetCapacity(int capacity)
    {
        _ammoStorage = capacity;
        return this;
    }

    public MainPartDataBuilder SetTurretPlace(Vector2 turretPlacement)
    {
        _turretPlacement = turretPlacement;
        return this;
    }

    public override void ParseData(string[] info)
    {
        Vector2 turretPlacement = new Vector2(float.Parse(info[6], CultureInfo.InvariantCulture), float.Parse(info[7], CultureInfo.InvariantCulture));
        int i = 0;
        this.SetName(info[i++]).SetAcceleration(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                SetMaxSpeed(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                SetAngularSpeed(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                SetDurability(float.Parse(info[i++], CultureInfo.InvariantCulture)).
                SetCapacity(int.Parse(info[i++], CultureInfo.InvariantCulture)).
                SetTurretPlace(turretPlacement).SetSprite(ImageLoader.MakeSprite(info[8], new Vector2(0.5f, 0.5f)));

    }

    protected override PartData MakePart()
    {
        return new MainPartData(_name, _acceleration, _maxSpeed, _angularSpeed,
                _durability, _ammoStorage, _turretPlacement, _sprite);

    }

    protected override bool Verify()
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
