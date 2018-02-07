using UnityEngine;

public class BattleActions : MonoBehaviour
{
    public void OffensiveAction(int actionIndex)
    {
        switch (actionIndex)
        {
            case 0:
                BattleStateMachine.AttackingCharacter.OffensiveOption = Character.OffensiveOptions.ATTACK; //Attacker chooses Attack
                break;
            case 1:
                BattleStateMachine.AttackingCharacter.OffensiveOption = Character.OffensiveOptions.STRIKE;  //Attacker chooses Strike
                break;
            case 2:
                BattleStateMachine.AttackingCharacter.OffensiveOption = Character.OffensiveOptions.TECHATTACK; //Attacker chooses Tech Attack
                break;
        }
    }

    public void DefensiveAction(int actionIndex)
    {
        switch (actionIndex)
        {
            case 0:
                BattleStateMachine.DefendingCharacter.DefensiveOption = Character.DefensiveOptions.DEFEND; //Defender chooses Defend
                break;
            case 1:
                BattleStateMachine.DefendingCharacter.DefensiveOption = Character.DefensiveOptions.COUNTER; //Defender chooses Counter
                break;
            case 2:
                BattleStateMachine.DefendingCharacter.DefensiveOption = Character.DefensiveOptions.TECHDEFEND; //Defender chooses Tech Defend
                break;
            case 3:
                BattleStateMachine.DefendingCharacter.DefensiveOption = Character.DefensiveOptions.GIVEUP; //Defender chooses Surrender
                break;
        }
    }
}