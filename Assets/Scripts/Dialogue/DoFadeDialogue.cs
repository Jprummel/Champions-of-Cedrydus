using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
	public class DoFadeDialogue : MonoBehaviour 
	{
        [SerializeField] private List<Graphic> _fadingItems;

        private void OnEnable()
        {
            FadeAll();
        }

        private void FadeAll()
        {
            for(int j = 0; j < _fadingItems.Count; j++)
            {
                _fadingItems[j].DOFade(0f, 0f);
            }

            for (int i = 0; i < _fadingItems.Count; i++)
            {
                _fadingItems[i].DOFade(1f, 1f);
            }
        }
    }
}