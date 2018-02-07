public class Enemy : Character {

	protected virtual void Awake () {
        CurrentHP = MaxHP;
        CharacterType = CharacterTypes.ENEMY;
	}
}