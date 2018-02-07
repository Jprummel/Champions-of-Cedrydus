using UnityEngine;
using Utility;

public class InputManager : MonoBehaviour
{
    public delegate void MovementInputAction(int2 direction);
    public static event MovementInputAction OnMovementInput;

    public delegate void AButtonAction();
    public static event AButtonAction OnAButton;

    public delegate void BattleAButtonAction();
    public static event BattleAButtonAction OnBattleAButton;

    public delegate void BButtonAction();
    public static event BButtonAction OnBButton;

    public delegate void XButtonAction();
    public static event XButtonAction OnXButton;

    public delegate void LBButtonAction();
    public static event LBButtonAction OnLBButton;

    public delegate void RBButtonAction();
    public static event RBButtonAction OnRBButton;

    void Update ()
    {
		if(GameStateManager.CurrentGameState == GameState.WORLD)
        {
            WorldInputs();
        }else if(GameStateManager.CurrentGameState == GameState.COMBAT)
        {
            CombatInputs();
        }
	}

    void WorldInputs()
    {
        //D-Pad
        if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_HORIZONTAL) > 0)
        {
            //Right dpad
            if (OnMovementInput != null)
                OnMovementInput(new int2(1, 0));

        }
        else if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_HORIZONTAL) < 0)
        {
            //Left dpad
            if (OnMovementInput != null)
                OnMovementInput(new int2(-1, 0));
        }
        else if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_VERTICAL) > 0)
        {
            //Up dpad
            if (OnMovementInput != null)
                OnMovementInput(new int2(0, -1));
        }
        else if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_VERTICAL) < 0)
        {
            //Down dpad
            if (OnMovementInput != null)
                OnMovementInput(new int2(0, 1));
        }

        //Face Buttons
        if (Input.GetButtonDown(InputStrings.CONTROLLER_A))
        {
            if (OnAButton != null)
                OnAButton();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_B))
        {
            if (OnBButton != null)
                OnBButton();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_X))
        {
            if (OnXButton != null)
                OnXButton();
        }
        
        //Bumpers and Triggers
        if (Input.GetButtonDown(InputStrings.CONTROLLER_LB))
        {
            if (OnLBButton != null)
                OnLBButton();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_RB))
        {
            if (OnRBButton != null)
                OnRBButton();
        }
    }

    void CombatInputs()
    {
        //D-Pad
        if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_HORIZONTAL) > 0)
        {
            //Right dpad
            //battle turn cards stuff
            if (BattleTurnCards.onSwitchCards != null)
            {
                BattleTurnCards.onSwitchCards();
            }
        }
        else if (Input.GetAxis(InputStrings.CONTROLLER_DPAD_HORIZONTAL) < 0)
        {
            //Left dpad
            //battle turn cards stuff
            if (BattleTurnCards.onSwitchCards != null)
            {
                BattleTurnCards.onSwitchCards();
            }
        }
       
        if (Input.GetButtonDown(InputStrings.CONTROLLER_A))
        {
            //battle turn cards stuff
            if (BattleTurnCards.onConfirmCards != null)
            {
                BattleTurnCards.onConfirmCards();
            }
            if (WaveText.IsTweening)
            {
                WaveText.KillSequence();
            }

            if (OnBattleAButton != null)
                OnBattleAButton();
        }
    }
}