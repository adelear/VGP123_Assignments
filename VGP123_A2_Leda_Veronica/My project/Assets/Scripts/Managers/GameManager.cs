using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    AudioSourceManager asm;
    public AudioClip dieSound;
    public AudioClip gameOverMusic;
    public AudioSource audioSource;

    static GameManager _instance = null;
    
    public static GameManager Instance
    {
        get => _instance;
        set
        {
            _instance = value;
        }
    }

    public int Lives
    {
        get => _lives;
        set
        {
            if (_lives > value) Respawn();

            _lives = value;

            if (_lives > maxLives) _lives = maxLives;

            Debug.Log("Lives value has changed to " + _lives.ToString());
            if (_lives < 0) GameOver();

            //Invoke an event here to listen to life value change
            if (OnLifeValueChanged != null)
                OnLifeValueChanged.Invoke(_lives);
        }
    }
    private int _lives = 3;
    public int maxLives = 3;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;

            Debug.Log("Score value has changed to " + _score.ToString());

            if (OnScoreValueChanged != null)
            {
                OnScoreValueChanged.Invoke(_score);
            } 
        }
    }
    private int _score = 0;

    public PlayerController playerPrefab;

    public PlayerController PlayerInstance
    {
        get => playerInstance;
    }
    private PlayerController playerInstance;

    [HideInInspector] public Transform spawnPoint;
    public UnityEvent<int> OnLifeValueChanged;
    public UnityEvent<int> OnScoreValueChanged; 

    // Start is called before the first frame update
    void Start()
    {
        asm = GetComponent<AudioSourceManager>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        UpdateSpawnPoint(spawnLocation);
    }

    public void UpdateSpawnPoint(Transform updatedPoint)
    {
        spawnPoint = updatedPoint;
    }


    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        //gameObject.GetComponent<CanvasManager>().GameOverMusic();  

        if (SceneManager.GetActiveScene().name == "GameOver")
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Title");
                Lives = 3;
                Score = 0;
                //Set Lives, and Score to zero
            }
        //go to game over scene
    }


    void Respawn()
    {
        playerInstance.transform.position = spawnPoint.position;
    }


    public void TakeDamage()
    {
        asm.PlayOneShot(dieSound, false);
        Lives -= 1;
        RespawnPlayer();
    }


    public void RespawnPlayer()
    {
        if (playerInstance != null)
        {
            playerInstance.transform.position = spawnPoint.position;
        }
    }

}