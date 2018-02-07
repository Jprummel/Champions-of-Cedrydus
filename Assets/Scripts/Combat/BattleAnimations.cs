using UnityEngine;
using DG.Tweening;

public class BattleAnimations : MonoBehaviour {

    private static Camera battleCamera;
    [SerializeField] private Camera _battleCamera;

    private void Start()
    {
        _battleCamera = Camera.main;
        battleCamera = _battleCamera;
    }

    public static void AttackingCameraMovement()
    {
        //Camera animation
        Sequence Csqn = DOTween.Sequence();

        TweenCallback callback = null;
        //callback += ShowActionText.OnHideAction;
        callback += AttackingCharacterMovement;

        Csqn.Append(battleCamera.DOOrthoSize(3, 0.5f));
        Csqn.Join(battleCamera.transform.DOMoveX(Mathf.Clamp(BattleStateMachine.AttackingCharacter.transform.position.x, -4.25f, 4.25f), 0.5f));

        //Csqn.AppendCallback(() => ShowActionText.TextAppear(BattleStateMachine.AttackingCharacter.OffensiveOption.ToString(), BattleStateMachine.AttackingCharacter.transform));
        Csqn.AppendCallback(() => ShowActionText.OnShowAction(BattleStateMachine.AttackingCharacter.OffensiveOption.ToString(), BattleStateMachine.AttackingCharacter.transform));
        Csqn.AppendInterval(0.4f);
        Csqn.Append(battleCamera.DOOrthoSize(5, 0.5f));
        Csqn.Join(battleCamera.transform.DOMoveX(0, 0.5f));

        Csqn.Join(battleCamera.transform.DOMoveX(0, 0.5f));
        Csqn.AppendCallback(() => ShowActionText.OnHideAction());
        Csqn.OnComplete(callback);
        
    }

    public static void AttackingCharacterMovement()
    {
        Sequence sqn = DOTween.Sequence();

        //Put the character's current position in a variable so we can move it back later.
        float startPos = BattleStateMachine.AttackingCharacter.transform.position.x;
        //Move the character to the defender, - an offset.

        if (BattleStateMachine.AttackingCharacter.transform.position.x < 0)
        {
            sqn.Append(BattleStateMachine.AttackingCharacter.transform.DOMoveX(BattleStateMachine.DefendingCharacter.transform.position.x - 4, 1f));
        }
        else
        {
            sqn.Append(BattleStateMachine.AttackingCharacter.transform.DOMoveX(BattleStateMachine.DefendingCharacter.transform.position.x + 4, 1f));
        }

        sqn.Append(battleCamera.DOOrthoSize(3, 0.5f));
        sqn.Join(battleCamera.transform.DOMoveX(Mathf.Clamp(BattleStateMachine.DefendingCharacter.transform.position.x, -4.25f, 4.25f), 0.5f));
        //sqn.AppendCallback(() => ShowActionText.TextAppear(BattleStateMachine.DefendingCharacter.DefensiveOption.ToString(), BattleStateMachine.DefendingCharacter.transform));
        sqn.AppendCallback(() => ShowActionText.OnShowAction(BattleStateMachine.DefendingCharacter.DefensiveOption.ToString(), BattleStateMachine.DefendingCharacter.transform));
        sqn.AppendInterval(0.75f);
        sqn.Append(battleCamera.DOOrthoSize(5, 0.5f));
        sqn.Join(battleCamera.transform.DOMoveX(0, 0.5f));
        //sqn.AppendCallback(ShowActionText.HideText);
        sqn.AppendCallback(() => ShowActionText.OnHideAction());
        //Deal damage when the animations are done and then move the character back to his starting position
        sqn.Append(BattleStateMachine.AttackingCharacter.transform.DOShakeScale(0.5f).OnComplete(() => BetweenTurnCallback(sqn, startPos)));
    }

    /// <summary>
    /// Deals damage and then checks if the combat is over
    /// </summary>
    /// <param name="sqn">Continue in the moving sequence so that we can stop the whole sequence when a player dies.</param>
    /// <param name="xPos">Starting position which the character moves back to.</param>
    private static void BetweenTurnCallback(Sequence sqn, float xPos)
    {
        DamageCalculations.OnDamage();

        //make the player move back to his original position and check if the combat is over
        if (!BattleStateMachine.StopTween)
        {
            TweenCallback callback = null;
            if(ShowCombatActions.OnShowCombatUI != null)
            {
                callback += () => ShowCombatActions.OnShowCombatUI(secondHalf: true);
            }

            if(BattleStateMachine.OnBattleActions != null)
            {
                callback += () => CallBack();
            }
            sqn.Append(BattleStateMachine.AttackingCharacter.transform.DOMoveX(xPos, 1f).OnComplete(callback));
        }
    }

    private static void CallBack()
    {
        if (BattleStateMachine.OnBattleActions != null)
        {
            BattleStateMachine.OnBattleActions(true);
        }
    }
}