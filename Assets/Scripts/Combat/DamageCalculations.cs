using UnityEngine;

public class DamageCalculations : MonoBehaviour {

    public delegate void DamageEvent();
    public static DamageEvent OnDamage;

    private Character _attackingCharacter;
    private Character _defendingCharacter;

    private Vector2 _damagePopUpSpawn;
    private float _baseLineDamage;
    private float _baseLineDefense;
    private float _dodgeChance;
    private int _damage;
    private bool _canDodge;
    private bool _dodged;
    private void OnEnable()
    {
        OnDamage += CalculateDamage;
    }

    void CalculateDamage()
    {
        _attackingCharacter = BattleStateMachine.AttackingCharacter;
        _defendingCharacter = BattleStateMachine.DefendingCharacter;
        _canDodge = true;

        if (_attackingCharacter.OffensiveOption == Character.OffensiveOptions.ATTACK)
        {
            _baseLineDamage = _attackingCharacter.TotalAttack * 2.8f;
            _baseLineDefense = _defendingCharacter.TotalDefense * 1.1f;
            switch (_defendingCharacter.DefensiveOption)
            {
                case Character.DefensiveOptions.COUNTER:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 2.4f); //41
                    break;
                case Character.DefensiveOptions.DEFEND:
                    _damage = Mathf.RoundToInt(_baseLineDamage - _baseLineDefense); //17
                    break;
                case Character.DefensiveOptions.TECHDEFEND:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 1.3f); //22
                    break;
                case Character.DefensiveOptions.GIVEUP:
                    BattleStateMachine.OnBattleWin(_defendingCharacter);
                    BattleStateMachine.OnBattleWon();
                    BattleStateMachine.OnSurrender();
                    break;
            }
            if (_damage <= 0)
            {
                _damage = 1;
            }
            DealDamage(_defendingCharacter);
        }

        if (_attackingCharacter.OffensiveOption == Character.OffensiveOptions.STRIKE)
        {
            _baseLineDamage = (_attackingCharacter.TotalAttack + _attackingCharacter.TotalTech + _attackingCharacter.TotalSpeed) * 4;
            _baseLineDefense = (_defendingCharacter.TotalDefense + _defendingCharacter.TotalTech + _defendingCharacter.TotalSpeed);
            switch (_defendingCharacter.DefensiveOption)
            {
                case Character.DefensiveOptions.COUNTER:
                    _canDodge = false;
                    _damage = Mathf.RoundToInt((_defendingCharacter.Attack + _defendingCharacter.Tech + _defendingCharacter.Speed) * 4 + (_attackingCharacter.Attack * 2) - (_attackingCharacter.Defense * 2));
                    if (_damage <= 0)
                    {
                        _damage = 1;
                    }
                    DealDamage(_attackingCharacter);
                    break;
                case Character.DefensiveOptions.DEFEND:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 1.6f); //144
                    if (_damage <= 0)
                    {
                        _damage = 1;
                    }
                    DealDamage(_defendingCharacter);
                    break;
                case Character.DefensiveOptions.TECHDEFEND:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 1.6f); //144
                    if (_damage <= 0)
                    {
                        _damage = 1;
                    }
                    DealDamage(_defendingCharacter);
                    break;
                case Character.DefensiveOptions.GIVEUP:
                    BattleStateMachine.OnBattleWin(_defendingCharacter);
                    BattleStateMachine.OnBattleWon();
                    BattleStateMachine.OnSurrender();
                    break;
            }
        }

        if (_attackingCharacter.OffensiveOption == Character.OffensiveOptions.TECHATTACK)
        {
            _baseLineDamage = ((_attackingCharacter.TotalAttack * 0.6f) + _attackingCharacter.TotalTech) * 2.2f;
            _baseLineDefense = ((_defendingCharacter.TotalDefense * 0.6f) + (_defendingCharacter.TotalTech * 0.6f));
            switch (_defendingCharacter.DefensiveOption)
            {
                case Character.DefensiveOptions.COUNTER:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 2.4f);
                    break;
                case Character.DefensiveOptions.DEFEND:
                    _damage = Mathf.RoundToInt((_baseLineDamage - _baseLineDefense) * 1.5f);
                    break;
                case Character.DefensiveOptions.TECHDEFEND:
                    _damage = Mathf.RoundToInt(_baseLineDamage - _baseLineDefense);
                    break;
                case Character.DefensiveOptions.GIVEUP:
                    BattleStateMachine.OnBattleWin(_defendingCharacter);
                    BattleStateMachine.OnBattleWon();
                    BattleStateMachine.OnSurrender();
                    break;
            }
            if (_damage <= 0)
            {
                _damage = 1;
            }
            DealDamage(_defendingCharacter);
        }
    }

    void DealDamage(Character defendingCharacter)
    {
        if (_canDodge)
        {
            Dodge();
        }
        if (!_dodged)
        {
            _damagePopUpSpawn = new Vector2(defendingCharacter.transform.position.x, defendingCharacter.transform.position.y + 2);
            SpawnDamagePopUp.OnDamage(_damage.ToString(), _damagePopUpSpawn);
            defendingCharacter.TakeDamage(_damage);
            if (defendingCharacter.CurrentHP <= 0)
            {
                defendingCharacter.CurrentHP = 0; //prevents ui from showing the character has negative HP
                                                  //Death animation
            }
            HealthUI.OnHealthEvent();
        }
    }

    void Dodge()
    {
        _dodgeChance = Mathf.Clamp(_defendingCharacter.Speed - _attackingCharacter.Speed, 0, 50);
        float dodgeChanceChecker = Random.Range(0, 101);

        if (dodgeChanceChecker <= _dodgeChance && _dodgeChance > 0)
        {
            SpawnDamagePopUp.OnDamage("Dodged", new Vector2(_defendingCharacter.transform.position.x, _defendingCharacter.transform.position.y + 2));
            _dodged = true;
        }
        else
        {
            _dodged = false;
        }
    }

    private void OnDisable()
    {
        OnDamage -= CalculateDamage;
    }
}