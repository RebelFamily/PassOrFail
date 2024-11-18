using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BookSorting : MonoBehaviour, IMiniGame
{
    [SerializeField] private int noOfStacksToSort = 2;
    [SerializeField] private GameObject perfects;
    [SerializeField] private GameObject tapTutorial0, tapTutorial1;
    public static BookSorting Instance;
    private int _completedStackCount = 0;
    private bool _isSorted = false, _isTutorialFinished = false;
    public enum BookColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private List<Stack> stacks = new List<Stack>();
    public void RegisterStack(Stack stack)
    {
        if (!stacks.Contains(stack))
        {
            stacks.Add(stack);
        }
    }
    public void EnableAllStackColliders(bool enable)
    {
        foreach (var stack in stacks.Where(stack => stack != null))
        {
            stack.GetComponent<BoxCollider>().enabled = enable; // Set the collider's enabled state
        }
        SetTutorialNextStep();
    }
    public void CheckIfActivityFinished()
    {
        foreach (var stack in stacks)
        {
            if (stack != null)
            {
                if(stack.IsStackRegisteredForCompletion()) 
                    continue;
                if (stack.IsStackSorted())
                {
                    stack.RegisterStackAsSorted();
                    perfects.SetActive(true);
                    _completedStackCount++;
                    if (_completedStackCount == noOfStacksToSort)
                    {
                        //Debug.Log("Sorting Completed");
                        _isSorted = true;
                        EndMiniGame();
                    }
                }
            }
        }
    }
    public bool AreBooksSorted()
    {
        return _isSorted;
    }
    public void SetTutorialNextStep()
    {
        Debug.Log("SetTutorialNextStep");
        if(_isTutorialFinished) return;
        tapTutorial0.SetActive(false);
        tapTutorial1.SetActive(true);
    }
    public void EndTutorial()
    {
        if(_isTutorialFinished) return;
        _isTutorialFinished = true;
        tapTutorial0.SetActive(false);
        tapTutorial1.SetActive(false);
        tapTutorial0.transform.parent.gameObject.SetActive(false);
    }
    public void StartMiniGame()
    {
        
    }
    public void EndMiniGame()
    {
        GamePlayManager.Instance.LevelComplete(1f);
    }
}