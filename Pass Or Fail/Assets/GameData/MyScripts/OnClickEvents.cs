using UnityEngine;
using UnityEngine.EventSystems;
public class OnClickEvents : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
 {
	public string buttonName;
	public void OnPointerUp(PointerEventData eventData)
	{
		if(SoundController.Instance)
			SoundController.Instance.PlayBtnClickSound();
		switch (buttonName)
		{
			case PlayerPrefsHandler.Play:
				SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.GamePlay);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
				break;
			case PlayerPrefsHandler.Teachers:
				var customization = SharedUI.Instance.metaUIManager.GetMenu(PlayerPrefsHandler.CharactersCustomization).GetComponent<Customization>();
				customization.SelectCategory(buttonName);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.CharactersCustomization);
				break;
			case PlayerPrefsHandler.Students:
				var studentsCustomization = SharedUI.Instance.metaUIManager.GetMenu(PlayerPrefsHandler.CharactersCustomization).GetComponent<Customization>();
				studentsCustomization.SelectCategory(buttonName);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.CharactersCustomization);
				break;
			case PlayerPrefsHandler.ClassRoom:
				var classRoomCustomization = SharedUI.Instance.metaUIManager.GetMenu(PlayerPrefsHandler.CharactersCustomization).GetComponent<Customization>();
				classRoomCustomization.SelectCategory(buttonName);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.CharactersCustomization);
				break;
			case PlayerPrefsHandler.NextComplete:
				GamePlayManager.Instance.SendProgressionEvent();
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
				break;
			case PlayerPrefsHandler.Home:
				GamePlayManager.Instance.SendProgressionEvent();
				SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.Meta);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
				break;
			case PlayerPrefsHandler.Replay:
				SharedUI.Instance.SetNextSceneIndex(PlayerPrefsHandler.GamePlay);
				SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.Loading);
				break;
			case PlayerPrefsHandler.Settings:
				SharedUI.Instance.SubMenu(PlayerPrefsHandler.Settings);
				break;
			case PlayerPrefsHandler.SettingsClose:
				SharedUI.Instance.CloseSubMenu();
				break;
			case PlayerPrefsHandler.Coins250:
				Callbacks.rewardType = Callbacks.RewardType.Coins250;
				AdsCaller.Instance.ShowRewardedAd();
				break;
			case PlayerPrefsHandler.RewardStreak:
				Callbacks.rewardType = Callbacks.RewardType.RewardStreak;
				AdsCaller.Instance.ShowRewardedAd();
				break;
			case PlayerPrefsHandler.HideSubMenu:
				SharedUI.Instance.CloseSubMenu();
				break;
			case PlayerPrefsHandler.AdToCorrectMistake:
				if (AdsCaller.Instance.IsRewardedAdAvailable())
				{
					Callbacks.rewardType = Callbacks.RewardType.MistakeCorrection;
					AdsCaller.Instance.ShowRewardedAd();
				}
				else
				{
					GamePlayManager.Instance.currentLevel.CorrectMistake();
				}
				break;
			case PlayerPrefsHandler.PayToCorrectMistake:
				if(PlayerPrefsHandler.currency < 30) return;
				CurrencyCounter.Instance.DeductCurrency(30);
				GamePlayManager.Instance.currentLevel.CorrectMistake();
				break;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		
	}
 }