using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BackgroundMusicClip
{
    public string SceneName;
    public AudioClip MusicClip;
}

public class BackgroundMusic : MonoBehaviour
{
    //Instance of this script.
    private static BackgroundMusic s_Instance = null;
    
    public static BackgroundMusic instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(BackgroundMusic)) as BackgroundMusic;
            }


            if (s_Instance == null)
            {
                GameObject obj = new GameObject("BackgroundMusic");
                s_Instance = obj.AddComponent(typeof(BackgroundMusic)) as BackgroundMusic;
            }

            return s_Instance;
        }

        set { }
    }
    
    void OnApplicationQuit()
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

        _backgroundAudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < _backgroundMusic.Length; i++)
        {
            _musicDictionary.Add(_backgroundMusic[i].SceneName, _backgroundMusic[i].MusicClip);
        }
        FadeMusic(true);
    }

    private AudioSource _backgroundAudioSource;

    [SerializeField] private BackgroundMusicClip[] _backgroundMusic;
    private Dictionary<string, AudioClip> _musicDictionary = new Dictionary<string, AudioClip>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode lsMode)
    {
        _backgroundAudioSource.clip = _musicDictionary[SceneManager.GetActiveScene().name];
        _backgroundAudioSource.Play();
        FadeMusic(true);
    }

    public void FadeMusic(bool fadeIn)
    {
        if(fadeIn)
            _backgroundAudioSource.DOFade(1f, 1f);
        else
            _backgroundAudioSource.DOFade(0f, 1f);
    }
}