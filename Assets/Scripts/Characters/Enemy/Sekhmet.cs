public class Sekhmet : Enemy {

	public Sekhmet()
    {
        CharacterName = "Sekhmet";
        Level = 8;
        RequiredXP = int.MaxValue;
        XpToGive = 600;
        //Base stats
        Attack = 17;
        Defense = 13;
        Tech = 5;
        Speed = 9;
        MaxHP = 120;
        CurrentHP = MaxHP;
    }
}
