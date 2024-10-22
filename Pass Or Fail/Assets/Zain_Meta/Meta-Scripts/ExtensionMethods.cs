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
                textMeshProUGUI.text = "$" + newCash + "M";
            }

            else if (cashPrice >= 1000)
            {
                var newCash = cashPrice / 1000f;
                newCash = (float)Math.Round(newCash, 2);
                textMeshProUGUI.text = "$" + newCash + "k";
            }
            else
                textMeshProUGUI.text = "$" + cashPrice;
        }

        public static void SetTextOfFiller(this int cashPrice, int total, Text textMeshProUGUI)
        {
            var filledAmount = total - cashPrice;
            textMeshProUGUI.text = filledAmount + "/" + total;
        }

        public static void ConvertToPercentage(this int value, int total, Text textMeshProUGUI)
        {
            var filledAmount = total - value;
            var percent = (filledAmount * 1f / total) * 100f;
            textMeshProUGUI.text = Mathf.Floor(percent) + "%";
        }

        public static void CountDownTimer(this float timeToDisplay, Text timeText)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }

        public static void CountDownTimer(this float timeToDisplay, Text timeText, string extraText)
        {
            timeText.text = extraText + '\n';
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text += $"{minutes:00}:{seconds:00}";
        }
    }
}