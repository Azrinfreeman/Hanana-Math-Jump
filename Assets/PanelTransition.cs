using System.Collections;
using UnityEngine;

// Short fade (and a small pop of the card image) whenever a menu panel is shown. Uses unscaled time.
// Panels that already have an Animator are left alone.
public class PanelTransition : MonoBehaviour
{
    const float Duration = 0.18f;
    const float StartScale = 0.94f;

    static readonly string[] PanelPaths =
    {
        "Center/Settings", "Center/Settings (1)", "Center/PlaySelect", "Center/StoreSelect",
        "Scoreboard", "Credit", "EndScreen",
    };

    CanvasGroup group;
    Transform card;
    Vector3 cardScale;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Install()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) => AddToPanels();
        AddToPanels();
    }

    static void AddToPanels()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas)
            return;
        foreach (string path in PanelPaths)
        {
            Transform t = canvas.transform.Find(path);
            if (t && !t.GetComponent<Animator>() && !t.GetComponent<PanelTransition>())
                t.gameObject.AddComponent<PanelTransition>();
        }
    }

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        if (!group)
            group = gameObject.AddComponent<CanvasGroup>();
        card = transform.Find("Image");
        if (card)
            cardScale = card.localScale;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Play());
    }

    void OnDisable()
    {
        group.alpha = 1f;
        if (card)
            card.localScale = cardScale;
    }

    IEnumerator Play()
    {
        group.alpha = 0f;
        for (float t = 0f; t < Duration; t += Time.unscaledDeltaTime)
        {
            float k = Mathf.SmoothStep(0f, 1f, t / Duration);
            group.alpha = k;
            if (card)
                card.localScale = cardScale * Mathf.Lerp(StartScale, 1f, k);
            yield return null;
        }
        group.alpha = 1f;
        if (card)
            card.localScale = cardScale;
    }
}
