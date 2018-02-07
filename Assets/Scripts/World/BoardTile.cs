using PlayerCharacters;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;

public class BoardTile : MonoBehaviour
{
    [HideInInspector] public Biome BattleBiome;

    private SpriteRenderer _sr;

    public delegate void TileEventDoneAction(int turnMutation);
    public static event TileEventDoneAction OnTileEventDone;

    [HideInInspector] public List<PlayerCharacter> PlayersOnThisTile = new List<PlayerCharacter>();

    protected ActivateHealingPopUp _healingPopUp;
    protected RotateSpinner _spinner;
    protected TurnManager _turnManager;

    private bool _holdsPlayer;
    public bool IsHightlighted;

    public bool HoldsPlayer
    {
        get { return _holdsPlayer; }
        set { _holdsPlayer = value; }
    }

    public int2 BoardPosition;

    private void Awake()
    {
        if (gameObject.name != InlineStrings.HEALINGPOPUPTAG)
        {
            _spinner = GameObject.FindWithTag(InlineStrings.SPINNERTAG).GetComponentInChildren<RotateSpinner>(true);
        }

        _turnManager = GameObject.FindWithTag(InlineStrings.TURNMANAGERTAG).GetComponent<TurnManager>();
        _healingPopUp = GameObject.FindWithTag(InlineStrings.HEALINGPOPUPTAG).GetComponent<ActivateHealingPopUp>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public virtual void OnEnable()
    {
        PlayerMovement.OnClearPrediction += ChangeHighlight;
        EnableCrosshair.OnUnighlight += ChangeHighlight;
    }

    public virtual void OnDisable()
    {
        PlayerMovement.OnClearPrediction -= ChangeHighlight;
        EnableCrosshair.OnUnighlight -= ChangeHighlight;
    }

    public virtual void PlayerLandsOnTile(PlayerCharacter player)
    {
        _holdsPlayer = true;
        PlayersOnThisTile.Add(player);
    }

    public virtual void TileEventDone()
    {
        if (OnTileEventDone != null)
        {
            OnTileEventDone(1);
        }
    }

    public virtual void ChangeHighlight(bool isHighlighted)
    {
        IsHightlighted = isHighlighted;
        if (isHighlighted)
        {
            _sr.color = Color.yellow;
            
        }  
        else
        {
            _sr.color = Color.white;
        }
            
    }
}