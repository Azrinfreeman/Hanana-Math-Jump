using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Starts UiScreenshotRunner from the menu, or when output/ui-shots/RUN exists (content: sizes such as
// "1080x2400;1920x1080", or "current"). Each size is set on the Game view, captured, then the next one runs.
[InitializeOnLoad]
public static class UiScreenshotMenu
{
    const string QueueKey = "FunMath.UiShots.Queue";
    const string TriggerFile = "output/ui-shots/RUN";
    const string StopFile = "output/ui-shots/STOP";
    static double nextPoll;

    static UiScreenshotMenu()
    {
        EditorApplication.update += Poll;
    }

    [MenuItem("Tools/FunMath/Capture UI Screenshots")]
    static void Capture()
    {
        EditorPrefs.SetString(QueueKey, "current");
    }

    static void Poll()
    {
        if (EditorApplication.timeSinceStartup < nextPoll)
            return;
        nextPoll = EditorApplication.timeSinceStartup + 1.0;

        if (File.Exists(StopFile))
        {
            File.Delete(StopFile);
            EditorPrefs.SetString(QueueKey, "");
            EditorPrefs.SetBool(UiScreenshotRunner.Flag, false);
            EditorApplication.isPlaying = false;
            return;
        }

        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling)
            return;

        if (File.Exists(TriggerFile))
        {
            EditorPrefs.SetString(QueueKey, File.ReadAllText(TriggerFile).Trim());
            File.Delete(TriggerFile);
        }

        string queue = EditorPrefs.GetString(QueueKey);
        if (string.IsNullOrEmpty(queue))
            return;

        string[] sizes = queue.Split(';');
        EditorPrefs.SetString(QueueKey, string.Join(";", sizes.Skip(1)));
        Begin(sizes[0]);
    }

    static void Begin(string size)
    {
        if (EditorSceneManager.GetActiveScene().name != "MainGame")
        {
            Debug.LogWarning("UiScreenshotMenu: open the MainGame scene first.");
            EditorPrefs.SetString(QueueKey, "");
            return;
        }

        string[] wh = size.Split('x');
        if (wh.Length == 2 && int.TryParse(wh[0], out int w) && int.TryParse(wh[1], out int h))
        {
            try
            {
                SetGameViewSize(w, h);
            }
            catch (Exception e)
            {
                Debug.LogWarning("UiScreenshotMenu: could not set Game view size, using current. " + e.Message);
            }
        }

        EditorPrefs.SetBool(UiScreenshotRunner.Flag, true);
        EditorApplication.isPlaying = true;
    }

    // Adds (once) and selects a fixed-resolution Game view size through the editor's internal API.
    static void SetGameViewSize(int width, int height)
    {
        Assembly asm = typeof(Editor).Assembly;
        Type sizesType = asm.GetType("UnityEditor.GameViewSizes");
        Type singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        object instance = singleton.GetProperty("instance").GetValue(null);
        object groupType = sizesType.GetProperty("currentGroupType").GetValue(instance);
        object group = sizesType.GetMethod("GetGroup").Invoke(instance, new[] { groupType });
        Type groupClass = group.GetType();
        Type sizeClass = asm.GetType("UnityEditor.GameViewSize");
        Type sizeKind = asm.GetType("UnityEditor.GameViewSizeType");

        int count = (int)groupClass.GetMethod("GetTotalCount").Invoke(group, null);
        int index = -1;
        for (int i = 0; i < count; i++)
        {
            object s = groupClass.GetMethod("GetGameViewSize").Invoke(group, new object[] { i });
            if (
                (int)sizeClass.GetProperty("width").GetValue(s) == width
                && (int)sizeClass.GetProperty("height").GetValue(s) == height
                && Convert.ToInt32(sizeClass.GetProperty("sizeType").GetValue(s)) == 1
            )
            {
                index = i;
                break;
            }
        }
        if (index < 0)
        {
            object size = sizeClass
                .GetConstructor(new[] { sizeKind, typeof(int), typeof(int), typeof(string) })
                .Invoke(new object[] { Enum.ToObject(sizeKind, 1), width, height, "FunMath " + width + "x" + height });
            groupClass.GetMethod("AddCustomSize").Invoke(group, new[] { size });
            index = count;
        }

        Type gameView = asm.GetType("UnityEditor.GameView");
        EditorWindow window = EditorWindow.GetWindow(gameView);
        gameView
            .GetMethod("SizeSelectionCallback", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Invoke(window, new object[] { index, null });
    }
}
