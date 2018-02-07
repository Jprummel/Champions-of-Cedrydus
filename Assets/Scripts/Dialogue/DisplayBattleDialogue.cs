using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dialogue
{
    public class DisplayBattleDialogue : MonoBehaviour
    {
        public TextAsset DialogueFile;

        [SerializeField] private GameObject _dialogueBox;
        [SerializeField] private GameObject _battleTurnCards;

        [SerializeField] private Text _sourceText;
        [SerializeField] private Text _dialogueText;

        private Tween _textTween;

        private DialogueNode _dialogueNode;

        private string _fileName;

        private Coroutine _callBackDelayCoroutine;

        private bool _isDialogueDone;

        private void Awake()
        {
            RetrieveDialogueDone();
            if (!_isDialogueDone)
                StartDialogueFromBeginning();
            else
                _battleTurnCards.SetActive(true);
        }

        private void OnEnable()
        {
            InputManager.OnBattleAButton += SkipText;
        }

        private void OnDisable()
        {
            InputManager.OnBattleAButton -= SkipText;
        }

        public void StartDialogueFromBeginning()
        {
            DisplayGivenDialogue(DialogueFile.name, 1);
            _dialogueBox.SetActive(true);
        }

        private void RetrieveDialogueDone()
        {
            int dialogueDone = PlayerPrefs.GetInt(InlineStrings.ISBATTLEDIALOGUEDONE, 0);

            if (dialogueDone == 0)
                _isDialogueDone = false;
            else
                _isDialogueDone = true;
        }

        private void SkipText()
        {
            if (!_isDialogueDone && _dialogueBox.activeSelf)
            {
                if (_textTween != null && _textTween.IsPlaying())
                {
                    _textTween.Kill();
                    _dialogueText.text = _dialogueNode.Text;
                }
                else
                {
                    if (_dialogueNode.NodeDestination == 1000)
                    {
                        DisableDialogueBox();
                    }
                    else
                    {
                        DisplayGivenDialogue(_fileName, _dialogueNode.NodeDestination);
                    }
                }
            }
        }

        public void DisplayGivenDialogue(string fileName, int id)
        {
            _fileName = fileName;
            _dialogueBox.SetActive(true);

            _dialogueNode = DialogueLoader.LoadDialogueNode(fileName, id);

            if (_textTween != null)
            {
                _textTween.Kill();
            }

            _dialogueText.text = string.Empty;
            _sourceText.text = _dialogueNode.DialogueSource;
            _textTween = _dialogueText.DOText(_dialogueNode.Text, (float)_dialogueNode.Text.ToCharArray().Length / 20, scrambleMode: ScrambleMode.None);
        }

        private void DisableDialogueBox()
        {
            PlayerPrefs.SetInt(InlineStrings.ISBATTLEDIALOGUEDONE, 1);
            RetrieveDialogueDone();

            _dialogueBox.SetActive(false);

            _battleTurnCards.SetActive(true);
        }
    }
}