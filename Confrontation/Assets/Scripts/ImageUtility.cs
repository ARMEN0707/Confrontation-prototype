using FuryLion.UI;
using UnityEngine;

public static class ImageUtility
{
    public static void SetImageScale(Image image, float x)
    {
        image.transform.localScale = new Vector3(x, image.transform.localScale.y, image.transform.localScale.z);
    }
    
    public static void SetImageScale(Image image, float x, float y)
    {
        image.transform.localScale = new Vector3(x, y, image.transform.localScale.z);
    }
}