using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour { //script in charge of updating goop currency in the front and back-end, aswell as saving and loading this currency as a playerpref
    public static CurrencyManager instance;
    int min = 0;
    int max = 999999999;
    public int goopAmount;

    public TextMeshProUGUI textObj;
    public TextMeshProUGUI PopupObj;
    private const string GoopAmountKey = "GoopAmount";

    void Awake () {
        instance = this;
        LoadGoopAmount();
    }

    void OnEnable () {
        EventsManager.AddCurrency.AddListener (IncreaseGoop);
        EventsManager.RemoveCurrency.AddListener (DecreaseGoop);
    }

    void DecreaseGoop (int amount) {
        goopAmount -= amount;
        goopAmount = Mathf.Clamp(goopAmount, min, max);  // Clamp as an integer
        UpdateCurrencyText();
        DisplayPopup (false,amount);
        SaveGoopAmount();
    }

    void IncreaseGoop (int amount) {
        goopAmount += amount;
        goopAmount = Mathf.Clamp(goopAmount, min, max);  // Clamp as an integer
        UpdateCurrencyText();
        DisplayPopup (true,amount);
        SaveGoopAmount();
    }

    public bool CheckCurrency (int amount) {
        return goopAmount >= amount;
    }

    void UpdateCurrencyText () {
        textObj.text = goopAmount.ToString("N0");  // Format as a string with commas
    }

    void SaveGoopAmount() {
        PlayerPrefs.SetInt(GoopAmountKey, goopAmount);
        PlayerPrefs.Save();
    }

    void LoadGoopAmount() {
        if (PlayerPrefs.HasKey(GoopAmountKey)) {
            goopAmount = PlayerPrefs.GetInt(GoopAmountKey);
        } else {
            goopAmount = 0;  // Default value if no saved data
        }
        UpdateCurrencyText();
    }

    void DisplayPopup (bool positive, int value) { //shows temporary popup of last amount of goop added/lost
        switch (positive) {
            case true:
                PopupObj.text = "+ " + value.ToString ("N0");
                StartCoroutine(ActiveDeactivePopup ());
                break;
            case false:
                PopupObj.text = "- " + value.ToString ("N0");
                StartCoroutine(ActiveDeactivePopup ());
                break;
        }
    }

    IEnumerator ActiveDeactivePopup () {
        PopupObj.gameObject.SetActive (true);
        yield return new WaitForSeconds(3f);
        PopupObj.gameObject.SetActive (false);
    }
}