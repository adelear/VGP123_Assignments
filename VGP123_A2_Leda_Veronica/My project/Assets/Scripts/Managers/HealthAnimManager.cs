using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnimManager : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.OnLifeValueChanged.AddListener(UpdateHealthUI); 
        InitializeHealthUI();   
    }

    // Update is called once per frame
    private void InitializeHealthUI()
    {
        UpdateHealthUI(GameManager.Instance.Lives); 
    }
    private void UpdateHealthUI(int lives) 
    {
        switch (lives)
        {
            case 1:
                anim.Play("1Health");
                break;
            case 2:
                anim.Play("2Health");
                break;
            case 3:
                anim.Play("3Health");
                break;
            default:
                anim.Play("Health");
                break; 
        }
    }
}
