using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandsController : MonoBehaviour
{
    public static LandsController instance;

    public List<Transform> LandsList;
    public int countland;

    public void RemoveLand()
    {
        StartCoroutine(destroyPrevLand());
    }
    IEnumerator destroyPrevLand()
    {

        yield return new WaitForSeconds(16f);
        LandsList[countland].gameObject.SetActive(false);
        countland++;
        //LandsList.RemoveAt(countland);
    }

    void Awake()
    {
        countland = 0;
        instance = this;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {


    }
}
