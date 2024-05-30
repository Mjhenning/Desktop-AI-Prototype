using System.Collections.Generic;
using UnityEngine;

public class API_Prompts : MonoBehaviour {
    static readonly List<string> Prompts = new List<string> {
        "d1ths",
        "current_population",
        "births",
        "tablets_sold",
        "websites_hacked",
        "youtube_videos",
        "spam_emails",
        "google_users",
        "facebook_users",
        "twitter_users",
        "instagram_photos",
        "dth1s_ads",
        "dth1s_cancer",
        "dth1s_malarial",
        "dth1s_cigarettes",
        "dth1s_alchool",
        "dth1s_children",
        "cigarettes_smoked",
        "coal_days",
        "gas_days",
        "oil_days",
        "oil_years"
    };

    static readonly List<string> PromptPrefix = new List<string> {
        "Total estimated deaths ",
        "The current estimated world population",
        "Total estimated births ",
        "Total estimated tablets sold ",
        "Total estimated websites hacked ",
        "Total estimated youtube videos produced ",
        "Total estimated spam emails sent ",
        "Total estimated googles users active ",
        "Total estimated facebook users active ",
        "Total estimated twitter users active ",
        "Total estimated instagram photos posted ",
        "Total estimated deaths due to aids ",
        "Total estimated deaths due to cancer ",
        "Total estimated deaths due to malaria ",
        "Total estimated deaths due to cigarettes ",
        "Total estimated deaths due to alcohol ",
        "Total estimated child deaths ",
        "Total estimated cigarettes smoked ",
        "Total estimated days of coal left",
        "Total estimated days of gas left",
        "Total estimated days of oil left",
        "Total estimated years of oil left"
    };

    public static string GrabAPrompt () {
        int randomIndex = Random.Range (0, Prompts.Count);
        return Prompts[randomIndex];
    }

    public static string ConstructStatResponse (string OgPrompt, string keyword, string num) { //text formatting issues
        int _index = Prompts.IndexOf (OgPrompt);
        float _numbers = float.Parse (num);
        string _formattedString = _numbers.ToString ("N1");
        string _response = PromptPrefix[_index] + keyword + " is " + _formattedString;
        return _response;
    }
}