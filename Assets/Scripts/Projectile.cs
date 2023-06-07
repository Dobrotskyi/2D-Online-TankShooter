using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private float _timeOfLife;
    private float _multiplier;
    private string _shooterNickname;

    public string ShooterName
    {
        get => _shooterNickname;
        set
        {
            if (string.IsNullOrEmpty(_shooterNickname))
                _shooterNickname = value;
        }
    }

    public void IgnoreCollisionWith(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
    }

    public void SetInfo(int damage, float timeOfLife, float multiplier)
    {
        _damage = damage;
        _timeOfLife = timeOfLife;
        _multiplier = multiplier;
        StartCoroutine(DestroyProjectile());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform superTransf = collision.transform;
        while (superTransf != null)
        {
            if (superTransf.gameObject.TryGetComponent(out ITakeDamageFromPlayer _takeDamageObj))
            {
                _takeDamageObj.TakeDamage(_damage, _shooterNickname);
                break;
            }

            if (superTransf.parent == null)
                break;

            superTransf = superTransf.parent;
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
        hitEffect.transform.localScale *= _multiplier;
        Destroy(hitEffect, hitEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
