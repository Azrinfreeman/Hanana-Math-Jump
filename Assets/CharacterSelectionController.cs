using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelectionController : MonoBehaviour
{
    public TextMeshProUGUI topNameText;
    public Image characterImage;

    public int listNum;
    public int coinsNeeded;
    public int trophyNeeded;
    public TextMeshProUGUI coinsNeededText;
    public TextMeshProUGUI trophyNeededText;

    public Transform ButtonPilih;

    public Transform lockImage;

    [Header("References ")]
    public Transform BeliPrompt;
    // Start is called before the first frame update
    void Start()
    {
        lockImage = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).transform;
        topNameText = transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        characterImage = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        coinsNeededText = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        trophyNeededText = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        listNum = 0;
        ButtonPilih = transform.parent.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Transform>();
        Invoke("ReassignImageandName", 0.3f);
    }

    IEnumerator characterBuying()
    {
        ButtonPilih.GetComponent<Animator>().SetTrigger("onClick");
        ButtonPilih.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        if (PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")) >= coinsNeeded &&
         PlayerPrefs.GetInt("RoundsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_")) >= trophyNeeded)
        {
            //unlock character
            PlayerPrefs.SetInt("Character_No_" + listNum, 1);

            //minus the coins from player
            int sum = PlayerPrefs.GetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"));
            sum -= coinsNeeded;

            PlayerPrefs.SetInt("StarsCollected_" + PlayerPrefs.GetInt("CurrentPlayerNo_"), sum);

            lockImage.GetComponent<Animator>().Play("lockDisappear");
            yield return new WaitForSeconds(1);
            BeliPrompt.gameObject.SetActive(true);
            if (!GameObject.Find("unlocked").transform.GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Find("unlocked").transform.GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSeconds(1.5f);
            ReassignImageandName();

        }
        else
        {
            Debug.Log("Tak cukup duit");
        }

    }
    public void BeliCharacter()
    {
        StartCoroutine(characterBuying());
    }

    void ReassignImageandName()
    {
        ButtonPilih.GetChild(0).GetComponent<Button>().interactable = true;
        topNameText.text = PathController.instance.TransformAnimList[listNum].name;
        characterImage.sprite = CharacterSelection.instance.characters[listNum].GetComponent<Image>().sprite;
        //
        coinsNeeded = PathController.instance.TransformAnimList[listNum].GetComponent<CharacterInfo>().coinsNeeded;
        coinsNeededText.text = coinsNeeded.ToString() + "\nCoins";
        //
        trophyNeeded = PathController.instance.TransformAnimList[listNum].GetComponent<CharacterInfo>().trophyNeeded;
        trophyNeededText.text = trophyNeeded.ToString() + "\nTrophy";

        if (PlayerPrefs.GetInt("CurrentCharacterNo_") == listNum)
        {
            ButtonPilih.GetChild(0).GetComponent<Button>().interactable = false;
        }

        if (PlayerPrefs.GetInt("Character_No_" + listNum) == 1)
        {

            ButtonPilih.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            characterImage.color = new Color32(255, 255, 255, 255);
            ButtonPilih.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PILIH";
            lockImage.gameObject.SetActive(false);
            ButtonPilih.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ApplyCharacter());
        }
        else if (PlayerPrefs.GetInt("Character_No_" + listNum) == 0)
        {

            ButtonPilih.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            characterImage.color = new Color32(190, 190, 190, 255);
            ButtonPilih.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "BELI";
            lockImage.gameObject.SetActive(true);
            ButtonPilih.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BeliCharacter());
        }

    }

    public void RightClick()
    {
        listNum++;
        if (listNum == CharacterSelection.instance.characters.Count)
        {
            listNum = 0;
        }
        ReassignImageandName();
    }

    public void LeftClick()
    {
        listNum--;

        if (listNum < 0)
        {
            listNum = CharacterSelection.instance.characters.Count - 1;
        }
        ReassignImageandName();
    }

    public void ApplyCharacter()
    {
        ButtonPilih.GetComponent<Animator>().SetTrigger("onClick");
        ButtonPilih.GetChild(0).GetComponent<Button>().interactable = false;
        PlayerPrefs.SetInt("CurrentCharacterNo_", listNum);
        PathController.instance.SetFirstChildToLast(listNum);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
