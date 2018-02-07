using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnEnds : MonoBehaviour {

	public static void EndAttackersTurnCallback()
    {
        //pass on your turn to the defender
        BattleStateMachine.AttackerTurn = !BattleStateMachine.AttackerTurn;

        //choose defensive option for the enemy if it is not a player
        if (BattleStateMachine.DefendingCharacter.CharacterType == InlineStrings.ENEMYTYPE)
        {
            KeyButton.AllowInput = false;
            AICombatBehaviour.OnChooseDefensive();
        }
        else
        {
            ShowCombatActions.OnActionConfirm("Attacker");
        }
    }


    /// <summary>
    /// Start the attacking character's animations.
    /// </summary>
    public static void EndActionSelectionPhase()
    {
        ShowCombatActions.OnActionConfirm("Reset");
        BattleStateMachine.OnBattleActions(false);
        BattleAnimations.AttackingCameraMovement();
    }
}
