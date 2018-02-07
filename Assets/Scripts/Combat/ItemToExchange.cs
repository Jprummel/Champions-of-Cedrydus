using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToExchange : MonoBehaviour {

    public static string itemToTake;

	public void SetItem()
    {
        itemToTake = GetComponentInChildren<Text>().text;
    }
}
