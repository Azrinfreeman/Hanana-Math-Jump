using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class UiTextFixer
{
    static readonly Dictionary<string, string> Replacements = new Dictionary<string, string>
    {
        { "KEMBALI", "BACK" },
        { "PILIH", "SELECT" },
        { "TAHNIAH!", "CONGRATULATIONS!" },
        { "ANDA BERJAYA MEMBELI CHARACTER BAHARU", "NEW CHARACTER UNLOCKED" },
    };

    [MenuItem("Tools/FunMath/Use English UI Text")]
    static void Run()
    {
        int changed = 0;
        foreach (TMP_Text text in Object.FindObjectsOfType<TMP_Text>(true))
        {
            if (Replacements.TryGetValue(text.text.Trim(), out string english))
            {
                Undo.RecordObject(text, "English UI text");
                text.text = english;
                EditorSceneManager.MarkSceneDirty(text.gameObject.scene);
                changed++;
            }
        }
        Debug.Log("UiTextFixer: replaced " + changed + " texts in the open scene(s). Save the scene.");
    }
}
