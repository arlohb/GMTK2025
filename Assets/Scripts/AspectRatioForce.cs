using UnityEngine;

[ExecuteInEditMode]
public class AspectRatioForce : MonoBehaviour
{
    public float aspectRatio = 16f / 9f;

    private Camera mainCamera;
    private Vector2 screenRes;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        Vector2 newScreenRes = new(Screen.width, Screen.height);
        if (screenRes == newScreenRes) return;
        screenRes = newScreenRes;

        float currentAspect = (float)Screen.width / Screen.height;

        // If screen it too wide
        if (currentAspect > aspectRatio)
        {
            float targetWidth = Screen.height * aspectRatio;
            float widthMul = targetWidth / Screen.width;
            float x = (1 - widthMul) / 2f;
            mainCamera.rect = new Rect(x, 0, widthMul, 1);
        }
        // If screen it too tall
        else if (currentAspect < aspectRatio)
        {
            float targetHeight = Screen.width / aspectRatio;
            float heightMul = targetHeight / Screen.height;
            float y = (1 - heightMul) / 2f;
            mainCamera.rect = new Rect(0, y, 1, heightMul);
        }
    }
}
