using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class CanvasManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button resumeGame;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Text")]
    public Text livesText;
    public Text volSliderText; 

    [Header("Slider")]
    public Slider volSlider; 

    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(StartGame);

        if (settingsButton)
            settingsButton.onClick.AddListener(ShowSettingsMenu);

        if (backButton)
            backButton.onClick.AddListener(ShowMainMenu);
        
        if (quitButton)
            quitButton.onClick.AddListener(Quit); 

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value));
            volSliderText.text = volSlider.value.ToString(); 
        }

        if (livesText)
        {
            GameManager.Instance.OnLifeValueChanged.AddListener((value)=> UpdateLifeText(value));
            livesText.text = "Lives: " + GameManager.Instance.Lives.ToString(); 
        }

        if (resumeGame)
        {
            resumeGame.onClick.AddListener(UnpauseGame);
        }

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(LoadTitle); 
    }

    void LoadTitle()
    {
        SceneManager.LoadScene("Title"); 
    }
    void UnpauseGame()
    {
        Time.timeScale = 1.0f; 
        pauseMenu.SetActive(false);

        //SOMEETHING ELSE aka everything in the game stops
    }
    void UpdateLifeText(int value)
    {
        livesText.text = "Lives: " + value.ToString();
    }
     
    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);   

            if (pauseMenu.activeSelf)
            {
                //do something to pause
                Time.timeScale = 0f;
                pauseMenu.SetActive(true); 
            }

            else
            {
                UnpauseGame();
                //unpause
            }

        }
    }
    void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void ShowMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    void StartGame()
    {
        SceneManager.LoadScene("Level"); 
    }

    void OnSliderValueChanged(float value)
    {
        volSliderText.text = value.ToString();
    }

    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); 
        #endif
    }
}
