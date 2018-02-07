using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Utility
{
    public class RotateSpinner : MonoBehaviour
    {
        public delegate void SpinnerDoneAction();
        public static event SpinnerDoneAction OnSpinnerDone;

        [SerializeField] private GameObject _spinnerItemPrefab;
        [SerializeField] private GameObject _stopSpinButton;
        
        [SerializeField] public List<Sprite> SpinnerImages;
        [SerializeField] private Sprite _normalSprite, _highlightedSprite = null;
        public List<string> SpinnerText = new List<string>();
        private List<string> _defaultSpinnerItems;

        private List<GameObject> _spinnerGameObjects = new List<GameObject>();

        [SerializeField] private Transform _arrow;

        private float _spinTime = .1f;
        private float _radius = 160f;
        private Vector3 _center;

        public int NumberOfMoves { get; set; }
        public int RandomSpin { get; set; }
        public int NumberOfSpinnerItems {get; set;}
        private int _currentSpinDirection;
        private int _stopIndex;
        private bool _stopped;
        private Ease _spinEase;

        [HideInInspector]public BoardTile SpinnerTile = null;

        private List<float> _spinnerRotations = new List<float>();
        private List<float> _spinnerProbabilities = new List<float>() { 4f, 16f, 16f, 16f, 16f, 16f, 16f };
        //private List<float> _spinnerProbabilities = new List<float>() { 0f, 100f, 0f, 0f, 0f, 0f, 0f };
#pragma warning disable 0414
        private Coroutine _rotatingCoroutine;
#pragma warning restore 0414

        public bool IsSpinnerText { get; set; }
        public bool IsSpinning;
        public static bool IsNewTurn;

        private void Awake()
        {
            _defaultSpinnerItems = SpinnerText;
            _center = _arrow.localPosition;
            NumberOfSpinnerItems = SpinnerText.Count;
            IsSpinnerText = true;
            IsNewTurn = true;
        }

        /// <summary>
        /// Sets the spinner items.
        /// </summary>
        private void OnEnable()
        {
            _spinEase = Ease.Linear;
            _spinTime = 0.1f;
            _stopped = false;

            if (IsNewTurn)
                ResetSpinnerItems();
            _stopSpinButton.SetActive(true);
            GameObject spinnerItemPrefabClone;

            _spinnerRotations.Clear();
            if(_spinnerGameObjects.Count == 0)
            {
                for (int i = 0; i < NumberOfSpinnerItems; i++)
                {
                    spinnerItemPrefabClone = Instantiate(_spinnerItemPrefab);
                    _spinnerGameObjects.Add(spinnerItemPrefabClone);
                }
            }
            else
            {
                if (NumberOfSpinnerItems > _spinnerGameObjects.Count)
                {
                    for (int j = 0; j < NumberOfSpinnerItems; j++)
                    {
                        if (j > _spinnerGameObjects.Count - 1)
                        {
                            spinnerItemPrefabClone = Instantiate(_spinnerItemPrefab);
                            _spinnerGameObjects.Add(spinnerItemPrefabClone);
                        }
                    }
                }
                else if (_spinnerGameObjects.Count > NumberOfSpinnerItems)
                {
                    for (int j= 0; j < _spinnerGameObjects.Count; j++)
                    {
                        if(j > NumberOfSpinnerItems - 1)
                        {
                            _spinnerGameObjects[j].SetActive(false);
                        }
                    }
                }
            }

            for (int k = 0; k < NumberOfSpinnerItems; k++)
            {
                _spinnerRotations.Add((360f / NumberOfSpinnerItems) * (k + 1) - ((360f / NumberOfSpinnerItems) / 2));
                _spinnerGameObjects[k].SetActive(true);
                _spinnerGameObjects[k].transform.SetParent(transform);
                _spinnerGameObjects[k].transform.localScale = Vector3.one;
                _spinnerGameObjects[k].transform.localPosition = CalculateSpinnerItemPos(_spinnerRotations[k]);
                if (IsSpinnerText)
                    _spinnerGameObjects[k].GetComponent<SetSpinnerItem>().SetTextItem(SpinnerText[k]);
                else
                    _spinnerGameObjects[k].GetComponent<SetSpinnerItem>().SetImageItem(SpinnerImages[k]);
            }
            StartRotation();
        }

        private IEnumerator SpinnerRotate()
        {
            Tween rotateTween = _arrow.DORotate(new Vector3(0, 0, -_spinnerRotations[_currentSpinDirection]), _spinTime).SetEase(_spinEase);

            yield return rotateTween.WaitForCompletion();

            for (int j = 0; j < _spinnerGameObjects.Count; j++)
            {
                _spinnerGameObjects[j].GetComponent<Image>().sprite = _normalSprite;
            }
            _spinnerGameObjects[_currentSpinDirection].GetComponent<Image>().sprite = _highlightedSprite;

            if (_currentSpinDirection != _stopIndex)
            {
                _currentSpinDirection += 1;
                if (_currentSpinDirection > _spinnerRotations.Count - 1)
                    _currentSpinDirection = 0;
                _rotatingCoroutine = StartCoroutine(SpinnerRotate());
            }
            else
            {
                StartCoroutine(SpinnerDisappear());
            }

            if (_stopped)
                _spinTime += 0.025f;
        }

        private Vector3 CalculateSpinnerItemPos(float ang)
        {
            Vector3 pos = new Vector3()
            {
                x = _center.x + _radius * Mathf.Sin(ang * Mathf.Deg2Rad),
                y = _center.y + _radius * Mathf.Cos(ang * Mathf.Deg2Rad)
            };
            return pos;
        }

        public void StartRotation()
        {
            _stopIndex = -1;
            _currentSpinDirection = 0; //oh shiet ik maakte weer een reference reeee
            _rotatingCoroutine = StartCoroutine(SpinnerRotate());
        }

        public void StopRotation()
        {
            IsSpinning = true;
            _stopped = true;
            _stopSpinButton.SetActive(false);
            RandomSpin = Random.Range(0, NumberOfSpinnerItems);
            if(NumberOfSpinnerItems == 7)
            {
                RandomSpin = RandomSpin.CalculateProbability(_spinnerProbabilities);
                NumberOfMoves = RandomSpin;
            }
            else
            {
                NumberOfMoves = 0;
            }
            StartCoroutine(ShowDrop());
        }

        private IEnumerator ShowDrop()
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            _stopIndex = RandomSpin;
        }

        private IEnumerator SpinnerDisappear()
        {
            yield return new WaitForSeconds(1f);
            if (SpinnerTile != null)
            {
                SpinnerTile.TileEventDone();
                SpinnerTile = null;
            }
            if (OnSpinnerDone != null)
                OnSpinnerDone();


            IsSpinning = false;
            gameObject.SetActive(false);
        }

        public void ResetSpinnerItems()
        {
            SpinnerText = _defaultSpinnerItems;
            NumberOfSpinnerItems = SpinnerText.Count;
            IsSpinnerText = true;
        }
    }
}