using System;
using System.Collections;
using UnityEngine;

public class Timers : MonoBehaviour {

    Coroutine aggressiveTimerCo;
    Coroutine checkAgroSwapCo;
    void Awake () {
        EventsManager.StartHourlyCheck.AddListener (StartChecks);
        EventsManager.MaskClicked.AddListener (RestartCall);
        EventsManager.BodyClicked.AddListener (RestartCall);
        EventsManager.TieClicked.AddListener (RestartCall);
    }

    void StartChecks()
    {
        // Perform the initial time check
        CheckTime();

        // Start the coroutine to check the time every hour
        StartCoroutine(CheckTimeEveryHour());

       aggressiveTimerCo = StartCoroutine (AggressiveTimer ());
    }

    IEnumerator CheckTimeEveryHour()
    {
        while (true)
        {
            // Calculate the time until the next hour
            DateTime _nextHour = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);
            TimeSpan _timeUntilNextHour = _nextHour - DateTime.Now;

            // Wait until the next hour
            yield return new WaitForSeconds((float)_timeUntilNextHour.TotalSeconds);

            // Perform the time check
            CheckTime();
        }
    }

    void CheckTime()
    {
        // Get the current system time
        DateTime _currentTime = DateTime.Now;

        // Define the start and end times for the desired range
        DateTime _startTime = DateTime.Today.AddHours(6); // 06:00
        DateTime _endTime = DateTime.Today.AddHours(22); // 00:00

        // Check if the current time falls within the range
        bool _shopOpen = _currentTime >= _startTime && _currentTime < _endTime;

        Debug.Log(_shopOpen ? "Current time is between 06:00 and 22:00" : "Current time is not between 06:00 and 22:00");

        // Invoke the event with the boolean value
        EventsManager.CheckShop(_shopOpen);
    }

    IEnumerator AggressiveTimer () {

        yield return new WaitForSeconds (300f);
        checkAgroSwapCo = StartCoroutine(CheckAggressiveSwap ());
    }

    IEnumerator CheckAggressiveSwap () {
        
        // Wait until the current state is no longer Shopping or Sleep
        while (AIController.Instance.GetCurrentState() == StateType.Shopping || AIController.Instance.GetCurrentState() == StateType.Sleep)
        {
            yield return new WaitForSeconds(1); // Wait until the next frame
        }

        // Change to aggressive state
        AIController.Instance.ChangeState(AIController.Instance.stateAgressive);


        // Wait until the current state is no longer aggressive
        while (AIController.Instance.GetCurrentState() == StateType.Agressive)
        {
            yield return null; // Wait until the next frame
        }

        // Restart the aggressive timer once the state is no longer aggressive
        StartCoroutine(AggressiveTimer());
        
    }

    void RestartCall () {
        StartCoroutine (RestartAgroTimers ());
    }
    
    IEnumerator RestartAgroTimers() {
        Debug.Log ("Restaring Timers");
        
        if (aggressiveTimerCo != null)
        {
            StopCoroutine(aggressiveTimerCo);
            aggressiveTimerCo = null;
        }
        if (checkAgroSwapCo != null)
        {
            StopCoroutine(checkAgroSwapCo);
            checkAgroSwapCo = null;
        }

        yield return new WaitForSeconds(1);

        aggressiveTimerCo = StartCoroutine(AggressiveTimer());
    }
}
