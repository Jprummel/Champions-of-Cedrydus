/*
	SoundManager.cs
	Created 9/28/2017 12:51:13 PM
	Project Resource Collector by Base Games
*/
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Utility
{
    public class SoundManager : MonoBehaviour
    {
        //Instance of this script.
        private static SoundManager s_Instance = null;

        /// <summary>
        /// Instantiates a new SoundManager if one cannot be found.
        /// </summary>
        public static SoundManager instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
                }


                if (s_Instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    s_Instance = obj.AddComponent(typeof(SoundManager)) as SoundManager;
                }

                return s_Instance;
            }

            set { }
        }

        /// <summary>
        /// Removes the instance of the SoundManager 
        /// </summary>
        void OnApplicationQuit()
        {
            s_Instance = null;
        }

        /// <summary>
        /// Loads the sound clips from the Resources/Audio folder so they can be used.
        /// Also makes sure the sound manager persists through every scene.
        /// </summary>
        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
            }

            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Plays the given at default volume and pitch if no volume and pitch are given.
        /// </summary>
        /// <param name="clipToPlay">The clip that will be played.</param>
        /// <param name="volume">The volume at which the clip will be played.</param>
        /// <param name="pitch">The pitch at which the clip will be played.</param>
        public void PlaySound(AudioClip clipToPlay, float volume = 1f, float pitch = 1f, bool fadeOut = false, float fadeDuration = 0.5f)
        {
            StartCoroutine(SimultaneousSound(clipToPlay, volume, pitch));
        }

        /// <summary>
        /// Plays the given at default volume and pitch if no volume and pitch are given.
        /// Plays simultaneously with other sounds.
        /// </summary>
        /// <param name="clipToPlay">The clip that will be played.</param>
        /// <param name="volume">The volume at which the clip will be played.</param>
        /// <param name="pitch">The pitch at which the clip will be played.</param>
        private IEnumerator SimultaneousSound(AudioClip clipToPlay, float volume = 1f, float pitch = 1f, bool fadeOut = false, float fadeDuration = 0.5f)
        {
            AudioSource tempAS = gameObject.AddComponent<AudioSource>();
            tempAS.clip = clipToPlay;
            tempAS.volume = volume;
            tempAS.pitch = pitch;
            tempAS.Play();
            yield return new WaitForSeconds(clipToPlay.length);
            if (fadeOut)
            {
                FadeSound(tempAS);
                yield return new WaitForSeconds(fadeDuration);
            }
            Destroy(tempAS);
        }

        public void FadeSound(AudioSource source, float fadeDuration = 0.5f)
        {
            source.DOFade(0, fadeDuration);
        }
    }
}