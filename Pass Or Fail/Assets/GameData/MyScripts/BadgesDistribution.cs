using DG.Tweening;
using UnityEngine;
public class BadgesDistribution : MonoBehaviour
{
    [SerializeField] private ReportCardUI reportCardUI;
    [SerializeField] private CorridorActivity.ReportCard[] reportCardEntries;
    [SerializeField] private StudentsHandler studentsHandler;
    private int _studentIndex = 0;
    private readonly int[] _reportCardEntriesIndexes = {-1, -1, -1};
    [SerializeField] private Transform[] badges;
    [SerializeField] private GameObject canvas;
    private readonly Vector3 _badgePosition = new (-0.00038f, 0.000614f, 0.00118f);
    private void Start()
    {
        SetupReportCardEntriesIndexes();
    }
    public void StartActivity()
    {
        SetReportUI(_studentIndex);
        canvas.SetActive(true);
    }

    private void EndActivity()
    {
        canvas.SetActive(false);
        //GamePlayManager.Instance.LevelComplete(1f);
    }
    private void SetReportUI(int index)
    {
        reportCardUI.SetReportCardUI(reportCardEntries[index]);
    }
    private void SetupReportCardEntriesIndexes()
    {
        var totalEntries = _reportCardEntriesIndexes.Length;
        for (var i = 0; i < totalEntries; i++)
        {
            var r = Random.Range(0, totalEntries);
            while (IsAlreadyAssigned(r))
            {
                r = Random.Range(0, totalEntries);
            }
            _reportCardEntriesIndexes[i] = r;
        }
    }
    private bool IsAlreadyAssigned(int newValue)
    {
        var totalEntries = _reportCardEntriesIndexes.Length;
        for (var i = 0; i < totalEntries; i++)
        {
            if (_reportCardEntriesIndexes[i] == newValue)
                return true;
        }
        return false;
    }
    public void AssignBadge(int badgeIndex)
    {
        reportCardUI.DisableButton(badgeIndex);
        badges[badgeIndex].parent = studentsHandler.GetStudents()[0].GetAnimator()
            .GetBoneTransform(HumanBodyBones.UpperChest);
        badges[badgeIndex].DOLocalMove(_badgePosition, 0.5f);
        canvas.SetActive(false);
        studentsHandler.ExitStudent(Expressions.ExpressionType.Happy);
        SoundController.Instance.PlayCorrectGradingSound();
        SharedUI.Instance.gamePlayUIManager.controls.ShowPerfects(PlayerPrefsHandler.Perfects);
        Invoke(nameof(ShowGoodEffect), 0.5f);
        Invoke(nameof(IsActivityEnds), 2f);
    }
    private void IsActivityEnds()
    {
        _studentIndex++;
        canvas.SetActive(true);
        if(_studentIndex >= 3)
            EndActivity();
        else
            SetReportUI(_studentIndex);
    }
    private void ShowGoodEffect()
    {
        SharedUI.Instance.gamePlayUIManager.controls.ShowBlinkAlert(PlayerPrefsHandler.Good);
    }
}
