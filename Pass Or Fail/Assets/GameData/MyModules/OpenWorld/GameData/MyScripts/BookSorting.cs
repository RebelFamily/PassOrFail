using UnityEngine;
using System.Collections.Generic;
public class BookSorting : MonoBehaviour, IMiniGame
{
    [SerializeField] private int noOfStacksToSort = 2;
    [SerializeField] private GameObject perfects;
    public static BookSorting Instance;
    private int _completedStackCount = 0;
    private bool _isSorted = false;
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
        foreach (var stack in stacks)
        {
            if (stack != null)
            {
                stack.GetComponent<BoxCollider>().enabled = enable; // Set the collider's enabled state
            }
        }
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
    public void StartMiniGame()
    {
        
    }
    public void EndMiniGame()
    {
        GamePlayManager.Instance.LevelComplete(1f);
    }
}