using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarouselUI.Demo;
using UnityEngine.Serialization;

namespace CarouselUI.Demo
{
    public class CarouselUIProcessorPlayerPrefs : CarouselUIProcessorBase
    {
        [FormerlySerializedAs ("_prefTrackerInstance")] [SerializeField, Tooltip("Tracker gameobject, necessary for updating and tracking.")] private PreferenceTracker prefTrackerInstance;
        [FormerlySerializedAs ("_settingToInfluence")] [SerializeField, Tooltip("Playerpref setting to set by this controller.")] private PreferenceEnum settingToInfluence;

        protected override void UpdateCarouselUI()
        {
            //STORES VALUES ON THIS INSTANCE OF THE SCRIPT, FIRED ONENABLE & WHENEVER CAROUSEL UPDATES
            StoredSettingsIndex = prefTrackerInstance.GetValues(settingToInfluence);

            base.UpdateCarouselUI(); //NEEDED BECAUSE THIS ALLOWS UPDATING OF THE ASSOCIATED CAROUSEL ELEMENT INDEX
        }

        protected override void DetermineOutput(int input)
        {
            //THIS OUTPUTS THE NEW VALUE FROM THE CAROUSEL
            prefTrackerInstance.SetValues(settingToInfluence, input);

            //BASE VERSION NOT REFERENCED HERE AS IT ONLY CONTAINS A PRINT
        }
    }
}