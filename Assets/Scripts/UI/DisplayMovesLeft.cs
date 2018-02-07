using UnityEngine;
using System.Collections;
using Utility;
using UnityEngine.UI;
using PlayerCharacters;

public class DisplayMovesLeft : MonoBehaviour
{
    [SerializeField] private Text _numberOfMovesText;

    private TurnManager _turnmanager;

    private void Start()
    {
        _turnmanager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
    }

    private void OnEnable()
    {
        PlayerMovement.OnMovedASpace += DisplayMoves;
        RotateSpinner.OnSpinnerDone += DisplayMoves;
        PlayerOptionsMenu.OnSlowHacked += DisplayMoves;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMovedASpace -= DisplayMoves;
        RotateSpinner.OnSpinnerDone -= DisplayMoves;
        PlayerOptionsMenu.OnSlowHacked -= DisplayMoves;
    }

    private void DisplayMoves()
    {
        _numberOfMovesText.text = "You have " + _turnmanager.ActivePlayer.NumberOfMoves + " moves left";
    }
}