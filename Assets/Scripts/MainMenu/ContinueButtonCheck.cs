using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonCheck : MonoBehaviour {

    [SerializeField]private Button _continueButton;

	void Awake () {
		if(!File.Exists(Application.persistentDataPath + "/" + InlineStrings.CHARACTERSAVEFILE)){
            _continueButton.interactable = false;
        }
	}
}
