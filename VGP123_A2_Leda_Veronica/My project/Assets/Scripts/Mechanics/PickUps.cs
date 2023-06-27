using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{

    public enum PickupType
    {
        Powerup1,
        Powerup2,
        Powerup3,
        Powerup4,
        Life,
        Score
    }


    public PickupType currentPickup;
    public AudioClip pickupSound; 

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<AudioSourceManager>().PlayOneShot(pickupSound, false); 
            PlayerController myController = collision.gameObject.GetComponent<PlayerController>();
            if (!myController) return;

            if (currentPickup == PickupType.Powerup1)
            {
                // 
                // 
                myController.StartJumpForceChange();
                Destroy(gameObject);
                return;
            }
            if (currentPickup == PickupType.Powerup2) //USELESS
            {
                myController.StartSpeedChange();
                Destroy(gameObject);
                return;
            }
            
            if (currentPickup == PickupType.Powerup3) // BIG MARIOOO 
            {
                GameManager.Instance.Lives++;  
                myController.bigMario = true;  
                Destroy(gameObject); 
                return;  
            }
            
            if (currentPickup == PickupType.Life)
            {
                // do something
                GameManager.Instance.Lives++;
                Destroy(gameObject);
                return;
            }
            if (currentPickup == PickupType.Powerup4) //FLAME MARIOO 
            {
                myController.flameMario = true; 
                myController.bigMario=true; 
                Destroy(gameObject);
                return;
            }
            // do something in regards to score 
            GameManager.Instance.Score++; 
            Destroy(gameObject);
        }
    }
}
