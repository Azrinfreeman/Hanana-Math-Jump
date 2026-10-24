using UnityEngine;
using UnityEngine.UI;

public class ButtonHandling : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        bool interactable = !GameController.instance.gameStart;
        if (button.interactable != interactable)
        {
            button.interactable = interactable;
        }
    }
}
