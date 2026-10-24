using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Runs once at startup, before any scene loads.
public static class DisplayBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        // Smartboards and tablets register palms/second fingers; one touch at a time keeps answers unambiguous.
        Input.multiTouchEnabled = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        var go = new GameObject("DisplayWatcher");
        go.hideFlags = HideFlags.HideInHierarchy;
        Object.DontDestroyOnLoad(go);
        go.AddComponent<DisplayWatcher>();
    }
}

// Re-applies screen-size dependent input tuning when the screen size or the EventSystem changes,
// and plays the shared tap sound for every enabled Button that has no sound of its own.
public class DisplayWatcher : MonoBehaviour
{
    int lastWidth;
    int lastHeight;
    EventSystem lastEventSystem;

    readonly List<RaycastResult> hits = new List<RaycastResult>();

    void Update()
    {
        EventSystem current = EventSystem.current;
        if (current && Input.GetMouseButtonDown(0))
            PlayTapSound(current);

        bool sizeChanged = Screen.width != lastWidth || Screen.height != lastHeight;
        if (!sizeChanged && current == lastEventSystem)
            return;

        lastWidth = Screen.width;
        lastHeight = Screen.height;
        lastEventSystem = current;

        // Default drag threshold is 10 px; scale it so taps on large screens are not read as drags.
        if (current)
            current.pixelDragThreshold = Mathf.Max(10, Mathf.RoundToInt(Mathf.Min(lastWidth, lastHeight) * 0.01f));
    }

    void PlayTapSound(EventSystem current)
    {
        current.RaycastAll(new PointerEventData(current) { position = Input.mousePosition }, hits);
        if (hits.Count == 0)
            return;

        Button button = hits[0].gameObject.GetComponentInParent<Button>();
        if (button && button.interactable && !HasOwnSound(button))
            Sfx.Play("touch");
    }

    // Some buttons already play a sound through a UnityEvent in the scene.
    static bool HasOwnSound(Button button)
    {
        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            if (button.onClick.GetPersistentTarget(i) is AudioSource)
                return true;
        }
        return false;
    }
}
