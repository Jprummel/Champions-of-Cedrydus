using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using PlayerCharacters;
using DG.Tweening;

public class TownTeleport : UsableItem
{
    private TownTile[] _townTiles;

    private void Start()
    {
        _townTiles = FindObjectsOfType<TownTile>();
    }

    public override void UseItem()
    {
        ExecuteUsableEffect();
    }

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        CloseInventory();

        Transform userTransform = _turnManager.ActivePlayer.transform;

        if (CameraFollowPlayer.OnChangeTarget != null)
            CameraFollowPlayer.OnChangeTarget(userTransform);

        Sequence teleportSequence = DOTween.Sequence();
        teleportSequence.Append(userTransform.DOScaleX(0.2f, 0.5f));
        teleportSequence.Append(userTransform.DOScaleY(0, 0.5f));
        teleportSequence.AppendCallback(() => MovePlayer());
        teleportSequence.Append(userTransform.DOScaleY(1, 0.5f));
        teleportSequence.Append(userTransform.DOScaleX(1, 0.5f));
        teleportSequence.AppendCallback(() => CompleteUsage());
        teleportSequence.SetLoops(1);
        //Teleport to a random town, ends turn
    }

    private void MovePlayer()
    {
        int randomTown = Random.Range(0, _townTiles.Length - 1);
        _user.transform.position = _townTiles[randomTown].transform.position;
        _turnManager.ActivePlayer.PositionOnMap = _townTiles[randomTown].BoardPosition;
        
    }

    private void CompleteUsage()
    {
        RemoveItemFromInventory();
        _turnManager.ExecuteTileEvent();
    }
}
