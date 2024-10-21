using System.Collections;
using AssetKits.ParticleImage;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private int cashReward = 500;
    private int _coinsReward = 0;
    [SerializeField] private Text cashText;
    [SerializeField] private ParticleImage cashEffect;
    private Vector3 _cashEffectDefaultPos;
    private const string Path = "CurrencyBar/Target";
    public static CurrencyCounter Instance;
    private void Start()
    {
        Instance = this;
        _cashEffectDefaultPos = cashEffect.transform.position;
    }
    private void OnEnable()
    {
        UpdateCoinsText();
    }
    public void UpdateCoinsText()
    {
        cashText.text = PlayerPrefsHandler.currency.ToString();
    }
    public void UpdateCurrency(int amount)
    {
        PlayerPrefsHandler.currency += amount;
        if (PlayerPrefsHandler.currency < 0)
        {
            PlayerPrefsHandler.currency = 0;
        }
        UpdateCoinsText();
    }
    public void DeductCurrency(int amount)
    {
        PlayerPrefsHandler.currency -= amount;
        if (PlayerPrefsHandler.currency < 0)
        {
            PlayerPrefsHandler.currency = 0;
        }
        UpdateCoinsText();
    }
    public void SetCurrency(int amount)
    {
        cashEffect.transform.position = _cashEffectDefaultPos;
        cashEffect.attractorTarget = transform.Find(Path);
        cashEffect.Play();
        var previousValue = PlayerPrefsHandler.currency;
        PlayerPrefsHandler.currency += amount;
        //StartCoroutine(CountUpToTarget(previousValue, PlayerPrefsHandler.currency, 5f));
        StartCoroutine(CountUpToTarget(1.5f));
    }
    public void SetCurrency(int amount, Transform newPosition)
    {
        cashEffect.transform.position = newPosition.position;
        cashEffect.attractorTarget = transform.Find(Path);
        cashEffect.Play();
        var previousValue = PlayerPrefsHandler.currency;
        PlayerPrefsHandler.currency += amount;
        //StartCoroutine(CountUpToTarget(previousValue, PlayerPrefsHandler.currency, 5f));
        StartCoroutine(CountUpToTarget(1.5f));
    }
    public void DeductCurrency(int amount, Transform newTarget)
    {
        cashEffect.transform.position = transform.Find(Path).position;
        cashEffect.attractorTarget = newTarget;
        cashEffect.Play();
        PlayerPrefsHandler.currency -= amount;
        StartCoroutine(CountUpToTarget(1.5f));
    }
    private IEnumerator CountUpToTarget(int previousVal, int targetVal, float duration)
    {
        yield return new WaitForSeconds(1.5f);
        var current = previousVal;
        while (current < targetVal)
        {
            current += (int)(targetVal / (duration/Time.deltaTime));
            current = Mathf.Clamp(current, 0, targetVal);
            cashText.text = current.ToString();
            yield return null;
        }
        UpdateCoinsText();
        SoundController.Instance.PlayBuySound();
        //Invoke($"Continue", 0.5f);
    }
    private IEnumerator CountUpToTarget(float duration)
    {
        yield return new WaitForSeconds(duration);
        SoundController.Instance.PlayBuySound();
        UpdateCoinsText();
    }
    public void UpdateCurrency(int amount , float delay)
    {
        PlayerPrefsHandler.currency += amount;
        if (PlayerPrefsHandler.currency < 0)
        {
            PlayerPrefsHandler.currency = 0;
        }
    }
    public void CurrencyDeduction(int coins)
    {
        CurrencyRegisterSound();
        PlayerPrefsHandler.currency -= coins;
        UpdateCoinsText();
    }
    private static void CurrencyRegisterSound()
    {
        if(SoundController.Instance)
            SoundController.Instance.PlayBuySound();
    }
    public int GetCoinsReward()
    {
        return _coinsReward;
    }
    public void SetCoinsReward(int coinsValue)
    {
        _coinsReward += coinsValue;
    }
    public int GetCashReward()
    {
        return cashReward;
    }
    public void SetCashReward(int cashValue)
    {
        cashReward += cashValue;
    }
    public void Continue()
    {
        //AdsCaller.Instance.ShowTimerAd();
        //AdsCaller.Instance.DestroyRectBanner();
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
    }
    public void SetCashEffectStartingPosition(Vector3 newPos)
    {
        //Debug.Log("newPos: " + newPos);
        cashEffect.transform.position = newPos;
    }
    public void SetCashEffectTarget(Transform newTarget)
    {
        cashEffect.attractorTarget = newTarget;
    }
    public void ShowCashImage(bool flag)
    {
        transform.Find("CurrencyBar").GetComponent<Image>().enabled = flag;
        transform.Find("CurrencyBar/CashText").gameObject.SetActive(flag);
    }
}