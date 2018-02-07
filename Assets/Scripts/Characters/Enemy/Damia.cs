public class Damia : Enemy {

	public Damia()
    {
        CharacterName = "Damia";
        Level = 6;
        RequiredXP = int.MaxValue;
        XpToGive = 460;
        //Base stats
        Attack = 15;
        Defense = 11;
        Tech = 2;
        Speed = 8;
        MaxHP = 90;
        CurrentHP = MaxHP;
    }
}
