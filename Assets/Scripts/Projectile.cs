using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage = -1;

    public void IgnoreCollisionWith(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
    }

    public void SetDamage(int damage)
    {
        if (_damage < 0)
            _damage = damage;
        else
            throw new System.Exception("Damage amount was already set");
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

        Destroy(gameObject);
    }
}
