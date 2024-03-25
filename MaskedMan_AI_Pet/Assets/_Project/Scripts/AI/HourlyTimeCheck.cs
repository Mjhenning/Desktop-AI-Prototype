using System;
using System.Collections;
using UnityEngine;

public class HourlyTimeCheck : MonoBehaviour
{
    void Awake () {
        EventsManager.StartHourlyCheck.AddListener (StartChecks);
    }
    
    void StartChecks()
    {
        // Perform the initial time check
        CheckTime();

        // Start the coroutine to check the time every hour
        StartCoroutine(CheckTimeEveryHour());
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
        DateTime _endTime = DateTime.Today.AddHours(22); // 22:00

        // Check if the current time falls within the range
        bool _shopOpen = _currentTime >= _startTime && _currentTime < _endTime;

        Debug.Log(_shopOpen ? "Current time is between 06:00 and 18:00" : "Current time is not between 06:00 and 18:00");

        // Invoke the event with the boolean value
        EventsManager.CheckShop(_shopOpen);
    }
}
