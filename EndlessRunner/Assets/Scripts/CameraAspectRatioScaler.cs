using UnityEngine;

// Responsive Camera Scaler
[ExecuteAlways]
public class CameraAspectRatioScaler : MonoBehaviour
{
    public Vector2 targetAspectSize =  new Vector3(9, 16);          // set the desired aspect ratio 
    public Vector3 camZoom = Vector3.one;                           // camera zoom factor to fit different aspect ratios

    void Start()
    {
        float targetAspectRatio = targetAspectSize.x / targetAspectSize.y;
        float deviceAspectRatio = (float)Screen.width / (float)Screen.height;   // determine the game window's current aspect ratio

        float scaleHeight = deviceAspectRatio / targetAspectRatio;              // current viewport height should be scaled by this amount

        Camera camera = GetComponent<Camera>();                                 

        if(scaleHeight < 1.0f)                                                  // if scaled height is less than current height, add letterbox
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else                                                                    // else, add pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}

// Script by https://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html