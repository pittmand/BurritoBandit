using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    //states
    enum State
    {
        Load,
        MainMenu,
        Intro,
        MainGame,
        CutScence,
        LoseScence,
        WinScene
    };
    State currentState;
    bool paused;
    float timestamp_PowerUP;

    //objects
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject inGameMenu;
    public GameObject HUD;
    Image lifeBar;
    Text score_displayed;

    //stats
    int current_life;
    int current_score;
    bool power_up;

    internal int Current_Life { get { return current_life; } }
    internal int Current_Score { get { return current_score; } }
    internal bool Power_Up { get { return power_up; } }

    //settings
    public int max_life = 6;
    public int starting_life = 3;
    public float duration_PowerUP = 10.0f;

    void Start () {
        //set game state
        currentState = State.Load;

        //freeze ingame time
        Time.timeScale = 0.0f;

        /// MAKE PERSISTANT ///
        Object.DontDestroyOnLoad(gameObject);//Self
        Object.DontDestroyOnLoad(mainMenu.transform.parent.gameObject);//UI

        /// FIND COMPONENTS ///
        lifeBar = HUD.GetComponentInChildren<Image>();
        score_displayed = HUD.GetComponentInChildren<Text>();

        /// LOAD MAIN MENU ///
        SceneManager.LoadScene("MainMenu");
        mainMenu.SetActive(true);

        //set game state
        currentState = State.MainMenu;
    }

    public void StartIntro()
    {
        //close main menu
        mainMenu.SetActive(false);

        //TODO: load intro

        //set game state
        currentState = State.Intro;
    }

    public void StartGame()
    {
        //close main menu (if open)
        mainMenu.SetActive(false);

        //TODO: load level
        SceneManager.LoadScene("LevelOne");

        //set stats
        setLife(starting_life);
        setScore(0);
        power_up = false;

        //enable hud
        HUD.SetActive(true);

        //unfreeze ingame time
        Time.timeScale = 1.0f;
        paused = false;

        //set game state
        currentState = State.MainGame;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void QuitToMain()
    {
        //freeze ingame time (if active)
        Time.timeScale = 0.0f;

        //close hud
        HUD.SetActive(false);

        //close in game menu (if open)
        inGameMenu.SetActive(false);

        //TODO: exit level
        SceneManager.LoadScene("MainMenu");

        //open main menu
        mainMenu.SetActive(true);

        //set game state
        currentState = State.MainMenu;
    }

    public void PauseGame()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0.0f;
            inGameMenu.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        if (paused)
        {
            paused = false;
            Time.timeScale = 1.0f;
            inGameMenu.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void ApplySettings()
    {
        //TODO: apply changes to settings file

        settingsMenu.SetActive(false);
    }

    public void CancelSettings()
    {
        //TODO: revert to settings in file

        settingsMenu.SetActive(false);
    }

    void Update()
    {
        //check if pauseing
        if(currentState == State.MainGame && Input.GetButtonDown("Pause"))
        {
            if (paused)
                ContinueGame();
            else
                PauseGame();
        }

        if (!paused)
        {
            //check if a power up expired
            if (power_up && duration_PowerUP < Time.time - timestamp_PowerUP)
            {

            }
        }

        
    }

    void setLife(int value)
    {
        current_life = value;
        lifeBar.fillAmount = (float)current_life / (float)max_life;
    }

    internal bool addLife()
    {
        if (current_life >= max_life)
            return false;
        else
        {
            ++current_life;
            lifeBar.fillAmount = (float)current_life / (float)max_life;
            return true;
        }
    }

    internal bool removeLife()
    {
        if (current_life <= 0)
            return false;
        else
        {
            --current_life;
            lifeBar.fillAmount = (float)current_life / (float)max_life;
            return true;
        }
    }

    void setScore(int value)
    {
        current_score = value;
        score_displayed.text = "SCORE:" + current_score;
    }

    internal void addScore(int value)
    {
        current_score += value;
        score_displayed.text = "SCORE:" + current_score;
    }

    internal void applyPowerUP()
    {
        power_up = true;
        timestamp_PowerUP = Time.time;
    }
}
