using UnityEngine;
using UnityEngine.UI;

public class ColorValueSwitch : MonoBehaviour
{
    public Button targetButton;

    public void InActive()
    {
        SetButtonColorValue(0.65f);
    }

    public void Active()
    {
        SetButtonColorValue(1f);
    }

    private void SetButtonColorValue(float value)
    {
        if (targetButton == null) return;

        Image buttonImage = targetButton.image;
        if (buttonImage == null) return;

        Color currentColor = buttonImage.color;
        float h, s, v;
        Color.RGBToHSV(currentColor, out h, out s, out v);
        v = value;
        buttonImage.color = Color.HSVToRGB(h, s, v);
    }
}