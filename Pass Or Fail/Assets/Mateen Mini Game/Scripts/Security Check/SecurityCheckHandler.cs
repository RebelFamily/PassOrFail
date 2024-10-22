using UnityEngine;

[System.Serializable]
public class BagData
{
    public SecurityCheckHandler.PropType propType;
    public Sprite propSprite;
    public Vector3 propSmallScale;
    public Vector3 propFullScale;
}
public class SecurityCheckHandler : MonoBehaviour
{
    [SerializeField] private StudentsHandler studentsHandler;
    [SerializeField] private BagData[] bags;
    [SerializeField] private GameObject canvas;
    private int _bagIndex = 0;

    /*private void Start()
    {
        SetupUniforms();
    }*/

    public void StartActivity()
    {
        canvas.SetActive(true);
    }

    private void EndActivity()
    {
        canvas.SetActive(false);
        //GamePlayManager.Instance.LevelComplete(1f);
    }

    /*private void SetupUniforms()
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
    }*/

    public void OnPass()
    {
        canvas.SetActive(false);
        studentsHandler.ExitStudent(Expressions.ExpressionType.Happy);
        if (bags[_bagIndex].propType == PropType.Allowed)
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
        if (bags[_bagIndex].propType == PropType.Allowed)
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
        _bagIndex++;
        canvas.SetActive(true);
        if (_bagIndex >= 3)
            EndActivity();
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