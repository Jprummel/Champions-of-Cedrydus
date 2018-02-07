[System.Serializable]
public class BigBossMan : Enemy {

    public BigBossMan()
    {
        CharacterName = "The Boss";
        Level = 10;
        RequiredXP = int.MaxValue;
        XpToGive = 0;
        //Base stats
        Attack = 15;
        Defense = 15;
        Tech = 15;
        Speed = 15;
        MaxHP = 200;
        CurrentHP = MaxHP;
    }
}