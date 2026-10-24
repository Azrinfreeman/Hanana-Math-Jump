using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Batch UI changes for the open MainGame scene. Driven by output/ui-polish/RUN, whose content is
//   report                      list fonts and small touch targets (changes nothing)
//   apply:<op>;<op>...          ops: font=<to>:<path part>|<path part>  (retargets Liberation texts),
//                               fallback=<font>=><fallback font>, touch, menuheights,
//                               sliders, scrollbars, fixhandles, questionlayout
// Writes output/ui-polish/report.txt. Refuses to apply if the scene has unsaved changes, and saves the
// scene afterwards so the result can be reviewed with git.
[InitializeOnLoad]
public static class UiPolishTool
{
    const string Dir = "output/ui-polish";
    const string Trigger = Dir + "/RUN";
    const float MinTarget = 190f;
    static double nextPoll;

    static UiPolishTool()
    {
        EditorApplication.update += Poll;
    }

    static void Poll()
    {
        if (EditorApplication.timeSinceStartup < nextPoll)
            return;
        nextPoll = EditorApplication.timeSinceStartup + 1.0;
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || !File.Exists(Trigger))
            return;

        string command = File.ReadAllText(Trigger).Trim();
        File.Delete(Trigger);
        var sb = new StringBuilder();
        try
        {
            Run(command, sb);
        }
        catch (Exception e)
        {
            sb.AppendLine("ERROR: " + e);
        }
        Directory.CreateDirectory(Dir);
        File.WriteAllText(Dir + "/report.txt", sb.ToString());
    }

    static void Run(string command, StringBuilder log)
    {
        var scene = EditorSceneManager.GetActiveScene();
        log.AppendLine("scene: " + scene.name + " dirty=" + scene.isDirty + " command=" + command);
        if (scene.name != "MainGame")
        {
            log.AppendLine("Open MainGame first.");
            return;
        }

        bool apply = command.StartsWith("apply:");
        if (apply && scene.isDirty)
        {
            log.AppendLine("Scene has unsaved changes. Save it first, then run again.");
            return;
        }

        string[] ops = apply ? command.Substring(6).Split(';') : new[] { "report" };
        int changes = 0;
        foreach (string op in ops)
        {
            if (op == "report")
            {
                ReportFonts(log);
                ReportTouch(log, false);
                ReportMenu(log, false);
            }
            else if (op.StartsWith("font="))
                changes += FontByPath(op.Substring(5), log);
            else if (op.StartsWith("fallback="))
                changes += AddFallback(op.Substring(9), log);
            else if (op == "touch")
                changes += ReportTouch(log, true);
            else if (op == "sliders")
                changes += PolishSliders(log);
            else if (op == "questionlayout")
                changes += QuestionLayout(log);
            else if (op == "fixhandles")
                changes += FixHandles(log);
            else if (op == "scrollbars")
                changes += WidenScrollbars(log);
            else if (op == "menuheights")
                changes += ReportMenu(log, true);
            else
                log.AppendLine("unknown op: " + op);
        }

        if (apply && changes > 0)
        {
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            log.AppendLine("saved scene, changes=" + changes);
        }
    }

    static string PathOf(Transform t)
    {
        var parts = new List<string>();
        for (; t != null; t = t.parent)
            parts.Add(t.name);
        parts.Reverse();
        return string.Join("/", parts.Skip(1));
    }

    static void ReportFonts(StringBuilder log)
    {
        log.AppendLine("== fonts");
        foreach (var g in UnityEngine.Object.FindObjectsOfType<TMP_Text>(true).GroupBy(t => t.font ? t.font.name : "(none)"))
        {
            log.AppendLine(g.Key + ": " + g.Count());
            foreach (TMP_Text t in g.Take(40))
                log.AppendLine("   " + PathOf(t.transform) + " | \"" + t.text.Replace("\n", " ") + "\" size=" + t.fontSize);
        }
        log.AppendLine("available font assets:");
        foreach (string guid in AssetDatabase.FindAssets("t:TMP_FontAsset"))
            log.AppendLine("   " + AssetDatabase.GUIDToAssetPath(guid));
    }

    static TMP_FontAsset LoadFont(string name)
    {
        return AssetDatabase
            .FindAssets("t:TMP_FontAsset " + name)
            .Select(g => AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(AssetDatabase.GUIDToAssetPath(g)))
            .FirstOrDefault(f => f.name == name);
    }

    // font=<to>:<part>|<part>: Liberation texts whose scene path contains a part get the new font.
    static int FontByPath(string spec, StringBuilder log)
    {
        int colon = spec.IndexOf(':');
        TMP_FontAsset to = LoadFont(spec.Substring(0, colon));
        string[] parts = spec.Substring(colon + 1).Split('|');
        if (!to)
        {
            log.AppendLine("font not found: " + spec.Substring(0, colon));
            return 0;
        }

        int n = 0;
        foreach (TMP_Text t in UnityEngine.Object.FindObjectsOfType<TMP_Text>(true))
        {
            if (!t.font || !t.font.name.StartsWith("LiberationSans"))
                continue;
            string path = PathOf(t.transform);
            if (!parts.Any(path.Contains))
                continue;
            Undo.RecordObject(t, "font");
            t.font = to;
            EditorUtility.SetDirty(t);
            log.AppendLine("   -> " + to.name + ": " + path);
            n++;
        }
        log.AppendLine("font=" + to.name + ": " + n + " texts");
        return n;
    }

    // fallback=<font>=><fallback>: missing glyphs in <font> are taken from <fallback>.
    static int AddFallback(string spec, StringBuilder log)
    {
        string[] ft = spec.Split(new[] { "=>" }, StringSplitOptions.None);
        TMP_FontAsset font = LoadFont(ft[0]);
        TMP_FontAsset fallback = LoadFont(ft[1]);
        if (!font || !fallback)
        {
            log.AppendLine("fallback font missing: " + spec);
            return 0;
        }
        if (font.fallbackFontAssetTable.Contains(fallback))
        {
            log.AppendLine("fallback already set: " + spec);
            return 0;
        }
        font.fallbackFontAssetTable.Add(fallback);
        EditorUtility.SetDirty(font);
        AssetDatabase.SaveAssets();
        log.AppendLine("fallback added: " + spec);
        return 1;
    }

    static Vector2 CanvasUnits(RectTransform rt)
    {
        Canvas parent = rt.GetComponentInParent<Canvas>();
        if (!parent)
            return new Vector2(float.MaxValue, float.MaxValue);
        Canvas canvas = parent.rootCanvas;
        Vector3 s = rt.lossyScale;
        Vector3 c = canvas.transform.lossyScale;
        return new Vector2(rt.rect.width * s.x / c.x, rt.rect.height * s.y / c.y);
    }

    static int ReportTouch(StringBuilder log, bool apply)
    {
        log.AppendLine(apply ? "== touch padding applied" : "== buttons below " + MinTarget + " canvas units");
        int n = 0;
        foreach (Button b in UnityEngine.Object.FindObjectsOfType<Button>(true))
        {
            Graphic g = b.targetGraphic;
            if (!g)
                continue;
            Vector2 size = CanvasUnits(g.rectTransform);
            float px = Mathf.Max(0f, (MinTarget - size.x) / 2f);
            float py = Mathf.Max(0f, (MinTarget - size.y) / 2f);
            if (px <= 0.5f && py <= 0.5f)
                continue;
            log.AppendLine("   " + PathOf(b.transform) + " size=" + size.x.ToString("0") + "x" + size.y.ToString("0") + " pad=" + px.ToString("0") + "," + py.ToString("0"));
            if (apply)
            {
                Undo.RecordObject(g, "touch padding");
                // left, bottom, right, top (negative grows the hit area)
                g.raycastPadding = new Vector4(-px, -py, -px, -py);
                EditorUtility.SetDirty(g);
            }
            n++;
        }
        return apply ? n : 0;
    }

    // Settings sliders: cyan fill, a visible round handle (they had none), label clear of the bar.
    static int PolishSliders(StringBuilder log)
    {
        Sprite knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        int n = 0;
        foreach (Slider s in UnityEngine.Object.FindObjectsOfType<Slider>(true))
        {
            string path = PathOf(s.transform);
            if (!path.StartsWith("Center/Settings"))
                continue;

            if (s.fillRect)
            {
                Image fill = s.fillRect.GetComponent<Image>();
                Undo.RecordObject(fill, "slider fill");
                fill.color = UiColors.Accent;
            }

            if (!s.handleRect)
            {
                var area = new GameObject("Handle Slide Area", typeof(RectTransform));
                Undo.RegisterCreatedObjectUndo(area, "slider handle");
                area.transform.SetParent(s.transform, false);
                var art = (RectTransform)area.transform;
                art.anchorMin = Vector2.zero;
                art.anchorMax = Vector2.one;
                art.offsetMin = new Vector2(55, 0);
                art.offsetMax = new Vector2(-55, 0);
                if (s.fillRect)
                    area.transform.SetSiblingIndex(s.fillRect.parent.GetSiblingIndex() + 1);

                var handle = new GameObject("Handle", typeof(RectTransform), typeof(Image));
                handle.transform.SetParent(area.transform, false);
                var hr = (RectTransform)handle.transform;
                hr.anchorMin = hr.anchorMax = new Vector2(0f, 0.5f);
                hr.sizeDelta = new Vector2(110, 110);
                Image img = handle.GetComponent<Image>();
                img.sprite = knob;
                img.color = UiColors.Primary;

                Undo.RecordObject(s, "slider handle");
                s.handleRect = hr;
            }

            Transform label = s.transform.Find("Text (TMP)");
            if (label)
            {
                var lr = (RectTransform)label;
                Undo.RecordObject(lr, "slider label");
                lr.anchoredPosition = new Vector2(lr.anchoredPosition.x, -30f);
            }
            EditorUtility.SetDirty(s);
            log.AppendLine("   slider " + path);
            n++;
        }
        return n;
    }

    // Question panel: two larger answer buttons inside the screen margins; the unused second row is hidden.
    static int QuestionLayout(StringBuilder log)
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform panel = canvas ? canvas.transform.Find("Question") : null;
        Transform row = panel ? panel.Find("horizontal") : null;
        if (!row)
        {
            log.AppendLine("Question/horizontal not found");
            return 0;
        }

        var panelRt = (RectTransform)panel;
        Undo.RecordObject(panelRt, "question panel");
        panelRt.sizeDelta = new Vector2(1560f, panelRt.sizeDelta.y);

        var rowRt = (RectTransform)row;
        Undo.RecordObject(rowRt, "question row");
        rowRt.sizeDelta = new Vector2(1500f, 360f);
        var layout = row.GetComponent<HorizontalLayoutGroup>();
        Undo.RecordObject(layout, "question row layout");
        layout.spacing = 60f;
        layout.padding = new RectOffset(0, 0, 0, 0);
        layout.childAlignment = TextAnchor.MiddleCenter;

        foreach (Transform button in row)
        {
            var rt = (RectTransform)button;
            Undo.RecordObject(rt, "answer button");
            rt.sizeDelta = new Vector2(700f, 360f);
            foreach (TMP_Text label in button.GetComponentsInChildren<TMP_Text>(true))
            {
                Undo.RecordObject(label, "answer font");
                label.fontSize = 170f;
            }
            log.AppendLine("   answer button " + button.name + " -> 700x360");
        }

        Transform row2 = panel.Find("horizontal (1)");
        if (row2)
        {
            Undo.RecordObject(row2.gameObject, "hide unused row");
            row2.gameObject.SetActive(false);
            log.AppendLine("   hid unused second row");
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRt);
        EditorUtility.SetDirty(panelRt);
        return 1;
    }

    // Slider stretches the handle across the slide area's height, so give the area a fixed height.
    static int FixHandles(StringBuilder log)
    {
        int n = 0;
        foreach (Slider s in UnityEngine.Object.FindObjectsOfType<Slider>(true))
        {
            if (!PathOf(s.transform).StartsWith("Center/Settings") || !s.handleRect)
                continue;
            var area = (RectTransform)s.handleRect.parent;
            Undo.RecordObject(area, "handle area");
            Undo.RecordObject(s.handleRect, "handle");
            area.anchorMin = new Vector2(0f, 0.5f);
            area.anchorMax = new Vector2(1f, 0.5f);
            area.offsetMin = new Vector2(55f, -55f);
            area.offsetMax = new Vector2(-55f, 55f);
            s.handleRect.sizeDelta = new Vector2(110f, 0f);
            EditorUtility.SetDirty(area);
            EditorUtility.SetDirty(s.handleRect);
            log.AppendLine("   handle " + PathOf(s.transform));
            n++;
        }

        // the dropdown list's scrollbar was widened by mistake; its viewport expects 20
        GameObject canvas = GameObject.Find("Canvas");
        Transform bar = canvas ? canvas.transform.Find("TopRight/dropdown/Template/Scrollbar") : null;
        if (bar && ((RectTransform)bar).sizeDelta.x > 21f)
        {
            Undo.RecordObject(bar, "scrollbar width");
            var rt = (RectTransform)bar;
            rt.sizeDelta = new Vector2(20f, rt.sizeDelta.y);
            EditorUtility.SetDirty(rt);
            log.AppendLine("   dropdown scrollbar back to 20");
            n++;
        }
        return n;
    }

    static int WidenScrollbars(StringBuilder log)
    {
        int n = 0;
        foreach (Scrollbar sb in UnityEngine.Object.FindObjectsOfType<Scrollbar>(true))
        {
            var rt = (RectTransform)sb.transform;
            if (rt.anchorMin.x == rt.anchorMax.x && rt.sizeDelta.x < 40f && !PathOf(rt).Contains("dropdown"))
            {
                Undo.RecordObject(rt, "scrollbar width");
                rt.sizeDelta = new Vector2(48f, rt.sizeDelta.y);
                EditorUtility.SetDirty(rt);
                log.AppendLine("   scrollbar " + PathOf(rt) + " -> 48");
                n++;
            }
        }
        return n;
    }

    static int ReportMenu(StringBuilder log, bool apply)
    {
        log.AppendLine(apply ? "== menu heights applied" : "== menu button heights");
        GameObject canvas = GameObject.Find("Canvas");
        Transform menu = canvas ? canvas.transform.Find("Center/1") : null;
        if (!menu)
            return 0;
        int n = 0;
        foreach (Transform child in menu)
        {
            var rt = (RectTransform)child;
            if (child.name.EndsWith("Button") || child.name.Contains("Button ("))
            {
                log.AppendLine("   " + child.name + " active=" + child.gameObject.activeSelf + " h=" + rt.sizeDelta.y.ToString("0.0") + " w=" + rt.sizeDelta.x.ToString("0.0"));
                if (apply && child.gameObject.activeSelf && Mathf.Abs(rt.sizeDelta.y - 218f) > 0.5f)
                {
                    Undo.RecordObject(rt, "menu height");
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, 218f);
                    EditorUtility.SetDirty(rt);
                    n++;
                }
            }
        }
        return n;
    }
}
