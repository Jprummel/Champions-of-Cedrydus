[System.Serializable]
public class WuBot : Enemy {

    public WuBot()
    {
        CharacterName = "Wu Bot";
        Level = 3;
        RequiredXP = int.MaxValue;
        XpToGive = 300;
        //Base stats
        Attack = 7;
        Defense = 5;
        Tech = 3;
        Speed = 4;
        MaxHP = 35;
        CurrentHP = MaxHP;
    }
}