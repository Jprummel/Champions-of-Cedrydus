public class Damia : Enemy {

	public Damia()
    {
        CharacterName = "Damia";
        Level = 4;
        RequiredXP = int.MaxValue;
        XpToGive = 400;
        //Base stats
        Attack = 9;
        Defense = 9;
        Tech = 1;
        Speed = 7;
        MaxHP = 60;
        CurrentHP = MaxHP;
    }
}
