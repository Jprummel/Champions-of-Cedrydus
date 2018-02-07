using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateItemGain : MonoBehaviour {

    public delegate void SetPanelActive(bool active, string ItemName, Sprite ItemSprite);
    public static SetPanelActive OnSetActive;
    [SerializeField] private GameObject obj;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemSprite;

    private void OnEnable()
    {
        OnSetActive += SetActive;
    }

    private void OnDisable()
    {
        OnSetActive -= SetActive;
    }

    void SetActive(bool active, string ItemName, Sprite ItemSprite)
    {
        itemText.text = "You have gained 1: " + ItemName + "!";
        itemSprite.sprite = ItemSprite;
        obj.SetActive(active);
    }
}
