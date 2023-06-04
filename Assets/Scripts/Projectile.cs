using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private float _timeOfLife;

    public void IgnoreCollisionWith(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
    }

    public void SetInfo(int damage, float timeOfLife)
    {
        _damage = damage;
        _timeOfLife = timeOfLife;
        StartCoroutine(DestroyProjectile());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform superTF = collision.transform;
        while (superTF != null)
        {
            if (superTF.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage _takeDamageObj))
            {
                _takeDamageObj.TakeDamage(_damage);
                break;
            }

            if (superTF.parent == null)
                break;

            superTF = superTF.parent;
        }
        Destroy();
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(_timeOfLife);
        Destroy();
    }

    private void Destroy()
    {
        GameObject hitEffect = Instantiate(GameObject.FindGameObjectWithTag("EffectsContainer").GetComponent<EffectsContainer>().ProjectileHit);
        hitEffect.transform.position = transform.position;
        hitEffect.transform.rotation = transform.rotation;
        Destroy(hitEffect, hitEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
