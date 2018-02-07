using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public delegate void LoadSceneAction(string sceneName);
    public static LoadSceneAction OnLoadScene;

    private void OnEnable()
    {
        OnLoadScene += LoadScene;
    }

    private void OnDisable()
    {
        OnLoadScene -= LoadScene;
    }

    public void LoadScene(string sceneName)
    {
        if(BattleSceneTransition.TransitionIn != null)
            BattleSceneTransition.TransitionIn();

        if(BackgroundMusic.instance != null)
            BackgroundMusic.instance.FadeMusic(false);

        if (EventSystem.current != null)
            EventSystem.current.enabled = false;

        GameStateManager.CurrentGameState = GameState.PAUSED;

        StartCoroutine(TransitionDelay(sceneName));
    }

    private IEnumerator TransitionDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}