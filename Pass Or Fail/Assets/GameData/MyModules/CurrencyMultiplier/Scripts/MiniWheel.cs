using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MiniWheel : MonoBehaviour
{
    [SerializeField] private LevelCompleteScript levelCompleteScript;
    [SerializeField] private Text rewardTextMiniGame,freeRewardText;
    [SerializeField] private Animator nobAnimator;
    private int reward;
    private int rewardMultiplier;
    public Button collectButton, rewardedAdBtn;
    public UnityEvent EndSpin;
    public UnityEvent CollectButtonOnClick;
    private bool isAlreadyCommandGiven;
    private bool isFreeReward;
    private void Start()
    {
        collectButton.onClick.AddListener(CollectButtonClick);
        rewardedAdBtn.onClick.AddListener(CollectButtonClick);
        isFreeReward = PlayerPrefsHandler.IsFreeSpinAvailable;
        if (isFreeReward)
        {
            collectButton.gameObject.SetActive(true);
            rewardedAdBtn.gameObject.SetActive(false);
        }
        else
        {
            rewardedAdBtn.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Callbacks.OnRewardCashMultiplier += MiniGameReward;
        collectButton.interactable = true;
    }
    private void OnDisable()
    {
        Callbacks.OnRewardCashMultiplier -= MiniGameReward;
    }
    private void CollectButtonClick()
    {
        if(isAlreadyCommandGiven) return;
        PlayButtonClickSound();
        CollectButtonOnClick.Invoke();
        OnTap();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<RewardValue>())
            return;

        rewardMultiplier = other.GetComponent<RewardValue>().multiplierValue;
        if (isFreeReward)
        {
            freeRewardText.text = "+ " + (GamePlayManager.Instance.LevelEndRewardValue * rewardMultiplier);
        }
        else
        {
            rewardTextMiniGame.text = "+ " + (GamePlayManager.Instance.LevelEndRewardValue * rewardMultiplier);
        }
    }
    private void OnTap()
    {
        if (isFreeReward)
        {
            PlayerPrefsHandler.IsFreeSpinAvailable = false;
            OnStopReward();
            MiniGameReward();
        }
        else
        {
            Callbacks.rewardType = Callbacks.RewardType.RewardCashMultiplier;
            if (!CheckAdAvailable())
            {
                return;
            }
            OnStopReward();
            AdsCaller.Instance.ShowRewardedAd();
        }
    }
    private bool CheckAdAvailable()
    {
        return AdsCaller.Instance.IsRewardedAdAvailable();
    }
    private void PlayButtonClickSound()
    {
        SoundController.Instance.PlayBtnClickSound();
    }
    private void OnStopReward()
    {
        EndSpin.Invoke();
        Destroy(nobAnimator);
    }
    private void MiniGameReward()
    {
        isAlreadyCommandGiven = true;
        if (isFreeReward)
        {
            rewardedAdBtn.interactable = false;
        }
        else
        {
            collectButton.interactable = false;
        }
        reward = GamePlayManager.Instance.LevelEndRewardValue * rewardMultiplier;
        CurrencyCounter.Instance.SetCurrency(reward);
        Invoke(nameof(Next),1f);
    }
    private void Next()
    {
        levelCompleteScript.NextStep();
    }
}