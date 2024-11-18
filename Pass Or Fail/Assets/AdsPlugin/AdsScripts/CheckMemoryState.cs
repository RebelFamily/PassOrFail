using UnityEngine;
public class CheckMemoryState : MonoBehaviour
{
    public static CheckMemoryState Instance;
    [SerializeField] private MemoryAdvisor memoryAdvisor;
    private MemoryAdvisor.MemoryState _memorystate;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
    }
   public void CheckMemory() {
        //_memorystate = memoryAdvisor.GetMemoryState(true);
    }
   public bool IsEnoughMemory()
   {
       return true;
       //return _memorystate != MemoryAdvisor.MemoryState.Critical;
   }
}