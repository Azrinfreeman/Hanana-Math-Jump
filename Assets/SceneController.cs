using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void FinishSplash()
    {
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator exitGame(Animator anim)
    {
        anim.SetTrigger("onClick");
        yield return new WaitForSeconds(0.2f);

        Application.Quit();
    }
    public void ExitGame(Animator anim)
    {
        StartCoroutine(exitGame(anim));
    }
}
