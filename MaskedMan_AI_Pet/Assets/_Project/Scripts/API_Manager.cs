using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class API_Manager : MonoBehaviour {
    public static API_Manager instance;

    void Awake () {
        instance = this;
    }

    public void CallRandomFact(System.Action<string> callback) { //used to send through the fact when needed
        StartCoroutine(RandomFactAPI.Grab(callback));
    }
}

public class RandomFactAPI : MonoBehaviour { //grabs a string from a web request, exctract's the string between the two "<blockquote>" inside the html string and returns it as the received fact
    const string API_URL = "https://uselessfacts.jsph.pl/random";

    public static IEnumerator Grab(System.Action<string> callback) {
        string url = $"{API_URL}";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                string response = request.downloadHandler.text;
                string fact = ExtractFactFromHtml(response);
                callback?.Invoke(fact);
            } else {
                Debug.LogError($"Error: {request.error}");
                callback?.Invoke(null);
            }
        }
    }

    private static string ExtractFactFromHtml(string html) {
        // Use regex to find the content inside the blockquote tags
        Regex regex = new Regex("<blockquote>(.*?)</blockquote>", RegexOptions.Singleline);
        Match match = regex.Match(html);
        if (match.Success) {
            return match.Groups[1].Value;
        } else {
            return "Fact not found.";
        }
    }
}