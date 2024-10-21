using UnityEngine;
public class SaveTheEgg : MonoBehaviour
{
    //public static readonly string[] EggNames = new string[] {"SimpleEgg", "FriedEgg", "EggWithBandage", "BrokenEgg", "EasterEgg1", "EasterEgg2", "EasterEgg3", "EasterEgg4", "DinoEgg"};
    [SerializeField] private EggsInventory eggsInventory;
    [SerializeField] private string breakableObjectName = "Baby Egg";
    [SerializeField] private string[] eggTaskNames;
    private int _questionIndex = 0, _questionCounter = 0;
    [SerializeField] private Student[] students;
    private readonly int[] _eggIndexesToAsk = new int[3];
    public void InvokeSetEggs()
    {
        Invoke(nameof(SetEggs), 0.1f);
    }
    private void SetEggs()
    {
        for (var i = 0; i < students.Length; i++)
        {
            var eggIndex = 0;
            if (PlayerPrefsHandler.LevelCounter > PlayerPrefsHandler.TotalLevels - 1)
            {
                eggIndex = Random.Range(0, eggsInventory.eggs.Length);
            }
            else
            {
                eggIndex = GetEggIndex(eggTaskNames[i]);
            }
            _eggIndexesToAsk[i] = eggIndex;
            var pos = students[i].GetGender() == Gender.FemaleStudent ? eggsInventory.eggs[eggIndex].positionForFemale : 
                eggsInventory.eggs[eggIndex].positionForMale;
            var egg = Instantiate(eggsInventory.eggs[eggIndex].eggObject, students[i].GetAnimator().GetBoneTransform(HumanBodyBones.RightHand));
            egg.transform.localPosition = pos;
        }
    }
    private int GetEggIndex(string eggName)
    {
        for (var i = 0; i < eggsInventory.eggs.Length; i++)
        {
            if (eggName == eggsInventory.eggs[i].eggName)
                return i;
        }
        return Random.Range(0, eggsInventory.eggs.Length);;
    }
    public bool IsRightAnswer()
    {
        //Debug.Log("IsRightAnswer");
        var flag = eggsInventory.eggs[_eggIndexesToAsk[_questionIndex]].isEggAlright;
        _questionIndex++;
        return flag;
    }
    public string GetBreakableObjectName()
    {
        return breakableObjectName;
    }
}