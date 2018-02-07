[System.Serializable]
public class Technomancer : PlayerCharacter {

    //Ranged Assassin Archetype class
    public Technomancer()
    {
        ClassName = ClassNames.TECHNOMANCER;
        Level = 1;
        RequiredXP = 400;
        //Base stats
        Attack = 4;
        Defense = 2;
        Tech = 5;
        Speed = 3;
        MaxHP = 50;
        CurrentHP = MaxHP;

        AttackToGain = 0;
        DefenseToGain = 0;
        TechToGain = 2;
        SpeedToGain = 1;
        MaxHPToGain = 10;
    }
}