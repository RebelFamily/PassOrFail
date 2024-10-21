using UnityEngine;
public class RateUsInvoker : MonoBehaviour
{
	[SerializeField] private GameObject rateUsPanel;
	private void OnEnable()
    {
	    if (PlayerPrefsHandler.GetBool("RateUs")) return;
	    if (PlayerPrefsHandler.LevelCounter == PlayerPrefsHandler.LevelNoForRating)
	    {
			rateUsPanel.SetActive(true);
	    }
    }
}