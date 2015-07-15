using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private GameHUD gameHUD;
    private Cursor cursor;
    private MusicPlayer musicPlayer;

    public GameAnalytics gameAnalytics;


    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
        
    }

    public void LoadLevel(string name)
    {
        Debug.Log("load level");
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);

        
        //Brick.breakableCount = 0;
        gameData.PauseGame(false);
        
        levelName = name;
        if (levelName.IndexOf("Level") >= 0)
        {
            musicPlayer.SetInMenu(false);
            Screen.showCursor = false;
        }
        else
        {
            musicPlayer.SetInMenu(true);
            Screen.showCursor = true;
            if (levelName == "LoseScreen")
            {
                //if player got high score, show name dialog instead of loading next level
                if (gameData.GetPlayerScoreRank() < 26)
                {
                    gameHUD.ToggleHighScoreNameDialog(true, levelName);
                    return;
                }
                
            }
        }
        gameAnalytics.LogScreen(levelName);
        Application.LoadLevel(levelName);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        Screen.showCursor = false;
        gameData.SetPlayerReady(false);
        gameData.ResetPlayerLives();
        gameData.ResetPlayerScore();
        musicPlayer.SetInMenu(false);
        //Brick.breakableCount = 0;
        Application.LoadLevel(1);
    }

    public void RestartLevel()
    {
        if (gameData.GetPlayerRemainingLives() <= 0)
        {
            gameData.ResetPlayerScore();
            gameData.ResetPlayerLives();
        }
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);

        gameData.PauseGame(false);
        //Brick.breakableCount = 0;
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "restartLevel", "Restart Level");
        }

        Application.LoadLevel(Application.loadedLevel);
    }

    public void ResetPlayer()
    {
        //gameData.PauseGame(false);
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        gameData.SetPlayerReady(false);
        //playerBall.ShowBallDestruction();
        //playerBall.LockBall();
    }

    public void LoadNextLevel()
    {
        musicPlayer.SetInMenu(false);
        gameData.PauseGame(false);
        //Brick.breakableCount = 0;
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "nextLevel", "Load Next Leve");
        }
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void MainMenu()
    {
        musicPlayer.SetInMenu(true);
        LoadLevel("StartScreen");
    }

    public void QuitRequest()
    {
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "quitGame", "Quit Game");
        }
        Application.Quit();
    }

    public void ShowLevelComplete()
    {
        musicPlayer.SetInMenu(true);
        //playerBall.ShowBallDestruction();
        //playerBall.LockBall();
        gameData.PauseGame(true);
        gameHUD.ShowLevelComplete();
        gameData.GainOneLife();
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "levelComplete", Application.loadedLevelName + " Complete", gameData.GetPlayerScore());
            gameAnalytics.LogScreen("Level Complete");
        }
    }


}
