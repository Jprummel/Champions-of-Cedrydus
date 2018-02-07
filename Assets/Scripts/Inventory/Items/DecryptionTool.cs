using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using PlayerCharacters;

public class DecryptionTool : UsableItem
{
    public delegate void DecryptionToolAction();
    public static event DecryptionToolAction OnDecryptionTool;

    public override void UseItem()
    {
        CloseInventory();
        ExecuteUsableEffect();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        PlayerMovement currentPlayerMovement = _turnManager.ActivePlayer;
        currentPlayerMovement.NumberOfMoves = 6;
        currentPlayerMovement.CanMoveAnyPlace = true;
        currentPlayerMovement.PlayerHasMoves = true;

        if (OnDecryptionTool != null)
            OnDecryptionTool();

        RemoveItemFromInventory();
        //Lets player walk between 0-6 spaces (players choice)
    }
}
