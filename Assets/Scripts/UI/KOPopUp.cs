using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KOPopUp : MonoBehaviour {

    public delegate void RecoveryTextEvent(Character character);
    public static RecoveryTextEvent OnRecoveryTextEvent;

    [SerializeField] private Text _recoveryText;

    private void OnEnable()
    {
        OnRecoveryTextEvent += ShowRecoveryText;
    }

    void ShowRecoveryText(Character character)
    {
        if (character.TurnsDead > 1)
        {
            _recoveryText.text = character.CharacterName + " is recovering power for " + character.TurnsDead + " more turns";
        }
        else
        {
            _recoveryText.text = character.CharacterName + " is recovering power for " + character.TurnsDead + " more turn";
        }
    }

    private void OnDisable()
    {
        OnRecoveryTextEvent -= ShowRecoveryText;
    }
}
