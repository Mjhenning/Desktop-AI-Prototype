using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour {
  public static CurrencyManager instance;
  public int goopAmount;

  void Awake () {
    instance = this;
  }

  void OnEnable () {
    EventsManager.AddCurrency.AddListener (IncreaseGoop);
    EventsManager.RemoveCurrency.AddListener (DecreaseGoop);
  }

  void DecreaseGoop (int amount) {
    goopAmount -= amount;
  }

  void IncreaseGoop (int amount) {
    goopAmount += amount;
  }

  public bool CheckCurrency (int amount) {
    if (goopAmount < amount) {
      return false;
    } else if (goopAmount>= amount) {
      return true;
    }
    return false;
  }
}
