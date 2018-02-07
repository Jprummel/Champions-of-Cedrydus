using UnityEngine;
using System.Collections;
using Utility;

public class CameraFollowPlayer : MonoBehaviour
{
    private CameraMovement _cameraMovement;
    public static bool FollowTarget = true;
    private Transform _target = null;

    public delegate void ChangeTargetAction(Transform target);
    public static ChangeTargetAction OnChangeTarget;

    private void OnEnable()
    {
        _cameraMovement = GetComponent<CameraMovement>();
        OnChangeTarget += ChangeTarget;
    }

    private void OnDisable()
    {
        OnChangeTarget -= ChangeTarget;
    }

    private void Update ()
    {
        if (_target != null && FollowTarget)
        {
            float x;
            float y;
            Vector3 newCamPosition;
            x = Mathf.Clamp(_target.position.x, _cameraMovement._minXPos, _cameraMovement._maxXPos);
            y = Mathf.Clamp(_target.position.y, _cameraMovement._minYPos, _cameraMovement._maxYPos);
            newCamPosition = new Vector3(x, y, transform.position.z);
            transform.position = newCamPosition;
        }
	}

    private void ChangeTarget(Transform target)
    {
        _target = target;
    }
}