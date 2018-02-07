using UnityEngine;
using Utility;

[System.Serializable]
public class LittleBigBossMan : Enemy {

    protected override void Awake()
    {
        base.Awake();
        DropItem = true;
    }

    public LittleBigBossMan()
    {
        CharacterName = "Miniboss btw";
        Level = 7;
        RequiredXP = int.MaxValue;
        XpToGive = 1750;
        //Base stats
        Attack = 12;
        Defense = 12;
        Tech = 12;
        Speed = 12;
        MaxHP = 150;
        CurrentHP = MaxHP;
    }

    public override void DeathCallback()
    {
        base.DeathCallback();
        DefeatedMiniBosses.Instance.SaveMiniBoss(BattleStateMachine.CharacterThatWon.BoardPosition);
    }
}
