using Inventory;
using Utility;

namespace Data
{
    [System.Serializable]
    public class Stats
    {
        public string CharacterName;
        public string ClassName;
        public bool IsInBattle;
        public int Attack; //Melee damage modifier
        public int Defense; //Melee damage reduction modifier
        public int Tech; //Magic damage modifier
        public int Speed; //Dodge chance modifier
        public int BonusAttack; //Bonus Attack
        public int BonusDefense; //Bonus Defense
        public int BonusTech; //Bonus Tech
        public int BonusSpeed; //Bonus Speed
        public float CurrentHP;
        public float MaxHP;
        public float BonusMaxHP; //Bonus Max HP

        public int Level;
        public int Gold;
        public float CurrentXP;
        public float RequiredXP;
        public float TotalEarnedXP;

        public int TurnsBuffed;
        public int TurnsDebuffed;
        public int TurnsMovementImpaired;
        public int TurnsInventoryLocked;
        public int TurnsDead;
        public bool KnockedOut;

        public int2 PositionInMap;
        public int2 BoardPosition;

        public PlayerInventory Inventory;
    }
}