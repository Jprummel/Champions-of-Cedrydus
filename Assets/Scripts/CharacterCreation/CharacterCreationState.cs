using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationState : MonoBehaviour {

    public enum CreationState
    {
        CHOOSEPLAYERAMOUNT,
        CHARACTERCREATION,
    }

    public static CreationState CurrentCreationState;
}
