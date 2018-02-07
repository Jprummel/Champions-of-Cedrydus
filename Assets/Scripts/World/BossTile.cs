using UnityEngine;
using System.Collections;
using Combat;
using System.Collections.Generic;

public class BossTile : BattleTile
{
    [SerializeField] private GameObject _bossPrefab;

    public override void AddOpponent(PlayerCharacter playerThatLandsOnTile)
    {
        if (playerThatLandsOnTile._PlayerInventory.HasEnoughTeleportKeys())
        {
            string battleKey;
            OngoingBattles ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();

            List<GameObject> playersOnThisTile = CheckForOtherPlayers();

            for (int i = 0; i < playersOnThisTile.Count; i++)
            {
                playersOnThisTile[i].GetComponent<PlayerCharacter>().Initiator = false;
                playersOnThisTile[i].GetComponent<SpriteRenderer>().flipX = false;
            }

            playerThatLandsOnTile.Initiator = true;

            if (playersOnThisTile.Count > 1)
            {
                battleKey = playersOnThisTile[0] + " vs " + playersOnThisTile[1];
                playersOnThisTile[1].GetComponent<SpriteRenderer>().flipX = true;

                if (!ongoingBattles.ongoingBattles.ContainsKey(battleKey))
                {
                    ongoingBattles.AddBattle(playersOnThisTile[0], playersOnThisTile[1], battleKey);
                }
            }
            else
            {
                battleKey = playerThatLandsOnTile.CharacterName + "'s battle";
                if (!ongoingBattles.ongoingBattles.ContainsKey(battleKey))
                {
                    GameObject enemy = EnemyDatabase.ReturnEnemy(0, 1);
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.CurrentHP = enemyScript.MaxHP;
                    SaveBattle.Instance.SaveSpecificEnemy(battleKey, enemyScript);

                    ongoingBattles.AddBattle(playerThatLandsOnTile.gameObject, _bossPrefab.gameObject, battleKey);
                }
            }
            BattleStateMachine.loadedBattle = battleKey;
            BattleSceneTransition.TransitionIn();
        }
        else
        {
            base.AddOpponent(playerThatLandsOnTile);
        }
    }
}