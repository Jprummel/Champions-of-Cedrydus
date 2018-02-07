[System.Serializable]
public class Juggernaut : PlayerCharacter {

    //Tank Archetype Class
	public Juggernaut()
    {
        ClassName = ClassNames.JUGGERNAUT;
        Level = 1;
        RequiredXP = 400;
        //Base stats
        Attack = 5;
        Defense = 4;
        Tech = 1;
        Speed = 2;
        MaxHP = 50;
        CurrentHP = MaxHP;

        AttackToGain = 2;
        DefenseToGain = 1;
        TechToGain = 0;
        SpeedToGain = 0;
        MaxHPToGain = 10;
        
    }
}