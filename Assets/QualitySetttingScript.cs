using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class QualitySetttingScript : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevel;

    void Start()
    {
        int level = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        transform.GetComponent<TMP_Dropdown>().SetValueWithoutNotify(level);
        ChangeLevel(level);
    }

    public void ChangeLevel(int value)
    {
        if (value < 0 || value >= qualityLevel.Length)
            return;
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevel[value];
        PlayerPrefs.SetInt("QualityLevel", value);
    }
}
