[System.Serializable]
public class Engineer : PlayerCharacter {

    //Bruiser Archetype class
    public Engineer()
    {
        ClassName = ClassNames.ENGINEER;
        Level = 1;
        RequiredXP = 400;
        //Base stats
        Attack = 4;
        Defense = 4;
        Tech = 2;
        Speed = 2;
        MaxHP = 50;
        CurrentHP = MaxHP;

        AttackToGain = 0;
        DefenseToGain = 1;
        TechToGain = 1;
        SpeedToGain = 0;
        MaxHPToGain = 20;
    }
}