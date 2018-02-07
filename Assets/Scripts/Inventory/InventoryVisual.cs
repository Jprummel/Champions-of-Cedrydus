/*
	InventoryVisual.cs
	Created 12/13/2017 10:26:46 AM
	Project Boardgame by Base Games
*/
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace Inventory
{
	public class InventoryVisual : MonoBehaviour 
	{
        [SerializeField] private GameObject _inventoryGameObject;

        [SerializeField] private EventSystem _inventoryEventSystem;

        [SerializeField] private Transform[] _inventorySlots;

        private List<GameObject> _currentInventoryItems = new List<GameObject>();

        private TurnManager _turnManager;

        private void OnEnable()
        {
            TurnManager.OnResetTurn += DisplayInventoryItems;
            InputManager.OnBButton += ReturnToMenuCloseInventory;
            UsableItem.OnCloseInventory += CloseInventory;
            PlayerOptionsMenu.OnAccessInventory += OpenInventory;
            PlayerTarget.OnItemUsageCanceled += OpenInventory;
        }

        private void OnDisable()
        {
            TurnManager.OnResetTurn -= DisplayInventoryItems;
            InputManager.OnBButton -= ReturnToMenuCloseInventory;
            UsableItem.OnCloseInventory -= CloseInventory;
            PlayerOptionsMenu.OnAccessInventory -= OpenInventory;
            PlayerTarget.OnItemUsageCanceled -= OpenInventory;
        }

        private void Awake()
        {
            _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
        }

        private void ReturnToMenuCloseInventory()
        {
            if (_inventoryGameObject.activeSelf)
            {
                _inventoryGameObject.SetActive(false);
                if (PlayerOptionsMenu.OnEnableOrDisableMenu != null)
                    PlayerOptionsMenu.OnEnableOrDisableMenu(true);
            }
        }

        private void CloseInventory()
        {
            if (_inventoryGameObject.activeSelf)
            {
                _inventoryGameObject.SetActive(false);
            }  
        }

        private void OpenInventory()
        {
            if (!_inventoryGameObject.activeSelf && _turnManager.ActivePlayerCharacter.TurnsInventoryLocked <= 0)
            {
                _inventoryGameObject.SetActive(true);
            }
        }

        private void DisplayInventoryItems()
        {
            for (int i = 0; i < _currentInventoryItems.Count; i++)
            {
                Destroy(_currentInventoryItems[i]);
            }
            _currentInventoryItems.Clear();

            List<string> consumableItems = _turnManager.ActivePlayerCharacter._PlayerInventory.InventoryItems;

            GameObject visualInventoryItem;

            for (int i = 0; i < consumableItems.Count; i++)
            {
                visualInventoryItem = Instantiate(ItemDatabase.DatabaseItems[consumableItems[i]]);
                visualInventoryItem.transform.SetParent(_inventorySlots[i]);
                visualInventoryItem.transform.localScale = Vector2.one;
                visualInventoryItem.transform.localPosition = Vector2.zero;
                _currentInventoryItems.Add(visualInventoryItem);
            }

            if (_currentInventoryItems.Count > 0)
            {
                for (int i = 0; i < _currentInventoryItems.Count; i++)
                {
                    if(_currentInventoryItems[i].GetComponent<Button>() != null)
                    {
                        _inventoryEventSystem.firstSelectedGameObject = _currentInventoryItems[i];
                        break;
                    }
                }
            }
        }
    }
}