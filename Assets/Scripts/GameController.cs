using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour {

    public static GameController s_Instance;

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
    float timestamp_AutoSave;

    //objects
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject inGameMenu;
    public GameObject HUD;
    RectTransform lifeBar;
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
    public bool toggle_AutoSave = true;
    public float duration_AutoSave = 60.0f;

    //files
    SaveData saveData;


    /// UNITY EVENTS ///

    void Start () {
        //set game state
        currentState = State.Load;

        //set refferance
        s_Instance = this;

        //freeze ingame time
        Time.timeScale = 0.0f;

        /// MAKE PERSISTANT ///
        Object.DontDestroyOnLoad(gameObject);//Self
        Object.DontDestroyOnLoad(mainMenu.transform.parent.gameObject);//UI

        /// FIND COMPONENTS ///
        lifeBar = HUD.transform.Find("Life Bar").GetComponent<RectTransform>();
        score_displayed = HUD.GetComponentInChildren<Text>();

        /// LOAD MAIN MENU ///
        SceneManager.LoadScene("MainMenu");
        mainMenu.SetActive(true);

        //set timestamps
        timestamp_AutoSave = Time.unscaledTime;

        //load general save data
        LoadData();

        //set game state
        currentState = State.MainMenu;
    }

    void Update()
    {
        // if main game
        if (currentState >= State.MainGame)
        {
            //check if pauseing
            if (currentState == State.MainGame && Input.GetButtonDown("Pause"))
            {
                if (paused)
                    ContinueGame();
                else
                    PauseGame();
            }

            if (!paused)
            {
                //check if defeated
                if (current_life <= 0)
                {
                    if (currentState == State.MainGame)//prevent redundant call
                        Defeated();
                }
                else
                {
                    //check if a power up expired
                    if (power_up && duration_PowerUP < Time.time - timestamp_PowerUP)
                    {
                        power_up = false;
                    }
                }
            }

            //check if beaten highscore
            if (current_score > saveData.highScore)
            {
                saveData.highScore = current_score;
            }

            //check if time to auto save
            if (toggle_AutoSave && currentState == State.MainGame && duration_AutoSave < Time.unscaledTime - timestamp_AutoSave)
            {
                AutoSave();
                timestamp_AutoSave = Time.unscaledTime;
            }
        }
    }



    ///  TRANSITIONS  ///

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

    //called from UI to quit the game and app
    public void QuitGame()
    {
        //save game
        SaveData();

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //called from UI to quit the game and return to menu
    public void QuitToMain()
    {
        //freeze ingame time (if active)
        Time.timeScale = 0.0f;

        //close in game menu (if open)
        inGameMenu.SetActive(false);

        //close hud
        HUD.SetActive(false);

        //save game
        SaveData();

        //exit level
        SceneManager.LoadScene("MainMenu");

        //open main menu
        mainMenu.SetActive(true);

        //set game state
        currentState = State.MainMenu;
    }

    //called when player is defeated
    public void Defeated()
    {
        //freeze ingame time (if active)
        Time.timeScale = 0.0f;

        //close in game menu (if open)
        inGameMenu.SetActive(false);

        //save game
        SaveData();

        //load fail scene as overlay
        SceneManager.LoadScene("YouLose", LoadSceneMode.Additive);

        //set game state
        currentState = State.LoseScence;
    }

    //called when player exits the win/lose cutscene
    public void ReturnToMain()
    {
        //close hud (if active)
        HUD.SetActive(false);

        //exit level
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




    ///  PLAYER STATUS  ///

    void setLife(int value)
    {
        current_life = value;
        lifeBar.sizeDelta = new Vector2(100.0f * (float)current_life, lifeBar.sizeDelta.y);
    }

    internal bool addLife()
    {
        if (current_life >= max_life)
            return false;
        else
        {
            ++current_life;
            lifeBar.sizeDelta = new Vector2(100.0f * (float)current_life, lifeBar.sizeDelta.y);
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
            lifeBar.sizeDelta = new Vector2(100.0f * (float)current_life, lifeBar.sizeDelta.y);
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




    ///  SAVE  ///

    internal string save_FilePath()
    {
        return Application.persistentDataPath + "/save.json";
    }

    internal void LoadData()
    {
        // Test if file has been created
        if (File.Exists(save_FilePath()))
        {
            // Read file
            string data = File.ReadAllText(save_FilePath());
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
        else
        {
            // Set defaults
            saveData = new SaveData();
            saveData.highScore = 0;

            // Create default save file
            SaveData();
        }

        
    }

    internal void SaveData()
    {
        Debug.Log("Save");
        string data = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(save_FilePath(), data);
    }

    internal void AutoSave()
    {
        Debug.Log("Auto-Save");
        SaveData();
    }
}
