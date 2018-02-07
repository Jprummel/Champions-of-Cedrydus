using UnityEngine;
using System.Collections;
 
public class ClearPlayerPrefs : MonoBehaviour
{
    public void Clear()
    {
        PlayerPrefs.DeleteAll();
    }
}