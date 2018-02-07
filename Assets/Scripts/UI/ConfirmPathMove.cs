using UnityEngine;
using System.Collections;
using PlayerCharacters;
using UI;
using Utility;

public class ConfirmPathMove : MonoBehaviour
{
    public delegate void PathConfirmedAction();
    public static event PathConfirmedAction OnPathConfirmed;

    private TurnManager _turnManager;

    [SerializeField] private GameObject _pathMoveMenu;

    public static bool IsConfirming;

    private void Start()
    {
        _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
    }

    private void OnEnable()
    {
        PlayerTarget.OnBoardTargetSelected += OpenPathMoveMenu;
        
    }

    private void OnDisable()
    {
        PlayerTarget.OnBoardTargetSelected -= OpenPathMoveMenu;
    }

    private void OpenPathMoveMenu()
    {
        IsConfirming = true;
        _pathMoveMenu.SetActive(true);
    }

    public void ConfirmThePathMove()
    {
        _turnManager.ActivePlayer.MoveAlongPath(PlayerTarget.BoardTarget.BoardPosition);
        if (OnPathConfirmed != null)
            OnPathConfirmed();
        DenyThePathMove();
    }

    public void DenyThePathMove()
    {
        _pathMoveMenu.SetActive(false);
        IsConfirming = false;
        PlayerTarget.ClearTargets();
    }
}