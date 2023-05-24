using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class EnemyTurret : Enemy
{
    Shoot shootScript;
    float timeSinceLastFire = 0.0f;

    public float projectileFireRate;
    public float firingRange;
    public Transform player;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        shootScript = GetComponent<Shoot>();
        shootScript.OnProjectileSpawned.AddListener(UpdateTimeSinceLastFire);

        if (projectileFireRate <= 0.0f)
            projectileFireRate = 2.0f;

        if (firingRange <= 0.0f)
            firingRange = 10.0f;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        AnimatorClipInfo[] curClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curClips[0].clip.name != "Shoot")
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate && IsPlayerInRange())
            {
                Vector3 playerDirection = player.position - transform.position;

                if (playerDirection.x < 0)
                {
                    // Player is on the left, fire left
                    Debug.Log("Turret fires left!");
                    //Flip enemy sprite, spawn projectiles on the left 
                    sr.flipX = true;  
                }
                else
                {
                    // Player is on the right, fire right
                    Debug.Log("Turret fires right!");
                    sr.flipX = false; 

                }

                anim.SetTrigger("Shoot");
            }
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    void UpdateTimeSinceLastFire()
    {
        timeSinceLastFire = Time.time;
    }

    private void OnDisable()
    {
        shootScript.OnProjectileSpawned.RemoveListener(UpdateTimeSinceLastFire);
    }

    bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= firingRange;
    }
}
