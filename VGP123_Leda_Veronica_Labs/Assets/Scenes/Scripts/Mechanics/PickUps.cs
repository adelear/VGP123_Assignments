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


    public PickupType currentPickup;

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
            if (currentPickup == PickupType.Powerup2)
            {
                myController.StartSpeedChange();
                Destroy(gameObject);
                return; 
            }
            if (currentPickup == PickupType.Life)
            {
                // do something
                Destroy(gameObject);
                return; 
            }
            // do something in regards to score 
            myController.score++; 
            Destroy(gameObject); 
        }
    }
}
