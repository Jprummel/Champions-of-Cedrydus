using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPunishmentUI : MonoBehaviour {

    public delegate void OnSetPunishmentUI(bool active);
    public static OnSetPunishmentUI SetPunishmentActive;

    [SerializeField] private GameObject _punishmentPanel;

    private void OnEnable()
    {
        SetPunishmentActive += SetUI;
    }

    void SetUI(bool active)
    {
        _punishmentPanel.SetActive(active);
    }

    private void OnDisable()
    {
        SetPunishmentActive -= SetUI;
    }
}
