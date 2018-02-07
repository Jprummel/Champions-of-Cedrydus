public class Auril : Enemy {

	public Auril()
    {
        CharacterName = "Auril";
        Level = 2;
        RequiredXP = int.MaxValue;
        XpToGive = 240;
        //Base stats
        Attack = 5;
        Defense = 4;
        Tech = 1;
        Speed = 3;
        MaxHP = 30;
        CurrentHP = MaxHP;
    }
}
