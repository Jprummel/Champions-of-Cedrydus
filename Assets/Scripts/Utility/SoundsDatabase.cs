/*
	SoundsDatabase.cs
	Created 9/28/2017 1:32:53 PM
	Project Resource Collector by Base Games
*/
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class SoundsDatabase : MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> _audioClips;

        public static SoundsDatabase instance;

        public static Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            AudioClips.Clear();
            AudioClips = new Dictionary<string, AudioClip>();
            for (int i = 0; i < _audioClips.Count; i++)
            {
                AudioClips.Add(_audioClips[i].name, _audioClips[i]);
            }
        }
    }
}