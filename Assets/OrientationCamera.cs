using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps the authored (portrait) field of view in portrait. In landscape the same vertical FOV would
// show a very wide view, so the vertical FOV is reduced to hold a target horizontal FOV.
public class OrientationCamera : MonoBehaviour
{
    [Tooltip("Horizontal field of view (degrees) aimed for in landscape.")]
    public float landscapeHorizontalFov = 78f;

    CinemachineFreeLook freeLook;
    float portraitFov;
    int lastWidth;
    int lastHeight;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Install()
    {
        SceneManager.sceneLoaded += (scene, mode) => AddToFreeLooks();
        AddToFreeLooks();
    }

    static void AddToFreeLooks()
    {
        foreach (CinemachineFreeLook f in FindObjectsOfType<CinemachineFreeLook>(true))
        {
            if (!f.GetComponent<OrientationCamera>())
                f.gameObject.AddComponent<OrientationCamera>();
        }
    }

    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        portraitFov = freeLook.m_Lens.FieldOfView;
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

        float fov = portraitFov;
        if (Screen.width > Screen.height)
        {
            float aspect = (float)Screen.width / Screen.height;
            float verticalFov =
                2f * Mathf.Atan(Mathf.Tan(landscapeHorizontalFov * 0.5f * Mathf.Deg2Rad) / aspect) * Mathf.Rad2Deg;
            fov = Mathf.Min(verticalFov, portraitFov);
        }

        freeLook.m_Lens.FieldOfView = fov;
        if (!freeLook.m_CommonLens)
        {
            for (int i = 0; i < 3; i++)
            {
                CinemachineVirtualCamera rig = freeLook.GetRig(i);
                if (rig)
                    rig.m_Lens.FieldOfView = fov;
            }
        }
    }
}
