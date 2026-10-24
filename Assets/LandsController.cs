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
        if (countland < LandsList.Count && LandsList[countland])
        {
            Destroy(LandsList[countland].gameObject);
        }
        countland++;
        //LandsList.RemoveAt(countland);
    }

    void Awake()
    {
        countland = 0;
        instance = this;
    }


}
