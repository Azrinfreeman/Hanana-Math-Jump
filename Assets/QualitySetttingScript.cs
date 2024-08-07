using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class QualitySetttingScript : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevel;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<TMP_Dropdown>().value = QualitySettings.GetQualityLevel();
    }

    // Update is called once per frame
    void Update() { }

    public void ChangeLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevel[value];
    }
}
