public class Naga : Enemy {

	public Naga()
    {
        CharacterName = "Naga";
        Level = 5;
        RequiredXP = int.MaxValue;
        XpToGive = 450;
        //Base stats
        Attack = 12;
        Defense = 8;
        Tech = 4;
        Speed = 8;
        MaxHP = 80;
        CurrentHP = MaxHP;
    }
}