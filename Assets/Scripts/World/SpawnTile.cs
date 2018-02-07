using Inventory;
using UnityEngine;

public class SpawnTile : BoardTile
{
    public override void PlayerLandsOnTile(PlayerCharacter player)
    {
        base.PlayerLandsOnTile(player);
        _healingPopUp.EnablePopUp();
    }
}