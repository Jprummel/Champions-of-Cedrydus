public class ThothBot : Enemy {

    public ThothBot()
    {
        CharacterName = "Thoth Bot";
        Level = 1;
        RequiredXP = int.MaxValue;
        XpToGive = 200;
        //Base stats
        Attack = 5;
        Defense = 2;
        Tech = 2;
        Speed = 2;
        MaxHP = 15;
        CurrentHP = MaxHP;
    }
}