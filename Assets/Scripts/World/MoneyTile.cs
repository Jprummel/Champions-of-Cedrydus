using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class MoneyTile : BoardTile
{
    [SerializeField] private List<string> spinnerObjects;

    public override void PlayerLandsOnTile(PlayerCharacter player)
    {
        base.PlayerLandsOnTile(player);
        RotateSpinner.IsNewTurn = false;
        _spinner.IsSpinnerText = true;
        _spinner.NumberOfSpinnerItems = spinnerObjects.Count;
        _spinner.SpinnerText = spinnerObjects;
        SpinnerStateManager.CurrentSpinnerState = SpinnerState.GAINABLESPINNER;
        _spinner.gameObject.SetActive(true);
        _spinner.SpinnerTile = this;
    }

    public override void TileEventDone()
    {
        base.TileEventDone();
        _turnManager.ActivePlayerCharacter.Gold += Int32.Parse(spinnerObjects[_spinner.RandomSpin]);
    }
}
