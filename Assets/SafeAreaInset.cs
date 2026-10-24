using UnityEngine;

// Moves an edge-anchored HUD element in from notches, camera cutouts and gesture bars in landscape.
// Portrait keeps the authored positions. Works on the element's own anchors, so the hierarchy is untouched.
public class SafeAreaInset : MonoBehaviour
{
    static readonly string[] HudNames = { "TopLeft", "TopRight", "Top", "Bottom", "version", "Name", "TimeToReach" };

    RectTransform rt;
    Canvas canvas;
    Vector2 basePosition;
    Vector2 baseOffsetMin;
    Vector2 baseOffsetMax;
    Rect lastSafeArea;
    Vector2Int lastScreen;
    float lastScale;

    public static void AddToHud(Transform canvasRoot)
    {
        foreach (string hudName in HudNames)
        {
            Transform t = canvasRoot.Find(hudName);
            if (t && !t.GetComponent<SafeAreaInset>())
                t.gameObject.AddComponent<SafeAreaInset>();
        }
    }

    void Awake()
    {
        rt = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        basePosition = rt.anchoredPosition;
        baseOffsetMin = rt.offsetMin;
        baseOffsetMax = rt.offsetMax;
    }

    void LateUpdate()
    {
        Rect safe = Screen.safeArea;
        var screen = new Vector2Int(Screen.width, Screen.height);
        float scale = canvas.scaleFactor;
        if (safe == lastSafeArea && screen == lastScreen && Mathf.Approximately(scale, lastScale))
            return;
        lastSafeArea = safe;
        lastScreen = screen;
        lastScale = scale;
        Apply(safe, scale);
    }

    void Apply(Rect safe, float scale)
    {
        bool landscape = Screen.width > Screen.height;
        float left = landscape ? safe.xMin / scale : 0f;
        float right = landscape ? (Screen.width - safe.xMax) / scale : 0f;
        float bottom = landscape ? safe.yMin / scale : 0f;
        float top = landscape ? (Screen.height - safe.yMax) / scale : 0f;

        if (rt.anchorMin.x == rt.anchorMax.x)
        {
            float shift = rt.anchorMin.x == 0f ? left : rt.anchorMin.x == 1f ? -right : 0f;
            rt.anchoredPosition = new Vector2(basePosition.x + shift, rt.anchoredPosition.y);
        }
        else
        {
            rt.offsetMin = new Vector2(baseOffsetMin.x + left, rt.offsetMin.y);
            rt.offsetMax = new Vector2(baseOffsetMax.x - right, rt.offsetMax.y);
        }

        if (rt.anchorMin.y == rt.anchorMax.y)
        {
            float shift = rt.anchorMin.y == 0f ? bottom : rt.anchorMin.y == 1f ? -top : 0f;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, basePosition.y + shift);
        }
        else
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, baseOffsetMin.y + bottom);
            rt.offsetMax = new Vector2(rt.offsetMax.x, baseOffsetMax.y - top);
        }
    }
}
