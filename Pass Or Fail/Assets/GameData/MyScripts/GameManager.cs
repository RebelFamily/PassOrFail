using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isTesting = true;
    [SerializeField, Range(0, PlayerPrefsHandler.TotalLevels - 1)] private int levelNo;
    public bool ActivityFlag { get; set; } = false;
    public bool MiniGameFlag { get; set; } = false;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        if(isTesting)
            PlayerPrefsHandler.CurrentLevelNo = levelNo;
    }
    public bool IsTesting()
    {
        return isTesting;
    }
}