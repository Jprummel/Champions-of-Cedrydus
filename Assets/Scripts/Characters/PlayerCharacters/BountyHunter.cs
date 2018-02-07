[System.Serializable]
public class BountyHunter : PlayerCharacter {

    //Assassin Archetype class
    public BountyHunter()
    {
        ClassName = ClassNames.BOUNTYHUNTER;
        Level = 1;
        RequiredXP = 400;
        //Base stats
        Attack = 5;
        Defense = 3;
        Tech = 1;
        Speed = 4;
        MaxHP = 50;
        CurrentHP = MaxHP;

        AttackToGain = 1;
        DefenseToGain = 0;
        TechToGain = 0;
        SpeedToGain = 2;
        MaxHPToGain = 10;
    }
}