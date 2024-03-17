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
            DateTime nextHour = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);
            TimeSpan timeUntilNextHour = nextHour - DateTime.Now;

            // Wait until the next hour
            yield return new WaitForSeconds((float)timeUntilNextHour.TotalSeconds);

            // Perform the time check
            CheckTime();
        }
    }

    void CheckTime()
    {
        // Get the current system time
        DateTime currentTime = DateTime.Now;

        // Define the start and end times for the desired range
        DateTime startTime = DateTime.Today.AddHours(6); // 06:00
        DateTime endTime = DateTime.Today.AddHours(18); // 18:00

        // Check if the current time falls within the range
        bool shopOpen = currentTime >= startTime && currentTime < endTime;

        Debug.Log(shopOpen ? "Current time is between 06:00 and 18:00" : "Current time is not between 06:00 and 18:00");

        // Invoke the event with the boolean value
        EventsManager.CheckShop(shopOpen);
    }
}
