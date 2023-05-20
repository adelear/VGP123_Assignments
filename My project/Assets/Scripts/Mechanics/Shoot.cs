using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;

    public float projectileSpeed;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public Projectile projectilePrefab;

    private PlayerController playerController; // Reference to the PlayerController script

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>(); // Find and assign the PlayerController script
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projectileSpeed <= 0)
            projectileSpeed = 7.0f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            Debug.Log("please set default values on " + gameObject.name);
    }

    public void Fire()
    {
        if (playerController.flameMario) // Check if flameMario is true
        {
            if (!sr.flipX)
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
                curProjectile.speed = -projectileSpeed;
            }
            else
            {
                Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
                curProjectile.speed = projectileSpeed;
            }
        }
    }
}
