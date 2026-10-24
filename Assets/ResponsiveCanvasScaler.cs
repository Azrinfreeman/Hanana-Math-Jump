using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Keeps the portrait-designed canvas as authored in portrait and switches it to height-based
// scaling in landscape, so panels fit the screen instead of shrinking to a few hundred pixels.
[RequireComponent(typeof(CanvasScaler))]
public class ResponsiveCanvasScaler : MonoBehaviour
{
    [Tooltip("Canvas height (in units) shown in landscape. Raise it to make the UI smaller.")]
    public float landscapeReferenceHeight = 3300f;

    CanvasScaler scaler;
    Vector2 portraitReference;
    float portraitMatch;
    int lastWidth;
    int lastHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Install()
    {
        SceneManager.sceneLoaded += (scene, mode) => AddToPortraitCanvases();
        AddToPortraitCanvases();
    }

    // Only canvases authored for portrait (taller reference than wide) need the switch.
    static void AddToPortraitCanvases()
    {
        foreach (CanvasScaler s in FindObjectsOfType<CanvasScaler>(true))
        {
            if (
                s.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize
                && s.referenceResolution.y > s.referenceResolution.x
                && !s.GetComponent<ResponsiveCanvasScaler>()
            )
            {
                s.gameObject.AddComponent<ResponsiveCanvasScaler>();
                SafeAreaInset.AddToHud(s.transform);
            }
        }
    }

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        portraitReference = scaler.referenceResolution;
        portraitMatch = scaler.matchWidthOrHeight;
        Apply();
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
            Apply();
    }

    void Apply()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        if (Screen.width > Screen.height)
        {
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.referenceResolution = new Vector2(portraitReference.x, landscapeReferenceHeight);
            scaler.matchWidthOrHeight = 1f;
        }
        else
        {
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.referenceResolution = portraitReference;
            scaler.matchWidthOrHeight = portraitMatch;
        }
    }
}
