using Combat;
using Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTile : BoardTile
{
    private GameObject _enemy;

    public override void OnEnable()
    {
        base.OnEnable();
        if(BattleStateMachine.OnExit == null)
        {
            BattleStateMachine.OnExit += TileEventDone;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        BattleStateMachine.OnExit -= TileEventDone;
    }

    public override void PlayerLandsOnTile(PlayerCharacter player)
    {
        base.PlayerLandsOnTile(player);
        SaveCharacters.Instance.SavePlayerCharacters();
        BattleSceneTransition.TransitionCallback += LoadScene;
        AddOpponent(player);
        GameStateManager.TimeGameState = GameState.PAUSED;
        GameStateManager.CurrentGameState = GameState.COMBAT;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(InlineStrings.BATTLESCENE);
        SetBattleBackground.BattleBiome = BattleBiome;
    }

    void NewEnemy()
    {
        switch (BattleBiome)
        {
            case Biome.SNOW:
                _enemy = EnemyDatabase.ReturnEnemy(0, 2);
                break;
            case Biome.FOREST:
                _enemy = EnemyDatabase.ReturnEnemy(2, 4);
                break;
            case Biome.COLOSEUM:
                _enemy = EnemyDatabase.ReturnEnemy(2, 4);
                break;
            case Biome.RUINS:
                _enemy = EnemyDatabase.ReturnEnemy(4, 6);
                break;
            case Biome.DESERT:
                _enemy = EnemyDatabase.ReturnEnemy(6, 8);
                break;
        }
    }

    public virtual void AddOpponent(PlayerCharacter playerThatLandsOnTile)
    {
        string battleKey;
        OngoingBattles ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();

        Character characterToFight;

        List<GameObject> playersOnThisTile = CheckForOtherPlayers();

        for (int i = 0; i < playersOnThisTile.Count; i++)
        {
            playersOnThisTile[i].GetComponent<PlayerCharacter>().Initiator = false;
            playersOnThisTile[i].GetComponent<SpriteRenderer>().flipX = false;
        }

        playerThatLandsOnTile.Initiator = true;

        if (playersOnThisTile.Count > 1)
        {
            GameObject playerToFight = playersOnThisTile[1];

            for (int i = 0; i < playersOnThisTile.Count; i++)
            {
                if (playersOnThisTile[i].GetComponent<Character>().CharacterName != playerThatLandsOnTile.CharacterName)
                {
                    playerToFight = playersOnThisTile[i];
                    break;
                }
            }

            battleKey = playerThatLandsOnTile.gameObject + " vs " + playerToFight;
            playerToFight.GetComponent<SpriteRenderer>().flipX = true;

            if (!ongoingBattles.ongoingBattles.ContainsKey(battleKey))
            {
                characterToFight = playerToFight.GetComponent<Character>();
                VersusUI.OnSetVersusStats(playerThatLandsOnTile, characterToFight);
                ongoingBattles.AddBattle(playerThatLandsOnTile.gameObject, playerToFight, battleKey);
            }
        }
        else
        {
            battleKey = playerThatLandsOnTile.CharacterName + "'s battle";
            if (!ongoingBattles.ongoingBattles.ContainsKey(battleKey))
            {
                NewEnemy();
                Enemy enemyScript = _enemy.GetComponent<Enemy>();
                enemyScript.CurrentHP = enemyScript.MaxHP;
                SaveBattle.Instance.SaveSpecificEnemy(battleKey,enemyScript);

                characterToFight = enemyScript;

                VersusUI.OnSetVersusStats(playerThatLandsOnTile, characterToFight);
                ongoingBattles.AddBattle(playerThatLandsOnTile.gameObject, _enemy, battleKey);
            }
            else if(ongoingBattles.ongoingBattles.ContainsKey(battleKey))
            {
                Battle battle = ongoingBattles.GetBattle(battleKey);
                float enemyHP = 0f;

                if(playerThatLandsOnTile.CharacterName == battle.Character1.GetComponent<Character>().CharacterName)
                {
                    characterToFight = battle.Character2.GetComponent<Character>();
                }
                else
                {
                    characterToFight = battle.Character1.GetComponent<Character>();
                }
                if (characterToFight.GetComponent<Enemy>() != null)
                {
                    enemyHP = OngoingBattles.EnemiesInBattleHP[battleKey];
                    Debug.Log(enemyHP);
                }
                VersusUI.OnSetVersusStats(playerThatLandsOnTile, characterToFight, enemyHP);
            }
        }
        BattleStateMachine.loadedBattle = battleKey;
        BackgroundMusic.instance.FadeMusic(false);
    }

    public virtual List<GameObject> CheckForOtherPlayers()
    {
        List<GameObject> playersOnThisTile = new List<GameObject>();

        playersOnThisTile.Add(_turnManager.ActivePlayerCharacter.gameObject);

        foreach (var player in _turnManager.Players)
        {
            if(_turnManager.ActivePlayer != player)
            {
                if(_turnManager.ActivePlayer.PositionOnMap == player.PositionOnMap)
                {
                    playersOnThisTile.Add(player.gameObject);
                }
            }
        }
        return playersOnThisTile;
    }

    public virtual bool NewPlayer(PlayerCharacter playerThatLandsOnTile)
    {
        for (int i = 0; i < PlayersOnThisTile.Count - 1; i++)
        {
            if (playerThatLandsOnTile == PlayersOnThisTile[i])
            {
                return false;
            }
        }
        return true;
    }
}