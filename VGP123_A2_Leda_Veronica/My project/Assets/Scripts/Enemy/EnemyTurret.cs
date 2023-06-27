using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class EnemyTurret : Enemy
{
    Shoot shootScript;
    float timeSinceLastFire = 0.0f;
    AudioSourceManager asm;
    public AudioClip dieSound; 
    public float projectileFireRate;
    public float firingRange;
    public float turretFireDistance;
    public string playerTag;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        asm = GetComponent<AudioSourceManager>(); 
        shootScript = GetComponent<Shoot>();
        shootScript.OnProjectileSpawned.AddListener(UpdateTimeSinceLastFire);

        if (projectileFireRate <= 0.0f)
            projectileFireRate = 2.0f;

        if (firingRange <= 0.0f)
            firingRange = 20.0f;

        if (turretFireDistance <= 0.0f)
            turretFireDistance = 10.0f;

        if (string.IsNullOrEmpty(playerTag))
            playerTag = "Player";
    }

    // Update is called once per frame
    private void Update()
    {
        AnimatorClipInfo[] curClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curClips[0].clip.name != "PlantShoot")
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag); // Find player using the player tag

            if (playerObject)
            {
                Vector3 playerDirection = playerObject.transform.position - transform.position;

                if (playerDirection.x < 0)
                {
                    // Player is on the left, fire left
                    Debug.Log("Turret fires left!");
                    sr.flipX = false;
                }
                else if (playerDirection.x > 0)
                {
                    // Player is on the right, fire right
                    Debug.Log("Turret fires right!");
                    sr.flipX = true;
                }

                float distance = Vector3.Distance(transform.position, playerObject.transform.position);

                if (distance <= firingRange && distance <= turretFireDistance && Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Shoot");
                }
                else
                {
                    // Player is out of range, stop firing
                    Debug.Log("Player out of range, stop firing");
                }
            }
        }

    }

    public override void Death()
    {
        asm.PlayOneShot(dieSound, false);
        // Delay before destroying the enemy GameObject
        float destroyDelay = dieSound.length; // Use the length of the death sound as the delay
        Destroy(gameObject, destroyDelay); 
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

    public void DestroyMyself()
    {
        Destroy(transform.parent.gameObject);
    }

    public void Squish()
    {
        anim.SetTrigger("Squish");
    }
}
