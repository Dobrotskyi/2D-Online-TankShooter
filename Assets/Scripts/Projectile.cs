using UnityEngine;

public class Projectile : MonoBehaviour
{
    public void IgnoreCollisionWith(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>(), true);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
