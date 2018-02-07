using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Combat;
using UI;
using DG.Tweening;

public class EchoCall : UsableItem
{
    private OngoingBattles _ongoingBattles;

    private void Start()
    {
        _ongoingBattles = GameObject.FindWithTag(InlineStrings.ONGOINGBATTLESTAG).GetComponent<OngoingBattles>();
    }

    public override void UseItem()
    {
        base.UseItem();
        PlayerTarget._TargetingMode = TargetingMode.Players;
        PlayerTarget.CanSelectAnyone = false;
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Spawn enemy on a player
        _ongoingBattles.AddBattle(PlayerTarget.Target.gameObject, EnemyDatabase.ReturnEnemy(0, 2).gameObject, PlayerTarget.Target.CharacterName + "'s battle");

        if (IndicateItemEffect.OnShowIndicator != null)
            IndicateItemEffect.OnShowIndicator(true, "An enemy has been spawned at " + PlayerTarget.Target.CharacterName + "'s position.");

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