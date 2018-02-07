using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activeself : MonoBehaviour {

	public void ActiveSelf()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
