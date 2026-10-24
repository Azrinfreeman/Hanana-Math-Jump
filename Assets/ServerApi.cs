using UnityEngine.Networking;

public static class ServerApi
{
    const string Base = "https://app-hanana.com/funmath/";
    public const int TimeoutSeconds = 10;

    public const string InsertMark = Base + "insertmark.php";
    public const string FetchRanking = Base + "fetchRanking.php";

    public static string Delete(string name, string id) =>
        Base + "fetchDelete.php?name=" + UnityWebRequest.EscapeURL(name) + "&id=" + UnityWebRequest.EscapeURL(id);
}
