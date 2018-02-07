using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamagePopUp : MonoBehaviour {

    void Start () {
        transform.DOJump(new Vector3(transform.position.x, transform.position.y + 1, 10), 2, 1, 1f);
        Destroy(this.gameObject, 1.5f);
    }
}