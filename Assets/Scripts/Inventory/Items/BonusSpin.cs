using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Utility;

public class BonusSpin : UsableItem
{
    private GameObject _spinner;

    private bool _isItemUsed = false;

    private void OnEnable()
    {
        RotateSpinner.OnSpinnerDone += SpinAnotherTime;
        _spinner = GameObject.FindWithTag(InlineStrings.SPINNERTAG).transform.GetChild(0).gameObject;
    }

    private void OnDisable()
    {
        RotateSpinner.OnSpinnerDone -= SpinAnotherTime;
    }

    public override void UseItem()
    {
        CloseInventory();
        ExecuteUsableEffect();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Extra spinner
        _spinner.SetActive(true);
        _isItemUsed = true;
    }

    private void SpinAnotherTime()
    {
        StartCoroutine(SpinTwiceDelay());
    }

    private IEnumerator SpinTwiceDelay()
    {
        yield return new WaitForEndOfFrame();

        if (_isItemUsed)
            _spinner.SetActive(true);

        RemoveItemFromInventory();
    }
}
