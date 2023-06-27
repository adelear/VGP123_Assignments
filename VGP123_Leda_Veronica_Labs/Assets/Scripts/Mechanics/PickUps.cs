using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{

    public enum PickupType
    {
        Powerup1,
        Powerup2, 
        Life, 
        Score
    }

    //public AudioClip pickupSound; 
    public PickupType currentPickup;
    public AudioClip pickupSound; 

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<AudioSourceManager>().PlayOneShot(pickupSound,false); //Playshot  

            if (currentPickup == PickupType.Powerup1)
            {
                PlayerController myController = collision.gameObject.GetComponent<PlayerController>(); 
                myController.StartJumpForceChange();
                Destroy(gameObject);
                return; 
            }   
            if (currentPickup == PickupType.Powerup2)
            {
                PlayerController myController = collision.gameObject.GetComponent<PlayerController>(); 
                myController.StartSpeedChange();
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
            // do something in regards to score 
            GameManager.Instance.Score++;  
            Destroy(gameObject); 
        }
    }
}
