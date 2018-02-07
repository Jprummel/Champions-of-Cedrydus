using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class KeyButton : MonoBehaviour
{
    public static bool AllowInput = false;

    [SerializeField]private KeyCode _key;
    //A = JoystickB0
    //B = JoystickB1
    //X = JoystickB2
    //Y = JoystickB3

    [SerializeField]private GameObject _parent;
    private static bool _takesInput = true;

    private Button _button;

    void Awake()
    {
        AllowInput = true;
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            if (_parent.name == InlineStrings.ATTACKERTAG)
            {
                if (BattleStateMachine.AttackerTurn && _takesInput && AllowInput)
                {
                    HideAttackerUI();
                    StartCoroutine(PressButton());
                }
            }
            else if (_parent.name == InlineStrings.DEFENDERTAG && _takesInput && AllowInput && !BattleStateMachine.AttackerTurn)
            {
                AllowInput = false;
                StartCoroutine(PressButton());
                BattleStateMachine.EndDefendersTurn();
            }
        }
    }

    void HideAttackerUI()
    {
        BattleStateMachine.EndAttackersTurn();
    }

    IEnumerator PressButton()
    {
        _takesInput = false;
        _button.onClick.Invoke();
        yield return new WaitForSeconds(0.4f);
        _takesInput = true;
    }
}