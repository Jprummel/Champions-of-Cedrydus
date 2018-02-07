using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicateItemEffect : MonoBehaviour
{
    public delegate void ShowIndicatorAction(bool showText, string indicatorText = null);
    public static ShowIndicatorAction OnShowIndicator;

    [SerializeField] private GameObject _indicatorGameObject;

    [SerializeField] private Text _indicatorText;

    private void OnEnable()
    {
        OnShowIndicator += ShowIndicator;
    }

    private void OnDisable()
    {
        OnShowIndicator -= ShowIndicator;
    }

    private void ShowIndicator(bool showText, string indicatorText = null)
    {
        if (indicatorText != null)
            _indicatorText.text = indicatorText;
        else
            _indicatorText.text = string.Empty;

        _indicatorGameObject.SetActive(showText);
    }
}