using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Combat;
using Serialization;
using Utility;

public class BattleStateMachine : MonoBehaviour
{
    public delegate void BattleCompletion();
    public delegate void SetBattleWinner(Character characterThatWon);
    public delegate void BattleUnfinished(string key);
    public delegate void TriggerDeathCallback();
    public static BattleCompletion OnBattleComplete;
    public static BattleCompletion OnBattleWon;
    public static TriggerDeathCallback OnSurrender;
    public static SetBattleWinner OnBattleWin;
    public static BattleCompletion OnTweenStop;
    public static BattleUnfinished OnBattleContinuation;

    public delegate void OnExitBattle();
    public static OnExitBattle OnExit;

    public delegate void BattleActions(bool active);
    public static BattleActions OnBattleActions;

    //public static lists
    public static List<Character> BattleCharacters = new List<Character>();
    public static Character AttackingCharacter;
    public static Character DefendingCharacter;

    public static string loadedBattle = "PlayervsEnemy";
    public static bool StopTween;

    public static bool CombatEnd;
    public static bool AttackerTurn;
    public static Character CharacterThatWon;
    public static Character CharacterThatLost;

    public static bool AttackChosen;
    public static bool DefenseChosen;
    private static bool saved = false;
    
    private void Awake()
    {
        OnTweenStop += StopBattleTween;
        OnBattleWin += SetWinner;
        OnExit += ExitBattle;
        OnSurrender += TriggerDeath;
        BattleSceneTransition.TransitionOut();
        saved = false;
        StartCoroutine(LateStart());
        LoadBattleCharacters();
    }

    private void LoadBattleCharacters()
    {
        OngoingBattles ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();

        if (ongoingBattles.ongoingBattles.ContainsKey(loadedBattle))
        {
            List<GameObject> loadedCharacters = ongoingBattles.LoadBattleCharacters(loadedBattle);

            GameObject parent = GameObject.FindWithTag(InlineStrings.BATTLECHARACTERSTAG);

            for (int i = 0; i < loadedCharacters.Count; i++)
            {
                GameObject go = Instantiate(loadedCharacters[i]);
                go.transform.SetParent(parent.transform);
                
                Character characterToAdd = go.GetComponent<Character>();

                Character loadedCharacter = loadedCharacters[i].GetComponent<Character>();
                characterToAdd.CharacterName = loadedCharacter.CharacterName;

                BattleCharacters.Add(characterToAdd);

                if(characterToAdd.CharacterType == InlineStrings.ENEMYTYPE)
                    SaveBattle.Instance.LoadEnemiesInBattle(loadedBattle);
            }

            LoadPlayers();
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        BattleSceneTransition.TransitionCallback += LoadMap;
        //start late start
    }

    public static void SetAttacker(Character attacker)
    {
        AttackingCharacter = attacker;
        AttackingCharacter.IsAttacking = true;
        AttackerTurn = true;

        if(AttackingCharacter.CharacterType == InlineStrings.ENEMYTYPE)
        {
            TweenCallback callback = null;
            callback += () => AICombatBehaviour.OnChooseOffensive();
            Sequence asqn = DOTween.Sequence();
            asqn.AppendInterval(0.05f);
            asqn.OnComplete(callback);
        }
    }

    public static void SetDefender(Character defender)
    {
        DefendingCharacter = defender;
    }

    private void StopBattleTween()
    {
        StopTween = true;
    }

    /// <summary>
    /// End the attacking character's turn and pass it on to the defender
    /// </summary>
    public static void EndAttackersTurn()
    {
        BattleTurnEnds.EndAttackersTurnCallback();
    }

    /// <summary>
    /// End the defending character's turn 
    /// </summary>
    public static void EndDefendersTurn()
    {
        GameObject defender = GameObject.FindWithTag(InlineStrings.ATTACKERTAG);
        GameObject attacker = GameObject.FindWithTag(InlineStrings.DEFENDERTAG);

        Sequence Asqn = DOTween.Sequence();

        //scale down all childs of the defender gameobject, which are the combat actions UI
        Asqn.Append(defender.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack));
        Asqn.Join(attacker.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack));

        Asqn.OnComplete(()=>BattleTurnEnds.EndActionSelectionPhase());
    }

    void SavePlayers()
    {
        GameObject spawnPlayers = GameObject.FindWithTag(InlineStrings.SPAWNPLAYERSTAG);

        spawnPlayers.SetActive(false);
        SaveCharacters.Instance.SavePlayerCharacters();
        spawnPlayers.SetActive(true);
    }

    void LoadPlayers()
    {
        GameObject spawnPlayers = GameObject.FindWithTag(InlineStrings.SPAWNPLAYERSTAG);

        spawnPlayers.SetActive(false);
        SaveCharacters.Instance.LoadPlayerCharacters();
        spawnPlayers.SetActive(true);
    }

    private void ExitBattle()
    {
        if (saved == false)
        {
            saved = true;

            SavePlayers();

            if (BattleCharacters.Count > 1)
            {
                for (int i = 0; i < BattleCharacters.Count; i++)
                {
                    if(BattleCharacters[i].CharacterType == InlineStrings.ENEMYTYPE)
                    {
                        SaveBattle.Instance.SaveEnemiesInBattle(loadedBattle);
                        break;
                    }
                }
            }
            else
            {
                if (CharacterThatLost.CharacterType == InlineStrings.ENEMYTYPE)
                    SaveBattle.Instance.RemoveBattle(loadedBattle);
            }

            BackgroundMusic.instance.FadeMusic(false);
            BattleSceneTransition.TransitionIn();
            GameStateManager.CurrentGameState = GameState.WORLD;
            TurnManager.TurnMutation = 1;
        }
    }

    void LoadMap()
    {
        //if (SceneLoader.OnLoadScene != null)
        //    SceneLoader.OnLoadScene(InlineStrings.MAPSCENE);
        SceneManager.LoadSceneAsync(InlineStrings.MAPSCENE);
    }

    private void TriggerDeath()
    {
        CharacterThatLost.DeathCallback();
    }

    public static bool PVPWon()
    {
        if (CharacterThatLost.CharacterType == InlineStrings.PLAYERTAG && CharacterThatWon.CharacterType == InlineStrings.PLAYERTAG)
        {
            return true;
        }
        else
            return false;
    }

    void SetWinner(Character characterThatLost)
    {
        for (int i = 0; i < BattleCharacters.Count; i++)
        {
            if(BattleCharacters[i] != characterThatLost)
            {
                CharacterThatWon = BattleCharacters[i];
            }
        }
        CharacterThatLost = characterThatLost;

        if (CharacterThatWon.CharacterType == InlineStrings.PLAYERTAG)
        {

        }
            //SoundManager.instance.PlaySound(SoundsDatabase.AudioClips["WinSound"]);

        OngoingBattles ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();
        if(ongoingBattles.GetBattle(loadedBattle) != null)
        {
            ongoingBattles.RemoveBattle(loadedBattle);
        }
    }

    private void OnDestroy()
    {
        OnTweenStop -= StopBattleTween;
        OnBattleWin -= SetWinner;
        StopTween = false;
        BattleCharacters.Clear();
        CombatEnd = false;
        OnExit -= ExitBattle;
        OnSurrender -= TriggerDeath;
    }
}