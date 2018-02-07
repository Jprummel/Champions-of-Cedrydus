/*
	PlayerMovement.cs
	Created 12/1/2017 9:07:58 AM
	Project Boardgame by Base Games
*/
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utility;
using World;
using UnityEngine.SceneManagement;

namespace PlayerCharacters
{
    [System.Serializable]
    public class Route
    {
        public bool IsViableRoute = true;
        public int2 EndPosition;
        public List<int2> MovePath = new List<int2>();
        public List<int2> PreviousPositions = new List<int2>();
        public int NumberOfMoves;
    }

    public class PlayerMovement : MonoBehaviour
    {
        public delegate void FinishedTurnAction();
        public static event FinishedTurnAction OnFinishedPathMove;

        public delegate void ClearPredictionAction(bool isHighlighted);
        public static event ClearPredictionAction OnClearPrediction;

        public delegate void MovesGoneAction();
        public static event MovesGoneAction OnMovesGone;

        public delegate void MovedASpaceAction();
        public static event MovedASpaceAction OnMovedASpace;

        public int NumberOfMoves { get; set; }

        [HideInInspector] public Map MapScript { get; set; }

        public List<int2> _previousTiles = new List<int2>();

        /*[HideInInspector]*/
        public int2 PositionOnMap = new int2(0, 6);
        private int2 _simulatedPosition;

        [SerializeField] private List<Route> _possibleRoutes = new List<Route>();

        [HideInInspector] public bool CanMoveAnyPlace = false;
        public bool PlayerHasMoves = false;
        public bool IsPlayerTurn { get; set; }
        private bool _canMove = true;
        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        private float _moveDelay = 0.25f;

        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            StartCoroutine(SetPositionDelay());
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode lsMode)
        {
            StartCoroutine(SetPositionDelay());
        }

        private IEnumerator SetPositionDelay()
        {
            yield return new WaitForEndOfFrame();
            if (GameStateManager.CurrentGameState == GameState.WORLD && SceneManager.GetActiveScene().name != InlineStrings.MAINMENUSCENE && SceneManager.GetActiveScene().name != InlineStrings.CHARACTERCREATIONSCENE && SceneManager.GetActiveScene().name != InlineStrings.WINSCENE)
            {
                MapScript = GameObject.FindWithTag(InlineStrings.MAPTAG).GetComponent<Map>();
                transform.position = MapScript.BoardToWorldPos(PositionOnMap);
            }
        }

        private void OnEnable()
        {
            TurnManager.OnResetTurn += ResetTurn;
            InputManager.OnMovementInput += MovePlayer;
            InputManager.OnXButton += CalculatePath;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DecryptionTool.OnDecryptionTool += CalculatePath;
        }

        private void OnDisable()
        {
            TurnManager.OnResetTurn -= ResetTurn;
            InputManager.OnMovementInput -= MovePlayer;
            InputManager.OnXButton -= CalculatePath;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            DecryptionTool.OnDecryptionTool -= CalculatePath;
        }

        public void ChangeSortingOrder(int sortingOrder)
        {
            _sr.sortingOrder = sortingOrder;
        }

        private void MovePlayer(int2 direction)
        {
            if (IsPlayerTurn && GameStateManager.TimeGameState == GameState.PLAYING && _canMove && !PlayerTarget.IsSelectingTarget)
            {
                _canMove = false;
                ExecuteMove(CalculateMove(direction, PositionOnMap));
                StartCoroutine(MoveDelay());
            }
        }

        private IEnumerator MoveDelay()
        {
            yield return new WaitForSeconds(_moveDelay);
            _canMove = true;
        }

        private int2 CalculateMove(int2 moveDirection, int2 startingMove)
        {
            int2 newPos = startingMove;
            int2 newMovePos = startingMove + moveDirection;

            if (!IsInBounds(newMovePos))
            {
                return new int2(-1, -1);
            }

            int simulatedSpot = MapScript.WorldMap[newMovePos.y, newMovePos.x];

            if (simulatedSpot < 3)
            {
                while (simulatedSpot < 3)
                {
                    if (simulatedSpot == 0)
                    {
                        return new int2(-1, -1);
                    }
                    newPos += moveDirection;
                    simulatedSpot = MapScript.WorldMap[newPos.y, newPos.x];
                }
            }
            else
            {
                newPos += moveDirection;
            }
            return newPos;
        }

