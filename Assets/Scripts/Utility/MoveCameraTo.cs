using DG.Tweening;
using UnityEngine;
using Utility;

public class MoveCameraTo : MonoBehaviour
{
    private CameraMovement _cameraMovement;

    public delegate void MoveCameraEvent(Transform transformToMoveTo);
    public static MoveCameraEvent OnMoveToPlayer;
    
    private void Awake()
    {
        OnMoveToPlayer += MoveCameraToPlayer;
        _cameraMovement = GetComponent<CameraMovement>();
    }

    private void MoveCameraToPlayer(Transform playerTransform)
    {
        float x;
        float y;
        Vector3 newCamPosition;
        x = Mathf.Clamp(playerTransform.position.x, _cameraMovement._minXPos, _cameraMovement._maxXPos);
        y = Mathf.Clamp(playerTransform.position.y, _cameraMovement._minYPos, _cameraMovement._maxYPos);
        newCamPosition = new Vector3(x, y, transform.position.z);
        transform.position = newCamPosition;
    }

    private void OnDestroy()
    {
        OnMoveToPlayer -= MoveCameraToPlayer;
    }
}
