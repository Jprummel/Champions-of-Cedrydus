using UnityEngine;

public enum SpinnerState
{
    MOVEMENTSPINNER,
    GAINABLESPINNER
}

public class SpinnerStateManager : MonoBehaviour {

    public static SpinnerState CurrentSpinnerState;	
}
