/*
	SetSpinnerItem.cs
	Created 12/8/2017 1:44:44 PM
	Project Boardgame by Base Games
*/
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SetSpinnerItem : MonoBehaviour 
	{
        [SerializeField] private Text _spinnerItemText;
        [SerializeField] private Image _spinnerItemImage;

        public void SetTextItem(string itemText)
        {
            _spinnerItemText.text = itemText;
            _spinnerItemText.gameObject.SetActive(true);
        }

        public void SetImageItem(Sprite itemImage)
        {
            _spinnerItemImage.sprite = itemImage;
            _spinnerItemImage.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _spinnerItemText.gameObject.SetActive(false);
            _spinnerItemImage.gameObject.SetActive(false);
        }
    }
}