using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts
{
    public static class ExtensionMethods
    {
        public static void HideCanvas(this CanvasGroup canvas)
        {
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }

        public static void SmoothHideCanvas(this CanvasGroup canvas)
        {
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
            DOVirtual.Float(canvas.alpha, 0, .25f, (alphaVal => canvas.alpha = alphaVal));
        }

        public static void SmoothShowCanvas(this CanvasGroup canvas)
        {
            DOTween.Kill(canvas);
            canvas.alpha = 0;

            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            DOVirtual.Float(canvas.alpha, 1, .25f, (alphaVal => canvas.alpha = alphaVal));
        }

        public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            return result;
        }

        public static void ShowCanvasWithDelay(this CanvasGroup canvasGroup)
        {
            DOTween.Kill(canvasGroup);
            canvasGroup.alpha = 0;

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            DOVirtual.DelayedCall(1f,
                () => { DOVirtual.Float(canvasGroup.alpha, 1, .25f, (alphaVal => canvasGroup.alpha = alphaVal)); });
        }


        public static void ParabolicMovement(this Transform throwingObject, Transform target, float duration = 1f,
            float arcHeight = 1f,Ease ease=Ease.Linear,Action onComplete = null)
        {
            Vector3 startPos = throwingObject.position;
            var targetPosition = target.position;
            Vector3 midPos = (startPos + targetPosition) * 0.5f;
            midPos += Vector3.up * arcHeight;

            // Create a bezier path with start, middle, and end positions
            Vector3[] path = new Vector3[] { startPos, midPos, targetPosition };

            // Use DOPath to move the object along the bezier path
            throwingObject.DOPath(path, duration, PathType.CatmullRom)
                .SetEase(ease)
                .OnUpdate(() =>
                {
                    // Calculate the direction to the next point on the path
                    var nextPosition =
                        Vector3.Lerp(path[0], path[1], throwingObject.position.z); // Adjust for the proper next point
                    var direction = (nextPosition - throwingObject.position).normalized;

                    // Calculate target rotation
                    var targetRotation = Quaternion.LookRotation(direction);
                    throwingObject.rotation =
                        Quaternion.RotateTowards(throwingObject.rotation, targetRotation, 720 * Time.deltaTime);
                })
                .OnComplete(() => 
                    onComplete?.Invoke()
                    );
        }

        public static void ShowCanvas(this CanvasGroup canvas)
        {
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }

        public static void ScaleUI(this RectTransform rectTransform, bool toScale, float scaleDelay)
        {
            DOTween.Kill(rectTransform);
            if (toScale)
                rectTransform.DOScale(1, scaleDelay).SetEase(Ease.Linear);
            else
                rectTransform.DOScale(0, scaleDelay).SetEase(Ease.Linear);
        }

        public static void ScaleUIWithEase(this RectTransform rectTransform, bool toScale, float scaleDelay, Ease ease)
        {
            DOTween.Kill(rectTransform);
            if (toScale)
                rectTransform.DOScaleX(1, scaleDelay).SetEase(ease);
            else
                rectTransform.DOScaleX(0, scaleDelay).SetEase(ease);
        }


        public static void SetFloatingPoint(this int cashPrice, Text textMeshProUGUI)
        {
            if (cashPrice >= 1000000)
            {
                var newCash = cashPrice / 1000000f;
                newCash = (float)Math.Round(newCash, 2);
                textMeshProUGUI.text =newCash + "M";
            }

            else if (cashPrice >= 1000)
            {
                var newCash = cashPrice / 1000f;
                newCash = (float)Math.Round(newCash, 2);
                textMeshProUGUI.text =newCash + "k";
            }
            else
                textMeshProUGUI.text = cashPrice.ToString();
        }
        public static void SetFloatingPoint(this int cashPrice, Text textMeshProUGUI,string extraMsg)
        {
            if (cashPrice >= 1000000)
            {
                var newCash = cashPrice / 1000000f;
                newCash = (float)Math.Round(newCash, 2);
                textMeshProUGUI.text =newCash + "M";
            }

            else if (cashPrice >= 1000)
            {
                var newCash = cashPrice / 1000f;
                newCash = (float)Math.Round(newCash, 2);
                textMeshProUGUI.text =newCash + "k";
            }
            else
                textMeshProUGUI.text = cashPrice.ToString();

            textMeshProUGUI.text += extraMsg;
        }
        public static void CountDownTimer(this float timeToDisplay, Text timeText)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}