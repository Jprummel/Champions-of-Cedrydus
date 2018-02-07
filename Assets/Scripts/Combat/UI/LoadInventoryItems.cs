using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadInventoryItems : MonoBehaviour {

    [SerializeField]private List<Button> _items = new List<Button>();

    private void OnEnable()
    {
        LoadDeadPlayersInventory(BattleStateMachine.CharacterThatLost);
    }

    private void LoadDeadPlayersInventory(Character deadPlayer)
    {
       PlayerCharacter deadPC = deadPlayer.GetComponent<PlayerCharacter>();

        for (int i = 0; i < deadPC._PlayerInventory.InventoryItems.Count; i++)
        {
            _items[i].interactable = true;

            _items[i].GetComponentInChildren<Text>().text = deadPC._PlayerInventory.InventoryItems[i].ToString();
        }
    }

    private void OnDisable()
    {
        _items.Clear();
    }
}
