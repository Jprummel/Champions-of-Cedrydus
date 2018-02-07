using UnityEngine;
using DG.Tweening;
using Combat;
using Serialization;
using PlayerCharacters;
using Utility;

[System.Serializable]
public class Character : MonoBehaviour
{
    public delegate void UsedRecoveryAction(string playerName);
    public static event UsedRecoveryAction OnUsedRecovery;

    public enum OffensiveOptions
    {
        ATTACK,
        TECHATTACK,
        STRIKE,
    }

    public enum DefensiveOptions
    {
        DEFEND,
        TECHDEFEND,
        COUNTER,
        GIVEUP
    }

    public OffensiveOptions OffensiveOption;
    public DefensiveOptions DefensiveOption;

    [SerializeField] protected Sprite _sprite;
    [Header("General Stats")]
    //Stats
    [SerializeField]protected bool _initiator;
    [SerializeField]protected int _goldToGive;

    public int2 BoardPosition { get; set; }
    public Sprite Sprite {get { return _sprite; }}
    public bool IsAttacking { get; set; }
    public bool Initiator
    {
        get { return _initiator; }
        set { _initiator = value; }
    }

    //Stats getters and setters

    public string CharacterName { get; set; }
    public int Level            { get; set; }
    public float CurrentXP      { get; set; }
    public float RequiredXP     { get; set; }
    public float TotalXPEarned  { get; set; }
    public int XpToGive         { get; set; }
    public int Gold             { get; set; }
    public int GoldToGive       { get { return _goldToGive; } }
    public bool DropItem        { get; set; }
    public int StatPoints       { get; set; }
    
    public int Attack       { get; set; }
    public int Defense      { get; set; }
    public int Tech         { get; set; }
    public int Speed        { get; set; }
    public float CurrentHP  { get; set; }
    public float MaxHP      { get; set; }

    public int BonusAttack  { get; set; }
    public int BonusDefense { get; set; }
    public int BonusTech    { get; set; }
    public int BonusSpeed   { get; set; }
    public float BonusMaxHP { get; set; }

    public int TotalAttack  { get; set; }
    public int TotalDefense { get; set; }
    public int TotalTech    { get; set; }
    public int TotalSpeed   { get; set; }
    public float TotalMaxHP { get; set; }

    public int AttackToGain     { get; set; }
    public int DefenseToGain    { get; set; }
    public int TechToGain       { get; set; }
    public int SpeedToGain      { get; set; }
    public int MaxHPToGain      { get; set; }

    //(De)Buff checkers
    public int TurnsBuffed { get; set; }
    public int TurnsDebuffed { get; set; }
    public int TurnsMovementImpaired { get; set; }
    public int TurnsInventoryLocked { get; set; }
    public int TurnsDead { get; set; }
    public bool KnockedOut;

    public string CharacterType { get; set;}

    void Start()
    {
        CalculateTotalStats();
    }

    public virtual void GainClassStats()
    {
        Attack += AttackToGain;
        Defense += DefenseToGain;
        Tech += TechToGain;
        Speed += SpeedToGain;
        MaxHP += MaxHPToGain;
    }

    public void CalculateTotalStats()
    {
        //Calculates base stat + bonus stat
        TotalAttack     = Attack + BonusAttack;
        TotalDefense    = Defense + BonusDefense;
        TotalTech       = Tech + BonusTech;
        TotalSpeed      = Speed + BonusSpeed;
        TotalMaxHP      = MaxHP + BonusMaxHP;
    }

    public void RemoveBonusStats()
    {
        //Removes buffs / debuffs
        BonusAttack = 0;
        BonusDefense = 0;
        BonusTech = 0;
        BonusSpeed = 0;
        BonusMaxHP = 0;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if(CurrentHP <= 0)
        {
            if(CharacterType == InlineStrings.PLAYERTAG && ((PlayerCharacter)this)._PlayerInventory.IsItemInInventory("Recovery"))
            {
                ((PlayerCharacter)this)._PlayerInventory.RemoveInventoryItem("Recovery");
                CurrentHP = MaxHP;
                if (OnUsedRecovery != null)
                    OnUsedRecovery(CharacterName);
            }
            else if(CharacterName == InlineStrings.BOSSTAG)
            {
                //Debug.Log("Wonnered");
                //win
                if(BattleStateMachine.OnTweenStop != null)
                    BattleStateMachine.OnTweenStop();

                WinnerInfo.WinnerName = BattleStateMachine.AttackingCharacter.CharacterName;
                WinnerInfo.WinnerSprite = BattleStateMachine.AttackingCharacter.GetComponent<SpriteRenderer>().sprite;
                SceneLoader.OnLoadScene(InlineStrings.WINSCENE);
            }
            else
            {
                BattleStateMachine.OnTweenStop();
                transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(DeathCallback);
            }
        }
    }

    public virtual void DeathCallback()
    {
        if (GameStateManager.CurrentGameState == GameState.COMBAT)
        {
            BattleStateMachine.OnBattleWin(this);
            BattleStateMachine.OnBattleWon();
            BattleStateMachine.BattleCharacters.Remove(this);
            CalculateXpDifference(BattleStateMachine.CharacterThatWon);
            PostCombatRewards.XPToGive = XpToGive;
            PostCombatRewards.GoldToGive = GoldToGive;
            PostCombatRewards.DropItem = DropItem;
            
            if(CharacterType == InlineStrings.PLAYERTAG)
            {
                //Remove all buffs/debuffs and movement impairing effects on death
                TurnsBuffed = 0;
                TurnsDebuffed = 0;
                TurnsMovementImpaired = 0;

                TurnsDead = Random.Range(1, 4); //Player is dead for random amount of turns between 1 (min) and 3 (max)   
                KnockedOut = true;

                GameObject spawnPlayers = GameObject.FindWithTag(InlineStrings.SPAWNPLAYERSTAG);

                foreach (Transform player in spawnPlayers.transform)
                {
                    PlayerCharacter pc = player.GetComponent<PlayerCharacter>();

                    if(pc.CharacterName == CharacterName)
                    {
                        OngoingBattles ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();

                        ResetCharacterPosition.TargetName = pc.CharacterName;
                        ongoingBattles.RemoveBattlesFromTarget(pc);
                    }
                }
            }
        }
    }

    public void ResetSpriteFlip()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = false;
    }

    public void Revive()
    {
        if (KnockedOut)
        {
            KnockedOut = false;
            CurrentHP = MaxHP;
            SaveCharacters.Instance.SavePlayerCharacters();
        }
    }

    public virtual void CalculateXpDifference(Character otherCharacter = null)
    {
        XpToGive = XpToGive;
    }
}