        private bool IsInBounds(int2 newMovePos)
        {
            if (newMovePos.x > MapScript.WorldMap.GetLongLength(1) - 1
                || newMovePos.x < 0
                || newMovePos.y > MapScript.WorldMap.GetLongLength(0) - 1
                || newMovePos.y < 0)
            {
                return false;
            }

            return true;
        }

        public void CalculatePath()
        {
            _possibleRoutes.Clear();

            if(CanMoveAnyPlace)
            {
                for (int i = 0; i < 7; i++)
                {
                    NumberOfMoves = i;
                    CalcPath();
                }
            }
            else
            {
                CalcPath();
            }
        }

        private void CalcPath()
        {
            if (GameStateManager.TimeGameState == GameState.PLAYING && IsPlayerTurn && _previousTiles.Count == 0 && _canMove)
            {
                List<Route> newPossibleRoutes = new List<Route>();

                PlayerTarget._TargetingMode = TargetingMode.AnyTile;

                _simulatedPosition = PositionOnMap;

                List<int2> walkable = CalculateWalkablePoints();

                if (NumberOfMoves > 0)
                {
                    for (int i = 0; i < walkable.Count; i++)
                    {
                        Route route = new Route();
                        route.PreviousPositions.Add(_simulatedPosition);
                        route.MovePath.Add(walkable[i]);
                        route.NumberOfMoves = NumberOfMoves;
                        route.NumberOfMoves -= 1;
                        newPossibleRoutes.Add(route);
                    }
                    int numberOfMoves = 0;
                    numberOfMoves += newPossibleRoutes[0].NumberOfMoves;
                    for (int i = 0; i < numberOfMoves; i++)
                    {
                        for (int j = 0; j < newPossibleRoutes.Count; j++)
                        {
                            if (newPossibleRoutes[j].NumberOfMoves > 0 && newPossibleRoutes[j].IsViableRoute)
                            {
                                _simulatedPosition = newPossibleRoutes[j].MovePath[newPossibleRoutes[j].MovePath.Count - 1];

                                walkable = CalculateWalkablePoints();
                                List<int2> newWalkable = new List<int2>();

                                for (int m = 0; m < walkable.Count; m++)
                                {
                                    if (walkable[m] != newPossibleRoutes[j].PreviousPositions[newPossibleRoutes[j].PreviousPositions.Count - 1])
                                    {
                                        newWalkable.Add(walkable[m]);
                                    }
                                }

                                if (newWalkable.Count > 1)
                                {
                                    for (int l = 0; l < newWalkable.Count; l++)
                                    {
                                        if (l > 0)
                                        {
                                            Route newRoute = new Route();
                                            newRoute.PreviousPositions = new List<int2>(newPossibleRoutes[j].PreviousPositions);
                                            newRoute.MovePath = new List<int2>(newPossibleRoutes[j].MovePath);
                                            newRoute.NumberOfMoves += newPossibleRoutes[j].NumberOfMoves;
                                            newRoute.PreviousPositions.Add(_simulatedPosition);
                                            newRoute.MovePath.Add(newWalkable[l]);
                                            newRoute.NumberOfMoves -= 1;
                                            newPossibleRoutes.Add(newRoute);
                                        }
                                    }
                                }
                                if (newWalkable.Count == 0)
                                {
                                    newPossibleRoutes[j].IsViableRoute = false;
                                }
                                else
                                {
                                    newPossibleRoutes[j].PreviousPositions.Add(_simulatedPosition);
                                    newPossibleRoutes[j].MovePath.Add(newWalkable[0]);

                                    newPossibleRoutes[j].NumberOfMoves -= 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Route route = new Route();
                    route.MovePath.Add(_simulatedPosition);
                    route.NumberOfMoves = NumberOfMoves;
                    _possibleRoutes.Add(route);
                }



                for (int i = 0; i < newPossibleRoutes.Count; i++)
                {
                    if (newPossibleRoutes[i].IsViableRoute)
                        _possibleRoutes.Add(newPossibleRoutes[i]);
                }

                if (OnClearPrediction != null)
                    OnClearPrediction(false);

                int2 endPosition;

                for (int i = 0; i < _possibleRoutes.Count; i++)
                {
                    endPosition = _possibleRoutes[i].MovePath[_possibleRoutes[i].MovePath.Count - 1];
                    _possibleRoutes[i].EndPosition = endPosition;
                    MapScript.BoardTiles[endPosition.y, endPosition.x].ChangeHighlight(true);
                }
            }
        }

        public void MoveAlongPath(int2 endPosition)
        {
            _canMove = false;
            if (CameraFollowPlayer.OnChangeTarget != null)
                CameraFollowPlayer.OnChangeTarget(transform);

            if (OnClearPrediction != null)
                OnClearPrediction(false);

            List<int2> chosenPath = new List<int2>();
            int2 chosenBoardPos = new int2(-1, -1);
            for (int i = 0; i < _possibleRoutes.Count; i++)
            {
                if (_possibleRoutes[i].EndPosition == endPosition)
                {
                    chosenPath = _possibleRoutes[i].MovePath;
                    chosenBoardPos = _possibleRoutes[i].EndPosition;
                    break;
                }
            }

            Sequence movePathSequence = DOTween.Sequence();
            
            for (int j = 0; j < chosenPath.Count; j++)
            {
                movePathSequence.AppendCallback(() => MovedSpace());
                movePathSequence.Append(transform.DOMove(MapScript.BoardToWorldPos(chosenPath[j]), _moveDelay).SetEase(Ease.Linear));
                movePathSequence.AppendInterval(.05f);
            }

            movePathSequence.AppendCallback(() => FinishPathMoveTurn());

            movePathSequence.SetLoops(1);

            PositionOnMap = chosenBoardPos;
        }

        private void MovedSpace()
        {
            SoundManager.instance.PlaySound(SoundsDatabase.AudioClips["MoveSound"], pitch: Random.Range(0.98f, 1.01f), volume: 0.25f);
            if (OnMovedASpace != null)
                OnMovedASpace();
        }

        private void FinishPathMoveTurn()
        {
            if (CameraFollowPlayer.OnChangeTarget != null)
                CameraFollowPlayer.OnChangeTarget(null);
            NumberOfMoves = 0;
            if (OnFinishedPathMove != null)
            {
                OnFinishedPathMove();
            }
            if (OnMovedASpace != null)
                OnMovedASpace();

            _canMove = true;
        }

        private List<int2> CalculateWalkablePoints()
        {
            List<int2> walkablePositions = new List<int2>();
            int2 calculatedPos;

            calculatedPos = CalculateMove(new int2(0, -1), _simulatedPosition);
            if (calculatedPos != new int2(-1, -1))
                walkablePositions.Add(calculatedPos);

            calculatedPos = CalculateMove(new int2(-1, 0), _simulatedPosition);
            if (calculatedPos != new int2(-1, -1))
                walkablePositions.Add(calculatedPos);

            calculatedPos = CalculateMove(new int2(0, 1), _simulatedPosition);
            if (calculatedPos != new int2(-1, -1))
                walkablePositions.Add(calculatedPos);

            calculatedPos = CalculateMove(new int2(1, 0), _simulatedPosition);
            if (calculatedPos != new int2(-1, -1))
                walkablePositions.Add(calculatedPos);

            return walkablePositions;
        }

        private void ExecuteMove(int2 positionToMoveTo)
        {
            if (positionToMoveTo != new int2(-1, -1) && NumberOfMoves > 0 || _previousTiles.Count > 0 && positionToMoveTo == _previousTiles[_previousTiles.Count - 1])
            {
                if (_previousTiles.Count > 0)
                {
                    if (positionToMoveTo == _previousTiles[_previousTiles.Count - 1])
                    {
                        _previousTiles.RemoveAt(_previousTiles.Count - 1);
                        NumberOfMoves += 1;

                        MovedSpace();
                    }
                    else
                    {
                        _previousTiles.Add(PositionOnMap);
                        NumberOfMoves -= 1;

                        MovedSpace();
                    }
                }
                else
                {
                    _previousTiles.Add(PositionOnMap);
                    NumberOfMoves -= 1;

                    MovedSpace();
                }

                PositionOnMap = positionToMoveTo;

                transform.DOMove(MapScript.BoardToWorldPos(PositionOnMap), _moveDelay).SetEase(Ease.Linear).OnComplete(() => MoveConfirmation());
            }
            else
            {
                MoveConfirmation();
            }
        }

        private void MoveConfirmation()
        {
            if ((NumberOfMoves == 0) && GameStateManager.TimeGameState == GameState.PLAYING && IsPlayerTurn && PlayerHasMoves)
            {
                if (OnMovesGone != null)
                    OnMovesGone();
            }
        }

        private void ResetTurn()
        {
            _previousTiles.Clear();
        }
    }
}