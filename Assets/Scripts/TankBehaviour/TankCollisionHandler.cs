using Photon.Pun;
using UnityEngine;

public class TankCollisionHandler : MonoBehaviourPun
{
    private Tank _tank;

    private void OnEnable()
    {
        _tank = gameObject.GetComponentInParent<Tank>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IResourceCrate crate))
            crate.UseMeOn(_tank);

    }
}
