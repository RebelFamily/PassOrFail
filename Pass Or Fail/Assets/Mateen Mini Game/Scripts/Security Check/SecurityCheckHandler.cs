using System;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    [System.Serializable]
    public class BagData
    {
        public SecurityCheckHandler.PropType propType;
        public Sprite propSprite;
        public Vector3 propSmallScale;
        public Vector3 propFullScale;
        public Vector3 smallScalePosition;
        public Vector3 largeScalePosition;
    }

    public class SecurityCheckHandler : MonoBehaviour
    {
        [SerializeField] private MiniGameStudentHandler studentsHandler;
        [SerializeField] private StudentBag[] bags;
        [SerializeField] private GameObject canvas;
        private int _bagIndex = 0;

        private void OnEnable()
        {
            EventManager.OnStudentReachedDestination += EnableCanvas;
            EventManager.OnStudentChecked += DisableCanvas;
        }

        private void OnDisable()
        {
            EventManager.OnStudentReachedDestination += EnableCanvas;
            EventManager.OnStudentChecked += DisableCanvas;
        }

        private void EnableCanvas(Transform student)
        {
            if(_bagIndex >= bags.Length) return;
            if(student != bags[_bagIndex].transform) return;
            Debug.Log("BagIndex : "+_bagIndex+" lenght: "+bags.Length +" :: " +student + " :: "+bags[_bagIndex].transform);
            Invoke(nameof(ActivateCanvas),1.3f);
        }

        private void ActivateCanvas()
        {
            canvas.SetActive(true);
        }

        private void DisableCanvas()
        {
            _bagIndex++;
            canvas.SetActive(false);
            CheckGameComplete();
        }

        public void AllowToPass()
        {
            canvas.SetActive(false);
            studentsHandler.ExitStudent(Expressions.ExpressionType.Happy,isAllowed: true);
            if (bags[_bagIndex].bagData.propType == PropType.Allowed)
            {
                /*SoundController.Instance.PlayCorrectGradingSound();
                SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
                Invoke(nameof(ShowGoodEffect), 0.5f);*/
            }
            else
            {
                /*SoundController.Instance.PlayWrongGradingSound();
                SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
                Invoke(nameof(ShowBadEffect), 0.5f);*/
            }

            EventManager.InvokeStudentChecked();
        }

        public void DontAllowToPass()
        {
            studentsHandler.ExitStudent(Expressions.ExpressionType.Sad,isAllowed: false);
            if (bags[_bagIndex].bagData.propType == PropType.Allowed)
            {
                /*SoundController.Instance.PlayWrongGradingSound();
                SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
                Invoke(nameof(ShowBadEffect), 0.5f);*/
            }
            else
            {
                /*SoundController.Instance.PlayCorrectGradingSound();
                SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
                Invoke(nameof(ShowGoodEffect), 0.5f);*/
            }
            EventManager.InvokeStudentChecked();
        }

        private void CheckGameComplete()
        {
            if(_bagIndex >= 3)
                Debug.Log("Game Complete");
        }
        private void ShowGoodEffect()
        {
            SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
        }

        private void ShowBadEffect()
        {
            SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
        }

        public enum PropType
        {
            None,
            Allowed,
            NotAllowed
        }
    }
}