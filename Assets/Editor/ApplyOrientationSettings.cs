using UnityEditor;
using UnityEngine;

// One-off: switches Player Settings to auto-rotate (portrait + both landscapes). Safe to run repeatedly.
public static class ApplyOrientationSettings
{
    [InitializeOnLoadMethod]
    [MenuItem("Tools/FunMath/Apply Auto-Rotate Settings")]
    static void Apply()
    {
        bool changed = false;
        if (PlayerSettings.defaultInterfaceOrientation != UIOrientation.AutoRotation)
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            changed = true;
        }
        changed |= Set(PlayerSettings.allowedAutorotateToPortrait, true, v => PlayerSettings.allowedAutorotateToPortrait = v);
        changed |= Set(PlayerSettings.allowedAutorotateToLandscapeLeft, true, v => PlayerSettings.allowedAutorotateToLandscapeLeft = v);
        changed |= Set(PlayerSettings.allowedAutorotateToLandscapeRight, true, v => PlayerSettings.allowedAutorotateToLandscapeRight = v);

        if (changed)
        {
            AssetDatabase.SaveAssets();
            Debug.Log("ApplyOrientationSettings: Player Settings set to Auto Rotation (portrait + landscape left/right).");
        }
    }

    static bool Set(bool current, bool wanted, System.Action<bool> apply)
    {
        if (current == wanted)
            return false;
        apply(wanted);
        return true;
    }
}
