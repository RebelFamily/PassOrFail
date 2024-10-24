using UnityEngine;
public class MiniGame : MonoBehaviour
{
    [SerializeField] private EnvironmentManager.Environment environment;
    [SerializeField] private MiniGameNames miniGameName;
    [SerializeField] private GameObject miniGame;
    private IMiniGame _iMiniGame;
    private IMiniGameInput _miniGameInput;
    public void StartMiniGame()
    {
        _iMiniGame = miniGame.GetComponent<IMiniGame>();
        _miniGameInput = miniGame.GetComponent<IMiniGameInput>();
        _iMiniGame.StartMiniGame();
        SharedUI.Instance.HideAll();
    }
    public EnvironmentManager.Environment GetEnvironment()
    {
        return environment;
    }
    private void EndMiniGame()
    {
        _iMiniGame.EndMiniGame();
    }
    public void MiniGameMouseDown()
    {
        _miniGameInput.MiniGameMouseDown();
    }
    public void MiniGameMouseUp()
    {
        _miniGameInput.MiniGameMouseUp();
    }
    public enum MiniGameNames
    {
        PencilSharpener = 0,
        BoardCleaning = 1,
        PinBoardSorting = 2,
        GeometrySorting = 3,
        BooksSorting = 4,
        BookStickers = 5,
        PenFilling = 6
    }
}
public interface IMiniGame
{
    void StartMiniGame ();
    void EndMiniGame();
}
public interface IMiniGameInput
{
    void MiniGameMouseDown ();
    void MiniGameMouseUp();
}