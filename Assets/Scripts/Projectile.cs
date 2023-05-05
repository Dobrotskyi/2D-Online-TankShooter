using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage = 10;
    
    public void IgnoreCollisionWith(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform superTF = collision.transform;
        while (superTF.parent != null)
        {
            if (superTF.gameObject.TryGetComponent<ITakeDamage>(out ITakeDamage _takeDamageObj))
            {
                _takeDamageObj.TakeDamage(_damage);
                break;
            }
            superTF = superTF.parent;
        }

        Destroy(gameObject);
    }
}
