using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime;
    public int damage; 
 
    //meant to be modified by the object creating this projectile (the shoot class) 
    [HideInInspector]
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0) lifetime = 2.0f;
        if (damage <= 0) damage = 1; 
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifetime);

        //On collision


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherCollider = collision.gameObject;
        
        if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerProjectile"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    GameManager.Instance.TakeDamage();
                }
                Destroy(gameObject);
            }
        }
        
        if (gameObject.CompareTag("PlayerProjectile"))
        {

            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("EnemyProjectile"))
            {
                Destroy(gameObject);
            }
            /*
            else if (gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
            */ 
        }
    }

}

