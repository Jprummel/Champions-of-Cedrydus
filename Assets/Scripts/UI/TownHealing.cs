using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownHealing : BoardTile {

    [SerializeField] private Text _goldCostText;
    [SerializeField] private GameObject _eventSystem;
    [SerializeField] private GameObject _errorPopUp;
    private int _goldCost;

    public override void OnEnable()
    {
        _goldCost = Mathf.RoundToInt((_turnManager.ActivePlayerCharacter.MaxHP - _turnManager.ActivePlayerCharacter.CurrentHP) * 3 );
        _goldCostText.text = "Would you like to rest for " + _goldCost.ToString() + " gold?";
    }

    public void HealPlayer()
    {
        if (_turnManager.ActivePlayerCharacter.Gold >= _goldCost)
        {
            _turnManager.ActivePlayerCharacter.CurrentHP = _turnManager.ActivePlayerCharacter.MaxHP;
            TileEventDone();
            gameObject.SetActive(false);
        }
        else
        {
            _eventSystem.SetActive(false);
            _errorPopUp.SetActive(true);
        }
    }

    public void DontHealPlayer()
    {
        TileEventDone();
        _errorPopUp.SetActive(false);
        _eventSystem.SetActive(true);
        gameObject.SetActive(false);
    }
}
