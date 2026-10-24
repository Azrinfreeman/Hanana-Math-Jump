using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public bool isRightLine;
    public bool isLeftLine;

    [Header("References")]
    public PathController player;

    enum Kind
    {
        Straight,
        Left,
        Right,
    }

    // Index matches GameController.enterLevel: green (no keyword), Blue, Yellow, Pink, Cyan.
    static readonly string[] ColourNames = { "", "Blue", "Yellow", "Pink", "Cyan" };
    static readonly List<int> candidates = new List<int>();

    bool spawned;

    void Start()
    {
        player = PathController.instance
            ? PathController.instance
            : GameObject.Find("Player").GetComponent<PathController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (spawned || !other.gameObject.CompareTag("Player"))
            return;
        spawned = true;

        LandsController.instance.RemoveLand();
        SpawnNextLand();
    }

    void SpawnNextLand()
    {
        float y = player.transform.eulerAngles.y;
        bool facing90 = y >= 80f && y < 270f;
        Kind standingOn = KindOf(player.LandType);

        // Tiles allowed after the one the player stands on, and the yaw the new tile needs.
        Kind first;
        Kind second;
        float yaw;
        if (standingOn == Kind.Straight && !facing90)
        {
            first = Kind.Left;
            second = Kind.Straight;
            yaw = 0f;
        }
        else if (standingOn == Kind.Left && !facing90)
        {
            first = Kind.Right;
            second = Kind.Straight;
            yaw = 90f;
        }
        else if (standingOn == Kind.Straight && facing90)
        {
            first = Kind.Straight;
            second = Kind.Right;
            yaw = 90f;
        }
        else if (standingOn == Kind.Right && facing90)
        {
            first = Kind.Straight;
            second = Kind.Left;
            yaw = 0f;
        }
        else
        {
            // Unexpected combination: keep the track going with a straight tile.
            Debug.LogWarning("SpawnPrefab fallback: " + player.LandType + " y=" + y);
            first = second = Kind.Straight;
            yaw = facing90 ? 90f : 0f;
        }

        int colour = CurrentColour();
        Transform prefab = Pick(colour, first, second);
        if (!prefab)
            prefab = Pick(colour, Kind.Straight, Kind.Straight);
        if (!prefab)
        {
            Debug.LogError("SpawnPrefab: no prefab available for colour index " + colour);
            return;
        }

        Transform t = Instantiate(
            prefab,
            transform.parent.Find("spawnPosition").position,
            Quaternion.Euler(0f, yaw, 0f)
        );
        LandsController.instance.LandsList.Add(t);
    }

    static int CurrentColour()
    {
        bool[] levels = GameController.instance.enterLevel;
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i])
                return i;
        }
        return 0;
    }

    // Random entry of the prefab list (duplicates keep their weight) matching colour and kind.
    static Transform Pick(int colour, Kind a, Kind b)
    {
        List<Transform> list = PrefabController.instance.prefabList;
        candidates.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            string n = list[i].name;
            Kind k = KindOf(n);
            if (MatchesColour(n, colour) && (k == a || k == b))
                candidates.Add(i);
        }
        if (candidates.Count == 0)
            return null;
        return list[candidates[Random.Range(0, candidates.Count)]];
    }

    static bool MatchesColour(string name, int colour)
    {
        if (colour > 0)
            return name.Contains(ColourNames[colour]);
        for (int i = 1; i < ColourNames.Length; i++)
        {
            if (name.Contains(ColourNames[i]))
                return false;
        }
        return true;
    }

    static Kind KindOf(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Kind.Straight;
        if (name.Contains("Left"))
            return Kind.Left;
        if (name.Contains("Right"))
            return Kind.Right;
        return Kind.Straight;
    }
}
