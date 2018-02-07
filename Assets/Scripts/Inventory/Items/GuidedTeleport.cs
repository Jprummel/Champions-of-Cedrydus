using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using UI;
using PlayerCharacters;
using DG.Tweening;

public class GuidedTeleport : UsableItem
{
    public override void UseItem()
    {
        PlayerTarget._TargetingMode = TargetingMode.Towns;
        base.UseItem();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Teleport to a chosen town, ends turn
        Transform userTransform = _turnManager.ActivePlayer.transform;

        if (CameraFollowPlayer.OnChangeTarget != null)
            CameraFollowPlayer.OnChangeTarget(userTransform);

        Sequence teleportSequence = DOTween.Sequence();
        teleportSequence.Append(userTransform.DOScaleX(0.2f, 0.5f));
        teleportSequence.Append(userTransform.DOScaleY(0, 0.5f));
        teleportSequence.AppendCallback(() => MovePlayer());
        teleportSequence.Append(userTransform.DOScaleY(1, 0.5f));
        teleportSequence.Append(userTransform.DOScaleX(1, 0.5f));
        teleportSequence.AppendCallback(() => CompleteTeleport());
        //teleportSequence.SetLoops(1);
    }

    private void MovePlayer()
    {
        _user.transform.position = PlayerTarget.TownTarget.transform.position;
        _turnManager.ActivePlayer.PositionOnMap = PlayerTarget.TownTarget.BoardPosition;
    }

    private void CompleteTeleport()
    {
        RemoveItemFromInventory();
        _turnManager.ExecuteTileEvent();
    }
}