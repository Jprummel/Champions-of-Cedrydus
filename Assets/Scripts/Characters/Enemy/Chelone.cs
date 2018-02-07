public class Chelone : Enemy {

	public Chelone()
    {
        CharacterName = "Chelone";
        Level = 7;
        RequiredXP = int.MaxValue;
        XpToGive = 540;
        //Base stats
        Attack = 17;
        Defense = 11;
        Tech = 6;
        Speed = 10;
        MaxHP = 100;
        CurrentHP = MaxHP;
    }
}