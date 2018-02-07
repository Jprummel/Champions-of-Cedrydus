using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerInfo : MonoBehaviour
{
    [SerializeField] private Image _winnerImage;
    [SerializeField] private Text _winnerText;

    public static Sprite WinnerSprite;
    public static string WinnerName;

    private void Start()
    {
        _winnerImage.sprite = WinnerSprite;
        _winnerImage.SetNativeSize();
        _winnerText.text = WinnerName + " has claimed Cedrydus!";

        WinnerSprite = null;
        WinnerName = string.Empty;
    }
}
