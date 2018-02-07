using UnityEngine;
using System.Collections;
using Combat;
using System.Collections.Generic;
using PlayerCharacters;
using Utility;
using Serialization;

public class MinibossTile : BattleTile {

    [SerializeField]private GameObject _miniBossPrefab;

    public override void AddOpponent(PlayerCharacter playerThatLandsOnTile)
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
                int2 boardPos = playerThatLandsOnTile.GetComponent<PlayerMovement>().PositionOnMap;

                if (!DefeatedMiniBosses.Instance.DefeatedBoss(boardPos))
                {
                    Enemy enemyScript = _miniBossPrefab.GetComponent<Enemy>();
                    enemyScript.CurrentHP = enemyScript.MaxHP;
                    SaveBattle.Instance.SaveSpecificEnemy(battleKey, enemyScript);

                    playerThatLandsOnTile.BoardPosition = BoardPosition;
                    SaveCharacters.Instance.SavePlayerCharacters();
                    ongoingBattles.AddBattle(playerThatLandsOnTile.gameObject, _miniBossPrefab.gameObject, battleKey);
                }
                else
                {
                    GameObject enemy = EnemyDatabase.ReturnEnemy(0, 1);
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.CurrentHP = enemyScript.MaxHP;
                    SaveBattle.Instance.SaveSpecificEnemy(battleKey, enemyScript);

                    ongoingBattles.AddBattle(playerThatLandsOnTile.gameObject, enemy, battleKey);
                }
            }
        }
        BattleStateMachine.loadedBattle = battleKey;
        BattleSceneTransition.TransitionIn();
    }
}