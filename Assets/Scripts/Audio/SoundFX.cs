using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour {

    public delegate void SoundFXEvent();
    public static SoundFXEvent OnButtonClick;

    [SerializeField] private AudioSource _audio;

    private void OnEnable()
    {
        OnButtonClick += PlayAudio;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	void PlayAudio()
    {
        if (!_audio.isPlaying)
        {
            _audio.Play();
        }
    }

    private void OnDisable()
    {
        OnButtonClick -= PlayAudio;
    }
}
