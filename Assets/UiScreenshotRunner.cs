#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Editor-only: walks through the UI screens of MainGame in Play mode and saves Game view screenshots.
// Started from Tools > FunMath > Capture UI Screenshots. Changes only which panels are visible, then restores them.
public class UiScreenshotRunner : MonoBehaviour
{
    public const string Flag = "FunMath.UiShots";

    static readonly string[] Panels =
    {
        "PlayerInput", "Center/1", "Center/Settings", "Center/Settings (1)", "Center/PlaySelect",
        "Center/StoreSelect", "Scoreboard", "Credit", "PlayerSelect", "Question", "EndScreen",
        "BeliPrompt", "PlayerReset", "TimeToReach",
    };

    struct Shot
    {
        public string name;
        public string show;
        public float wait;
    }

    static readonly Shot[] Shots =
    {
        new Shot { name = "01-initial", show = null, wait = 8f },
        new Shot { name = "02-settings", show = "Center/Settings", wait = 0.6f },
        new Shot { name = "03-settings-levels", show = "Center/Settings (1)", wait = 0.6f },
        new Shot { name = "04-play-select", show = "Center/PlaySelect", wait = 0.6f },
        new Shot { name = "05-store", show = "Center/StoreSelect", wait = 0.8f },
        new Shot { name = "06-scoreboard", show = "Scoreboard", wait = 7f },
        new Shot { name = "07-credit", show = "Credit", wait = 0.6f },
        new Shot { name = "08-players", show = "PlayerSelect", wait = 0.8f },
        new Shot { name = "09-name-entry", show = "PlayerInput", wait = 0.8f },
        new Shot { name = "10-question", show = "Question", wait = 0.6f },
        new Shot { name = "11-time-to-reach", show = "TimeToReach", wait = 0.6f },
        new Shot { name = "12-end-screen", show = "EndScreen", wait = 0.8f },
        new Shot { name = "13-unlock-prompt", show = "BeliPrompt", wait = 0.8f },
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Start()
    {
        if (EditorPrefs.GetBool(Flag) && SceneManager.GetActiveScene().name == "MainGame")
            new GameObject("UiScreenshotRunner").AddComponent<UiScreenshotRunner>();
    }

    IEnumerator Run()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        string dir = Path.GetFullPath("output/ui-shots");
        Directory.CreateDirectory(dir);

        var original = new Dictionary<string, bool>();
        foreach (string p in Panels)
        {
            Transform t = canvas.Find(p);
            if (t)
                original[p] = t.gameObject.activeSelf;
        }

        foreach (Shot shot in Shots)
        {
            if (shot.show != null)
            {
                foreach (string p in Panels)
                {
                    Transform t = canvas.Find(p);
                    if (t)
                        t.gameObject.SetActive(p == shot.show);
                }
            }
            if (shot.show == "EndScreen")
                GameController.instance.ShowRunSummary();
            yield return new WaitForSecondsRealtime(shot.wait);
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(Path.Combine(dir, shot.name + "_" + Screen.width + "x" + Screen.height + ".png"));
            yield return new WaitForSecondsRealtime(0.3f);
        }

        foreach (KeyValuePair<string, bool> kv in original)
            canvas.Find(kv.Key).gameObject.SetActive(kv.Value);

        Debug.Log("UiScreenshotRunner: saved screenshots to " + dir);
        EditorPrefs.SetBool(Flag, false);
        EditorApplication.isPlaying = false;
    }

    void Awake()
    {
        StartCoroutine(Run());
    }
}
#endif
