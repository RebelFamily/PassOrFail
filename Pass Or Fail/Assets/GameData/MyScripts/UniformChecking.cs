using System;
using UnityEngine;
public class UniformChecking : MonoBehaviour
{
    [SerializeField] private StudentsHandler studentsHandler;
    [SerializeField] private Uniform[] uniforms;
    [SerializeField] private GameObject canvas;
    private int _uniformIndex = 0;

    private void Start()
    {
        SetupUniforms();
    }
    public void StartActivity()
    {
        canvas.SetActive(true);
    }
    private void EndActivity()
    {
        canvas.SetActive(false);
        //GamePlayManager.Instance.LevelComplete(1f);
    }
    private void SetupUniforms()
    {
        var students = studentsHandler.GetStudents();
        var totalUniforms = uniforms.Length;
        foreach (var t in students)
        {
            var r = UnityEngine.Random.Range(0, totalUniforms);
            while (uniforms[r].isAssigned)
            {
                r = UnityEngine.Random.Range(0, totalUniforms);
            }
            if (uniforms[r].IsCleaned())
            {
                uniforms[r].isAssigned = true;
                continue;
            }
            uniforms[r].isAssigned = true;
            var dirtPoint = t.GetAnimator().GetBoneTransform(uniforms[r].boneToPlaceDirt);
            Instantiate(uniforms[r].dustProp, dirtPoint);
        }
    }
    public void OnPass()
    {
        canvas.SetActive(false);
        studentsHandler.ExitStudent(Expressions.ExpressionType.Happy);
        if (uniforms[_uniformIndex].IsCleaned())
        {
            SoundController.Instance.PlayCorrectGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
            Invoke(nameof(ShowGoodEffect), 0.5f);
        }
        else
        {
            SoundController.Instance.PlayWrongGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
            Invoke(nameof(ShowBadEffect), 0.5f);
        }
        Invoke(nameof(IsActivityEnds), 2f);
    }
    public void OnFail()
    {
        canvas.SetActive(false);
        studentsHandler.ExitStudent(Expressions.ExpressionType.Sad);
        if (uniforms[_uniformIndex].IsCleaned())
        {
            SoundController.Instance.PlayWrongGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Warnings);
            Invoke(nameof(ShowBadEffect), 0.5f);
        }
        else
        {
            SoundController.Instance.PlayCorrectGradingSound();
            SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
            Invoke(nameof(ShowGoodEffect), 0.5f);
        }
        Invoke(nameof(IsActivityEnds), 2f);
    }
    private void IsActivityEnds()
    {
        _uniformIndex++;
        Invoke(nameof(EnableCanvas), 0.5f);
        if(_uniformIndex >= 3)
            Invoke(nameof(EndActivity), 0.5f);
    }
    private void EnableCanvas()
    {
        canvas.SetActive(true);
    }
    private void ShowGoodEffect()
    {
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
    }
    private void ShowBadEffect()
    {
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Bad);
    }
    [Serializable] public class Uniform
    {
        public UniformType uniformType;
        public GameObject dustProp;
        public HumanBodyBones boneToPlaceDirt;
        public bool isAssigned = false;
        public bool IsCleaned()
        {
            return uniformType == UniformType.Cleaned;
        }
    }
    public enum UniformType
    {
        Cleaned,
        Dirty
    }
}