using UnityEngine;
using Inventory;

public class PlayerCharacter : Character
{
    public PlayerInventory _PlayerInventory = new PlayerInventory();
    public string ClassName { get; set; }

    public bool CanMove { get; set; }

    protected void Awake()
    {
        CharacterType = CharacterTypes.PLAYER;
    }

    public override void CalculateXpDifference(Character otherCharacter)
    {
        //If 2 player characters battle, calculate difference between total earned xps to jump to that characters level
        if(TotalXPEarned > otherCharacter.TotalXPEarned)
        {
            XpToGive = Mathf.RoundToInt(TotalXPEarned - otherCharacter.TotalXPEarned);
        }
    }
}