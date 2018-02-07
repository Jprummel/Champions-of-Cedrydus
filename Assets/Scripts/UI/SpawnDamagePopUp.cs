using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SpawnDamagePopUp : MonoBehaviour {

    public delegate void ShowDamageEvent(string damage,Vector2 spawnPos);
    public static ShowDamageEvent OnDamage;

    [SerializeField] private GameObject _damagePopUp;

	void OnEnable () {
        OnDamage += ShowDamage;
	}

    void ShowDamage(string damage, Vector2 spawnPos)
    {
        GameObject damagePopUp = Instantiate(_damagePopUp);
        damagePopUp.transform.position = spawnPos;
        Text damageText = damagePopUp.GetComponentInChildren<Text>();
        damageText.text = damage;
    }

    private void OnDisable()
    {
        OnDamage -= ShowDamage;
    }
}
