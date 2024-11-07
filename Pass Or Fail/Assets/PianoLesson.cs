using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class PianoLesson : MonoBehaviour
{
    [SerializeField] private Animator pianoTeacher;
    [SerializeField] private Camera pianoCamera;
    [SerializeField] private ParticleSystem musicalNotes;
    [SerializeField] private Transform rightHand, leftHand;
    [SerializeField] private Transform[] rightSideKeysPoints, leftSideKeysPoints;
    [SerializeField] private PianoKey[] pianoKeysToPress;
    [SerializeField] private PianoClassStudent[] students;
    [SerializeField] private GameObject perfects, warnings;
    private int _keyIndex = 0;
    private Ray _ray;
    private RaycastHit _hit;
    private const int RayMaxDistance = 50;
    [SerializeField] private LayerMask layersToHit;
    private static readonly Vector3 KeyPressedEulerAngle = new (4f, 0f, 0f), RightHandDefaultPos = new (0.294f, 0.611f, -0.849f), 
        LeftHandDefaultPos = new (-0.179f,0.611f, -0.849f);
    private const string Animation0Name = "pressPianoKeyWithRightHand", Animation1Name = "pressPianoKeyWithLeftHand";
    private List<PianoKey> _pressedPianoKeys = new List<PianoKey>();
    private WaitForSeconds _delay = new WaitForSeconds(1f);
    private bool _isActivityFinished = false, _allowKeyPressing = false;
    public enum PianoKeySide
    {
        RightSide,
        LeftSide
    }
    public void StartActivity()
    {
        HighlightKey();
        _allowKeyPressing = true;
    }
    private void Update()
    {
        if(_isActivityFinished) return;
        if(!_allowKeyPressing) return;
        if (!Input.GetMouseButtonDown(0)) return;
        _ray = pianoCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(_ray, out _hit, RayMaxDistance, layersToHit)) return;
        Debug.Log(_hit.transform.name);
        MoveHandToPressKey(_hit.transform.GetComponent<PianoKey>());
    }
    private void PressKey(PianoKey pressedKey)
    {
        if(!_isActivityFinished)
            _pressedPianoKeys.Add(pressedKey);
        pressedKey.transform.DOLocalRotate(KeyPressedEulerAngle, 0.3f).OnComplete(() =>
        {
            if (!_isActivityFinished)
            {
                if (IsRightKey(pressedKey, pianoKeysToPress[_keyIndex]))
                {
                    //Debug.Log("Good");
                    perfects.SetActive(true);
                    SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
                }
                else
                {
                    //Debug.Log("Bad");
                    warnings.SetActive(true);
                    SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
                }
            }
            _keyIndex++;
            pressedKey.OnPressingKey();
            musicalNotes.Play();
            pressedKey.transform.DOLocalRotate(Vector3.zero, 0.3f);
            if(_isActivityFinished) return;
            if (_keyIndex >= pianoKeysToPress.Length)
            {
                //Debug.Log("ActivityEnds");
                _isActivityFinished = true;
                pianoKeysToPress[^1].HighlightKey(false);
                StartCoroutine(PlayPiano());
                StudentsStartPlayingPiano();
                EndActivity();
                return;
            }
            ChangeStudentsExpression();
            HighlightKey();
        });
    }
    private void MoveHandToPressKey(PianoKey key)
    {
        _allowKeyPressing = false;
        if (key.GetPianoKeySide() == PianoKeySide.RightSide)
        {
            var handTarget = rightSideKeysPoints[key.GetKeyNo()].position;
            pianoTeacher.Play(Animation0Name);
            PressKey(key);
            rightHand.DOMove(handTarget, 0.5f).OnComplete(() =>
            {
                rightHand.DOLocalMove(RightHandDefaultPos, 0.5f);
            });
        }
        else
        {
            var handTarget = leftSideKeysPoints[key.GetKeyNo()].position;
            pianoTeacher.Play(Animation1Name);
            PressKey(key);
            leftHand.DOMove(handTarget, 0.5f).OnComplete(() =>
            {
                leftHand.DOLocalMove(LeftHandDefaultPos, 0.5f);
            });
        }
    }
    private void HighlightKey()
    {
        if(_keyIndex > 0)
            pianoKeysToPress[_keyIndex - 1].HighlightKey(false);
        pianoKeysToPress[_keyIndex].HighlightKey(true);
        _allowKeyPressing = true;
    }
    private bool IsRightKey(PianoKey pressedKey, PianoKey requiredKey)
    {
        return pressedKey.GetPianoKeySide() == requiredKey.GetPianoKeySide() && pressedKey.GetKeyNo() == requiredKey.GetKeyNo();
    }
    private IEnumerator PlayPiano()
    {
        yield return _delay;
        while (this.enabled)
        {
            _keyIndex = 0;
            foreach (var t in _pressedPianoKeys)
            {
                MoveHandToPressKey(t);
                yield return _delay;
            }
        }
    }
    private void StudentsStartPlayingPiano()
    {
        foreach (var t in students)
        {
            t.PlayPiano();
        }
    }
    private void ChangeStudentsExpression()
    {
        foreach (var t in students)
        {
            t.ShowEmotion();
        }
    }
    private void EndActivity()
    {
        GamePlayManager.Instance.LevelComplete(5f);
    }
}