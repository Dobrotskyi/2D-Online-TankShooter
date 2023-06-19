public class RepairCrate : ResourceCrate
{
    public override void UseMeOn(Tank tank)
    {
        tank.RestoreHealth(_capacity);
        DestroyThis();
    }
}
