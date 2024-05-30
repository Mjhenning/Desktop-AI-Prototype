using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Random = UnityEngine.Random;

public class API_Manager : MonoBehaviour {
    public static API_Manager instance;

    void Awake () {
        instance = this;
    }

    void Start () {
        
    }

    public void CallRandomFact(System.Action<string> callback) { //used to send through the fact when needed
        StartCoroutine(RandomFactAPI.Grab(callback));
    }
    
    public void CallRealTimeStatistics(string endpoint, System.Action<string> callback) {
        StartCoroutine(RealTimeStatisticsAPI.Grab(endpoint, callback));
    }
    
    public void HandleResponse(string response) {
        if (!string.IsNullOrEmpty(response)) {
            try {
                var jsonResponse = JObject.Parse(response);
                var responseToken = jsonResponse["response"];
                
                if (responseToken is JObject) {
                    var thisYear = responseToken["this_year"]?.ToString();
                    var today = responseToken["today"]?.ToString();

                    int random = Random.Range (0, 2);

                    switch (random) {
                        case 0:
                            EventsManager.FeedStat (thisYear, "this year");
                            break;
                        case 1:
                            EventsManager.FeedStat (today, "today");
                            break;
                    }
                    
                } else if (responseToken != null) {
                    var responseValue = responseToken.ToString();
                    EventsManager.FeedStat (responseValue, "");
                } else {
                    Debug.LogError("Failed to parse the response: response token is null");
                }
            } catch (Exception ex) {
                Debug.LogError($"Failed to parse the response: {ex.Message}");
            }
        } else {
            Debug.LogError("Failed to get response");
        }
    }
}

public class RandomFactAPI : MonoBehaviour { 
    const string API_URL = "https://uselessfacts.jsph.pl/random.json?language=en";

    public static IEnumerator Grab(System.Action<string> callback) {
        using (UnityWebRequest request = UnityWebRequest.Get(API_URL)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                string response = request.downloadHandler.text;
                string fact = ExtractFactFromJson(response);
                callback?.Invoke(fact);
            } else {
                Debug.LogError($"Error: {request.error}");
                callback?.Invoke(null);
            }
        }
    }

    private static string ExtractFactFromJson(string json) {
        var jsonObject = JObject.Parse(json);
        return jsonObject["text"]?.ToString() ?? "Fact not found.";
    }
}

public class RealTimeStatisticsAPI : MonoBehaviour {
    const string BASE_URL = "https://real-time-statistics.p.rapidapi.com/counters/";
    const string API_KEY = "9d44048bafmshf994a9435256602p142a03jsnf2a44bfb9891";
    const string API_HOST = "real-time-statistics.p.rapidapi.com";

    public static IEnumerator Grab(string endpoint, System.Action<string> callback) {
        string url = $"{BASE_URL}{endpoint}";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            request.SetRequestHeader("X-RapidAPI-Key", API_KEY);
            request.SetRequestHeader("X-RapidAPI-Host", API_HOST);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                string response = request.downloadHandler.text;
                callback?.Invoke(response);
            } else {
                Debug.LogError($"Error: {request.error}");
                callback?.Invoke(null);
            }
        }
    }
}
