using UnityEngine;

public class TankCollisionHandler : MonoBehaviour
{
    private Tank _tank;

    private void OnEnable()
    {
        _tank = gameObject.GetComponentInParent<Tank>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IResourceCrate>(out IResourceCrate crate))
        {
            crate.UseMeOn(_tank);
        }
    }
}
