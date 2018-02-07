/*
	GameStateManager.cs
	Created 11/10/2017 1:33:58 PM
	Project Boardgame by Base Games
*/
using System.Collections;
using UnityEngine;

public enum GameState
{
    CHARACTERCREATION,
    WORLD,
    COMBAT,
    PLAYING,
    PAUSED
}

public class GameStateManager : MonoBehaviour
{
    public static GameState CurrentGameState;
    public static GameState TimeGameState = GameState.PLAYING;

    //Instance of this script.
    private static GameStateManager s_Instance = null;

    public static GameStateManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameStateManager)) as GameStateManager;
            }


            if (s_Instance == null)
            {
                GameObject obj = new GameObject("GameStateManager");
                s_Instance = obj.AddComponent(typeof(GameStateManager)) as GameStateManager;
            }

            return s_Instance;
        }

        set { }
    }
    
    private void OnApplicationQuit()
    {
        s_Instance = null;
    }

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
        }

        s_Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeTimeState(GameState gameState)
    {
        StartCoroutine(ChangeTimeStateDelay(gameState));
    }

    private IEnumerator ChangeTimeStateDelay(GameState gameState)
    {
        yield return new WaitForEndOfFrame();
        TimeGameState = gameState;
    }
}