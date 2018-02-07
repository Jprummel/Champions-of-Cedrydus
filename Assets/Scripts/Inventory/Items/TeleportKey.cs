using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class TeleportKey : UsableItem {

    public override void ExecuteUsableEffect()
    {
        base.ExecuteUsableEffect();
        //Bring back to your base to win
    }
}
