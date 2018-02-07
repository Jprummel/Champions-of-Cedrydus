using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationInput : MonoBehaviour {

    public delegate void CharacterCreationStateEvent(int stateIndex);
    public static CharacterCreationStateEvent OnSwitchState;

    [SerializeField] private AudioSource _audio;

    public enum CharacterCreationStates
    {
        CHOOSEPLAYERS = 1,
        CREATECHARACTERS = 2,
        CHARACTERNAMING = 3
    }

    public static CharacterCreationStates CharacterCreationState;

	void OnEnable () {
        CharacterCreationState = CharacterCreationStates.CHOOSEPLAYERS;
        OnSwitchState += SwitchState;
	}
	
	void Update () {

        switch (CharacterCreationState)
        {
            case CharacterCreationStates.CHOOSEPLAYERS:
                ChoosePlayersInput();
                break;
            case CharacterCreationStates.CREATECHARACTERS:
                CreateCharactersInput();
                break;
            case CharacterCreationStates.CHARACTERNAMING:
                CharacterNamingInput();
                break;
        }
	}

    void ChoosePlayersInput()
    {
        if (Input.GetButtonDown(InputStrings.CONTROLLER_B))
        {
            ChoosePlayerAmount.OnCancelCreation();
            _audio.Play();
        }
    }

    void CreateCharactersInput()
    {
        if (Input.GetButtonDown(InputStrings.CONTROLLER_B))
        {
            CharacterCreation.OnCancelCreation();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_Y))
        {
            CharacterCreation.OnNamingEvent();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_START))
        {
            CharacterCreation.OnFinishCreation();
        }
    }

    void CharacterNamingInput()
    {
        if (Input.GetButtonDown(InputStrings.CONTROLLER_X))
        {
            OnScreenKeyboard.OnDelete();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_B))
        {
            OnScreenKeyboard.OnCancel();
        }

        if (Input.GetButtonDown(InputStrings.CONTROLLER_START))
        {
            OnScreenKeyboard.OnFinish();
        }
    }

    public void SwitchState(int stateIndex)
    {
        //1 = Chooseplayers, 2 = CreateCharacters, 3 = CharacterNaming
        switch (stateIndex) {
            case 1:
                CharacterCreationState = CharacterCreationStates.CHOOSEPLAYERS;
                break;
            case 2:
                CharacterCreationState = CharacterCreationStates.CREATECHARACTERS;
                break;
            case 3:
                CharacterCreationState = CharacterCreationStates.CHARACTERNAMING;
                break;
        }
    }

    private void OnDisable()
    {
        OnSwitchState -= SwitchState;
    }
}
