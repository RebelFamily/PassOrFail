using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    [SerializeField] private string buildingName;
    [SerializeField] private bool isFillingHorizontally = false;
    [SerializeField] private MeshRenderer meshRenderer;
    private Animator animator;
    [SerializeField] private float fillIncrement = 0.01f, maxFillPercent = 0.6f, minFillPercent = -0.6f;
    private float _fillPercent = 0f;
    private int _remainingCost = 0;
    [SerializeField] private int totalCost = 95;
    [SerializeField] private int cashDecValue = 2;
    private static readonly int FillRate = Shader.PropertyToID("_FillRate");
    private static readonly int IsHorizontal = Shader.PropertyToID("_IsHorizontal");
    private const string ANIMATION_NAME = "fillEffect";
    public UnityEvent onFillingComplete;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _fillPercent = PlayerPrefsHandler.GetBuildingFillerValue(buildingName, minFillPercent);
        _remainingCost = PlayerPrefsHandler.GetBuildingCostValue(buildingName, totalCost);
        totalCost = _remainingCost;
        meshRenderer.material.SetFloat(FillRate, _fillPercent);
        meshRenderer.material.SetInt(IsHorizontal, System.Convert.ToInt32(isFillingHorizontally));
        fillIncrement = (maxFillPercent - minFillPercent) / totalCost;
        //Debug.Log("fillIncrement: " + fillIncrement );
    }
    public void Fill()
    {
        if(!(animator.GetCurrentAnimatorStateInfo(0).IsName(ANIMATION_NAME) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
            animator.Play(ANIMATION_NAME);
        _fillPercent += fillIncrement;
        meshRenderer.material.SetFloat(FillRate, _fillPercent/* * maxFillPercent*/);
        _remainingCost -= cashDecValue;
        PlayerPrefsHandler.SetBuildingFillerValue(buildingName, _fillPercent);
        PlayerPrefsHandler.SetBuildingCostValue(buildingName, _remainingCost);
        //CurrencyCounter.Instance.CurrencyDeduction(cashDecValue);
        PlayerPrefsHandler.currency -= cashDecValue;
        SoundController.Instance.PlayFillingSound();
        //Debug.Log("_fillPercent: " + _fillPercent + " cash: " + PlayerPrefsHandler.currency);
    }
    public string GetBuildingName()
    {
        return buildingName;
    }
    public float GetMaxFillValue()
    {
        return maxFillPercent;
    }
    public float GetMinFillValue()
    {
        return minFillPercent;
    }
    public float GetCurrentFillValue()
    {
        return _fillPercent;
    }
    public int GetRemainingCost()
    {
        return _remainingCost;
    }
    public int GetTotalCost()
    {
        return totalCost;
    }
}