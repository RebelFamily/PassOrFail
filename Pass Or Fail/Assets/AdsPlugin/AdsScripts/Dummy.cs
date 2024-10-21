using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
public class Dummy : MonoBehaviour
{
    [SerializeField] private InputField inputFieldX, inputFieldY;
    [SerializeField] private float yOffsetFromCenter;

    public void CallRectBanner()
    {
        CalculateValues();
        /*var x = int.Parse(inputFieldX.text);
        var y = int.Parse(inputFieldY.text);
        AdmobManager.Instance.RequestRectBanner(x, y);*/
    }
    private void CalculateValues()
    {
        // Calculate the screen width and height in dp
        float screenWidthInDp = Screen.width / Screen.dpi * 160;
        float screenHeightInDp = Screen.height / Screen.dpi * 160;

        // Calculate the center position in dp
        float centerXInDp = screenWidthInDp / 2;
        float centerYInDp = screenHeightInDp / 2;

        // Calculate the Y position with an offset from the center
        //float yPositionInDp = centerYInDp + yOffsetFromCenter;

        // Create an anchor for the banner ad
        float anchorX = centerXInDp / screenWidthInDp;
        float anchorY = centerYInDp / screenHeightInDp;
        if(inputFieldX.text == "")
            inputFieldX.text = anchorX.ToString(CultureInfo.InvariantCulture);
        if(inputFieldY.text == "")
            inputFieldY.text = anchorY.ToString(CultureInfo.InvariantCulture);
        AdmobManager.Instance.RequestRectBanner(int.Parse(inputFieldX.text), int.Parse(inputFieldY.text));

        /*// Calculate the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the center position
        float centerX = screenWidth / 2;
        float centerY = screenHeight / 2;

        // Convert the center position to device-independent pixels (dp)
        float centerXInDp = centerX / Screen.dpi * 160;
        float centerYInDp = centerY / Screen.dpi * 160;

        // Create an anchor for the banner ad
        float anchorX = centerXInDp / screenWidth;
        float anchorY = centerYInDp / screenHeight;
        inputFieldX.text = anchorX.ToString(CultureInfo.InvariantCulture);
        inputFieldY.text = anchorY.ToString(CultureInfo.InvariantCulture);
        AdmobManager.Instance.RequestRectBanner((int)anchorX, 760);*/
    }
}