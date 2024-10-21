using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class Globe : MonoBehaviour
{
    public enum AnswerSelection
    {
        Random,
        ForcefullyRight,
        ForcefullyWrong
    }
    [SerializeField] private GlobeCountry[] globeCountries;
    [SerializeField] private string[] answers;
    [SerializeField] private string[] funnyAnswers;
    [SerializeField] private AnswerSelection[] answerSelection;
    [SerializeField] private Transform earthGlobe;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    [SerializeField] private Animator teacher;
    private int questionIndex = 0, answerIndex = 0, questionCounter = 0;
    private string answerString;
    private void HideAllFlags()
    {
        SharedUI.Instance.gamePlayUIManager.controls.EnableAnswerImage(false);
        foreach (var t in globeCountries)
        {
            t.countryFlag.SetActive(false);
        }
    }
    private void SetTargetFlag(int countryIndex)
    {
        globeCountries[countryIndex].countryFlag.GetComponent<MeshRenderer>().materials[1].mainTexture =
            globeCountries[countryIndex].countryFlagTexture;
        globeCountries[countryIndex].isAsked = true;
    }
    public void SelectCountryToAsk()
    {
        SharedUI.Instance.gamePlayUIManager.controls.SetStreakCounterText();
        HideAllFlags();
        questionIndex = Random.Range(0, globeCountries.Length);
        while (globeCountries[questionIndex].isAsked)
        {
            questionIndex = Random.Range(0, globeCountries.Length);
        }
        SetTargetFlag(questionIndex);
        var randomIndex0 = Random.Range(0, globeCountries.Length);
        while (randomIndex0 == questionIndex || globeCountries[randomIndex0].isAsked)
        {
            randomIndex0 = Random.Range(0, globeCountries.Length);
        }
        var randomIndex1 = Random.Range(0, globeCountries.Length);
        while (randomIndex1 == questionIndex || randomIndex1 == randomIndex0 || globeCountries[randomIndex0].isAsked)
        {
            randomIndex1 = Random.Range(0, globeCountries.Length);
        }
        teacher.Play($"TeacherGlobeRotation");
        var endValue0 = new Vector3(globeCountries[randomIndex1].countryRotation.x, 360f, globeCountries[randomIndex1].countryRotation.z);
        var endValue1 = new Vector3(globeCountries[randomIndex0].countryRotation.x, 360f, globeCountries[randomIndex0].countryRotation.z);
        earthGlobe.DORotate(endValue0, 1f).SetRelative(true).OnComplete(() =>
        {
            earthGlobe.DORotate(endValue1, 1f).SetRelative(true).OnComplete(() =>
            {
                earthGlobe.DORotate(globeCountries[questionIndex].countryRotation, 1f).OnComplete(() =>
                {
                    globeCountries[questionIndex].countryFlag.SetActive(true);
                    SelectAnswer();
                });
            });
        });
    }
    private void SelectAnswer()
    {
        switch (answerSelection[questionCounter])
        {
            case AnswerSelection.Random:
                answerIndex = Random.Range(0, answers.Length);
                answerString = answers[answerIndex];
                break;
            case AnswerSelection.ForcefullyRight:
                answerString = globeCountries[questionIndex].countryName;
                break;
            case AnswerSelection.ForcefullyWrong:
                answerIndex = Random.Range(0, funnyAnswers.Length);
                answerString = funnyAnswers[answerIndex];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        SharedUI.Instance.gamePlayUIManager.controls.EnableAnswerImage(true, answerString);
        questionCounter++;
        GamePlayManager.Instance.currentLevel.DeactivateInProgressFlag();
    }
    public bool IsRightAnswer()
    {
        //Debug.Log("IsRightAnswer");
        return globeCountries[questionIndex].countryName == answerString;
    }
    [Serializable]
    public class GlobeCountry
    {
        public string countryName;
        public Texture countryFlagTexture;
        public GameObject countryFlag;
        public Vector3 countryRotation;
        public bool isAsked = false;
    }
}