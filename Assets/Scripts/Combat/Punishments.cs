using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punishments : MonoBehaviour {

    private PlayerCharacter _lostCharacter;


    private void OnEnable()
    {
        SetCharacters();
    }

    public void TakeMoney()
    {
        int goldToRemoveAndTake = _lostCharacter.Gold / 2;
        
        BattleStateMachine.CharacterThatLost.Gold -= goldToRemoveAndTake;
        BattleStateMachine.CharacterThatWon.Gold += goldToRemoveAndTake;

        Continue();
    }

    public void TakeItems()
    {
        BattleStateMachine.CharacterThatLost.GetComponent<PlayerCharacter>()._PlayerInventory.RemoveInventoryItem(ItemToExchange.itemToTake);
        BattleStateMachine.CharacterThatWon.GetComponent<PlayerCharacter>()._PlayerInventory.AddInventoryItem(ItemToExchange.itemToTake);

        Continue();
    }

    public void CastInventoryLock()
    {
        _lostCharacter.TurnsInventoryLocked = _lostCharacter.TurnsInventoryLocked + Random.Range(1, 4);

        Continue();
    }

    public void Continue()
    {
        if(BattleStateMachine.OnExit != null)
        {
            BattleStateMachine.OnExit();
        }
    }

    private void SetCharacters()
    {
        GameObject spawnPlayers = GameObject.FindWithTag(InlineStrings.SPAWNPLAYERSTAG);

        PlayerCharacter playerToTransferFrom = BattleStateMachine.CharacterThatLost.GetComponent<PlayerCharacter>();

        foreach (Transform player in spawnPlayers.transform)
        {
            PlayerCharacter pc = player.GetComponent<PlayerCharacter>();

            if (playerToTransferFrom.CharacterName == pc.CharacterName)
            {
                _lostCharacter = pc;
            }
        }
    }
}
