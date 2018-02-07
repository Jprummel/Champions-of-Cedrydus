using UnityEngine;

public class TownTile : BoardTile
{
    public override void PlayerLandsOnTile(PlayerCharacter player)
    {
        base.PlayerLandsOnTile(player);
        if (player.TurnsDead <= 0)
        {
            _healingPopUp.EnablePopUp();
        }  
    }
}