using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using UI;
using DG.Tweening;

public class LockDown : UsableItem
{
    public override void UseItem()
    {
        PlayerTarget.CanSelectAnyone = true;
        PlayerTarget._TargetingMode = TargetingMode.Players;
        base.UseItem();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Locks inventory of target player

        PlayerTarget.Target.TurnsInventoryLocked = 3;

        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(true, PlayerTarget.Target.CharacterName + "'s inventory is locked for 3 turns.");

        Sequence lockSequence = DOTween.Sequence();
        lockSequence.AppendInterval(2f);
        lockSequence.AppendCallback(() => CompleteUsage());
        lockSequence.SetLoops(1);
    }

    private void CompleteUsage()
    {
        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(false);

        EnableSpinner();

        RemoveItemFromInventory();
    }
}
