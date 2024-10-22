using UnityEngine;
public class RateUsInvoker : MonoBehaviour
{
	[SerializeField] private GameObject rateUsPanel;
	private void OnEnable()
    {
	    if (PlayerPrefsHandler.GetBool(PlayerPrefsHandler.RateUsString)) return;
	    if (PlayerPrefsHandler.LevelCounter == PlayerPrefsHandler.LevelNoForRating)
	    {
			rateUsPanel.SetActive(true);
	    }
    }
}