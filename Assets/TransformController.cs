using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TransformController : MonoBehaviour
{
    //  public static TransformController instance;
    public List<string> userCollection;
    public List<string> userIdCollection;
    public Transform childrenContentPrefab;

    private void Awake()
    {
        // instance = this;
    }

    public Transform contentHeight;

    public int children;

    public List<Transform> listClones;

    public bool isRank;

    [Header("For Ranking")]
    public RankingList rankings = new RankingList();

    [System.Serializable]
    public class Ranking
    {
        public string nama;
        public int score;

        public string id_ingame;
    }

    [System.Serializable]
    public class RankingList
    {
        public Ranking[] ranking;
    }

    // Start is called before the first frame update
    void Start()
    {
        children = 0;
        contentHeight = transform;
    }

    // Update is called once per frame
    void Update()
    {
        children = listClones.Count;

        contentHeight.GetComponent<RectTransform>().sizeDelta = new Vector2(
            1300,
            200 * listClones.Count
        );
    }

    private void OnDisable()
    {
        if (isRank)
        {
            for (int i = 0; i < listClones.Count; i++)
            {
                //                Debug.Log("wr");
                Destroy(listClones[i].gameObject);
            }
            listClones.Clear();
        }
    }

    private void OnEnable()
    {
        children = 0;
        contentHeight = transform;

        children = listClones.Count;

        contentHeight.GetComponent<RectTransform>().sizeDelta = new Vector2(
            1300,
            250 * listClones.Count
        );
        if (!isRank)
        {
            AddPrefab();
        }
        else
        {
            AddPrefabRanking();
        }
    }

    public void AddPrefabRanking()
    {
        StartCoroutine(fetchRanking());
    }

    TextMeshProUGUI status;

    void ShowStatus(string text, bool retry)
    {
        if (!status)
        {
            var go = new GameObject("Status", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(Button));
            go.transform.SetParent(transform.parent, false);
            status = go.GetComponent<TextMeshProUGUI>();
            status.font = GameController.instance.levelText.font;
            status.fontSharedMaterial = status.font.material;
            status.fontSize = 80;
            status.alignment = TextAlignmentOptions.Center;
            status.color = new Color32(70, 70, 70, 255);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(60f, 60f);
            rt.offsetMax = new Vector2(-60f, -60f);
            var button = go.GetComponent<Button>();
            button.targetGraphic = status;
            button.onClick.AddListener(() =>
            {
                if (status.raycastTarget)
                    AddPrefabRanking();
            });
        }
        status.text = text;
        status.raycastTarget = retry;
        status.transform.SetAsLastSibling();
        status.gameObject.SetActive(true);
    }

    void HideStatus()
    {
        if (status)
            status.gameObject.SetActive(false);
    }

    static readonly Color Silver = new Color32(190, 196, 208, 255);
    static readonly Color Bronze = new Color32(205, 127, 50, 255);

    IEnumerator fetchRanking()
    {
        userIdCollection.Clear();
        userCollection.Clear();
        foreach (Transform old in listClones)
        {
            if (old)
                Destroy(old.gameObject);
        }
        listClones.Clear();
        ShowStatus("LOADING...", false);

        using (UnityWebRequest www = UnityWebRequest.Get(ServerApi.FetchRanking))
        {
            www.timeout = ServerApi.TimeoutSeconds;
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                ShowStatus("NO CONNECTION\nTAP TO RETRY", true);
                yield break;
            }

            //name in server head json must be ranking
            try
            {
                rankings = JsonUtility.FromJson<RankingList>(www.downloadHandler.text);
            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e.Message);
                rankings = null;
            }
            if (rankings == null || rankings.ranking == null || rankings.ranking.Length == 0)
            {
                ShowStatus("NO CONNECTION\nTAP TO RETRY", true);
                yield break;
            }
        }

        HideStatus();
        // keep the rows clear of the scrollbar on the right
        float rowWidth = ((RectTransform)transform).rect.width - 100f;
        string me = PlayerPrefs.GetString("CurrentPlayer_");
        string myId = PlayerPrefs.GetString("CurrentPlayerid_");
        int ownIndex = -1;

        for (int i = 0; i < rankings.ranking.Length; i++)
        {
            Transform content = Instantiate(childrenContentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            content.SetParent(transform);
            content.localScale = new Vector3(1, 1, 1);
            content.localPosition = new Vector3(0, 0, 0);
            var rowRect = (RectTransform)content;
            // shrink the whole row (bar and columns together) so it stays clear of the scrollbar
            float fullWidth = rowRect.rect.width > 0f ? rowRect.rect.width : 1299.4f;
            content.localScale = Vector3.one * Mathf.Clamp(rowWidth / fullWidth, 0.5f, 1f);
            listClones.Add(content);

            Transform row = content.GetChild(0);
            Ranking entry = rankings.ranking[i];
            TextMeshProUGUI rankText = row.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = row.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = row.GetChild(2).GetComponent<TextMeshProUGUI>();
            rankText.text = (i + 1) + ".";
            nameText.text = TextUtil.Printable(nameText, entry.nama);
            scoreText.text = entry.score.ToString();
            // shrink long ranks, names and scores to fit instead of spilling out of the row
            foreach (TextMeshProUGUI label in new[] { rankText, nameText, scoreText })
            {
                label.fontSizeMax = label.fontSize;
                label.fontSizeMin = 24f;
                label.enableAutoSizing = true;
            }

            Image bg = row.GetComponent<Image>();
            bool own = entry.nama.Equals(me) && entry.id_ingame.Equals(myId);
            if (own)
            {
                bg.color = UiColors.Primary;
                ownIndex = i;
            }
            else if (i < 3)
            {
                bg.color = i == 0 ? UiColors.Star : i == 1 ? Silver : Bronze;
                Color dark = new Color32(45, 45, 45, 255);
                rankText.color = nameText.color = scoreText.color = dark;
            }
        }

        // scroll so the player's own row is in view
        yield return null;
        Canvas.ForceUpdateCanvases();
        ScrollRect scroll = GetComponentInParent<ScrollRect>();
        if (scroll && ownIndex > 0 && listClones.Count > 1)
        {
            scroll.verticalNormalizedPosition = Mathf.Clamp01(1f - (float)ownIndex / (listClones.Count - 1));
        }
    }

    public void AddPrefab()
    {
        userIdCollection.Clear();
        userCollection.Clear();
        for (int i = 0; i < 20; i++)
        {
            //Debug.Log(i);
            //Debug.Log(PlayerPrefs.GetString("Player_" + i));

            string str = PlayerPrefs.GetString("Player_" + i);
            if (string.IsNullOrEmpty(str) == true)
            {
                // Debug.Log("null");
            }
            else
            {
                //  Debug.Log("addedd");
                //  Debug.Log(i);

                userCollection.Add("Player_" + i);
                userIdCollection.Add("Playerid_" + i);
                //if not empty add to content
                Transform content = Instantiate(
                    childrenContentPrefab,
                    new Vector3(0, 0, 0),
                    Quaternion.identity
                );
                content.SetParent(transform);
                content.localScale = new Vector3(1, 1, 1);
                content.localPosition = new Vector3(0, 0, 0);
                int l = i + 1;
                content
                    .transform.GetChild(0)
                    .transform.GetChild(0)
                    .GetComponent<TextMeshProUGUI>()
                    .text = userCollection.Count.ToString();
                listClones.Add(content);
            }
        }
    }

    public void ClearChildrenAndDismiss()
    {
        for (int i = 0; i < listClones.Count; i++)
        {
            //            Debug.Log("wr");
            Destroy(listClones[i].gameObject);
        }
        listClones.Clear();
        transform
            .parent.transform.parent.transform.parent.GetComponent<Animator>()
            .Play("dismissPlayerSelect");
    }

    public void DismissPlayerSelect()
    {
        for (int i = 0; i < listClones.Count; i++)
        {
            Debug.Log("wr");
            Destroy(listClones[i].gameObject);
        }
        listClones.Clear();
        transform
            .parent.transform.parent.transform.parent.GetComponent<Animator>()
            .Play("dismissPlayerSelect");
    }
}
