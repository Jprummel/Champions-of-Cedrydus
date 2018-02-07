using UnityEngine;
using Utility;

[System.Serializable]
public class MiniBoss : Enemy {

    protected override void Awake()
    {
        base.Awake();
        DropItem = true;
    }

    public MiniBoss()
    {
        CharacterName = "Keeper of the key";
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
