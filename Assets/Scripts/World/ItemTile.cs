using Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using System.Collections;

public class ItemTile : BoardTile
{
    [SerializeField] private List<Sprite> spinnerObjects;
    private List<Sprite> currentSpinnerObjects = new List<Sprite>();
    [SerializeField] private GameObject itemConfirmPanel;

    public override void PlayerLandsOnTile(PlayerCharacter player)
    {
        base.PlayerLandsOnTile(player);
        RotateSpinner.IsNewTurn = false;
        _spinner.IsSpinnerText = false;

        currentSpinnerObjects = GetItemSpinnerOptions();
        _spinner.NumberOfSpinnerItems = currentSpinnerObjects.Count;
        _spinner.SpinnerImages = currentSpinnerObjects;
        SpinnerStateManager.CurrentSpinnerState = SpinnerState.GAINABLESPINNER;
        _spinner.gameObject.SetActive(true);
        _spinner.SpinnerTile = this;
    }

    public override void TileEventDone()
    {
        _turnManager.ActivePlayerCharacter._PlayerInventory.AddInventoryItem(currentSpinnerObjects[_spinner.RandomSpin].name);
        StartCoroutine(SetItemgain());
    }

    private List<Sprite> GetItemSpinnerOptions()
    {
        List<int> addedItems = new List<int>();
        List<Sprite> ItemsForSpinner = new List<Sprite>();

        for (int i = 0; i < 8; i++)
        {
            int itemToAdd = GiveNonDuplicateInt(addedItems);
            addedItems.Add(itemToAdd);

            ItemsForSpinner.Add(spinnerObjects[itemToAdd]);
        }

        return ItemsForSpinner;
    }

    private int GiveNonDuplicateInt(List<int> ints)
    {
        int itemToShow = Random.Range(0, spinnerObjects.Count);

        while (ints.Contains(itemToShow))
        {
            itemToShow = Random.Range(0, spinnerObjects.Count);
        }

        return itemToShow;
    }

    IEnumerator SetItemgain()
    {
        ActivateItemGain.OnSetActive(true, currentSpinnerObjects[_spinner.RandomSpin].name, currentSpinnerObjects[_spinner.RandomSpin]);
        yield return new WaitForSeconds(2f);
        ActivateItemGain.OnSetActive(false, currentSpinnerObjects[_spinner.RandomSpin].name, currentSpinnerObjects[_spinner.RandomSpin]);
        base.TileEventDone();
    }
}