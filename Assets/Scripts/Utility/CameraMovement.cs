/*
	CameraMovement.cs
	Created 12/18/2017 9:54:05 AM
	Project Boardgame by Base Games
*/
using UI;
using UnityEngine;

namespace Utility
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject _stickAnimationGameObject;

        private float _movementSpeed = 50f;
        public float _minXPos = -10f, _maxXPos = 500f, _minYPos = -127f, _maxYPos = 5f;
        private float _standardMinXPos, _standardMaxXPos, _standardMinYPos, _standardMaxYPos;
        private float _zoomedMinXPos = 35f, _zoomedMaxXPos = 155f, _zoomedMinYPos = -105f, _zoomedMaxYPos = -13f;
        private float _maxCameraSize = 30f, _minCameraSize = 15f, _cameraSize = 15f;

        private Rigidbody2D _rb2D;
        private Camera _mainCamera;

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _mainCamera = GetComponent<Camera>();

            _standardMinXPos = _minXPos;
            _standardMaxXPos = _maxXPos;
            _standardMinYPos = _minYPos;
            _standardMaxYPos = _maxYPos;
        }

        private void FixedUpdate()
        {
            if (GameStateManager.TimeGameState != GameState.PAUSED && PlayerTarget.IsSelectingTarget)
            {
                if (CameraFollowPlayer.OnChangeTarget != null)
                    CameraFollowPlayer.OnChangeTarget(null);
                MoveCamera();
            }
            else
            {
                _rb2D.velocity = Vector2.zero;
            }
        }

        private void MoveCamera()
        {
            float leftStickX = Input.GetAxis(InputStrings.CONTROLLER_LEFTSTICK_HORIZONTAL) * _movementSpeed;
            float leftStickY = Input.GetAxis(InputStrings.CONTROLLER_LEFTSTICK_VERTICAL) * _movementSpeed;

            _cameraSize += Input.GetAxis(InputStrings.CONTROLLER_RIGHTSTICK_VERTICAL);

            _rb2D.velocity = new Vector2(leftStickX, -leftStickY);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minXPos, _maxXPos), Mathf.Clamp(transform.position.y, _minYPos, _maxYPos), transform.position.z);

            _cameraSize = Mathf.Clamp(_cameraSize, _minCameraSize, _maxCameraSize);

            _mainCamera.orthographicSize = _cameraSize;

            _minXPos = _standardMinXPos + CalculateDifference(_standardMinXPos, _zoomedMinXPos);
            _maxXPos = _standardMaxXPos - CalculateDifference(_standardMaxXPos, _zoomedMaxXPos);
            _minYPos = _standardMinYPos + CalculateDifference(_standardMinYPos, _zoomedMinYPos);
            _maxYPos = _standardMaxYPos - CalculateDifference(_standardMaxYPos, _zoomedMaxYPos);

            if(_cameraSize > 25f && (transform.position.x == _zoomedMinXPos || transform.position.x == _zoomedMaxXPos || transform.position.y == _zoomedMinYPos || transform.position.y == _zoomedMaxYPos))
            {
                _stickAnimationGameObject.SetActive(true);
            }
            else
            {
                _stickAnimationGameObject.SetActive(false);
            }
        }

        private float CalculateDifference(float nonZoomPos, float zoomPos)
        {
            float newCalculation = (_cameraSize - 15f) * (((Mathf.Abs(nonZoomPos - zoomPos)) / 15f));
            return newCalculation;
        }
    }
}