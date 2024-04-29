using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour {
  public int goopAmount;

  void OnEnable () {
    EventsManager.AddCurrency.AddListener (IncreaseGoop);
    EventsManager.RemoveCurrency.AddListener (DecreaseGoop);
  }

  void DecreaseGoop (int amount) {
    goopAmount =- amount;
  }

  void IncreaseGoop (int amount) {
    goopAmount += amount;
  }
}
