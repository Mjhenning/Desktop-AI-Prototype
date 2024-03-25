using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CarouselUI
{
    public abstract class CarouselUIProcessorBase : MonoBehaviour
    {
        [FormerlySerializedAs ("_associatedCarousel")] [SerializeField, Tooltip("Carousel script that this processor tracks.")] private CarouselUIElement associatedCarousel;

        protected int StoredSettingsIndex;

        private void Start()
        {
            //TRIES TO OBTAIN CAROUSEL SCRIPT
            if (TryGetComponent<CarouselUIElement>(out var _element))
            { associatedCarousel = _element; }
            else
            { Debug.LogError($"Could not find CarouselUIElement on {this.gameObject.name}. Fix this."); }

            //SUBSCRIBE TO EVENTS
            associatedCarousel.InputEvent += OnInputDetected;
        }

        private void OnEnable() //WHEN GAMEOBJECT IS ENABLED INDICES FOR CAROUSELS ARE IMMEDIATELY UPDATED!
        {
            UpdateCarouselUI();
        }

        private void OnInputDetected() //FIRED ON EVENT
        {
            DetermineOutput(associatedCarousel.CurrentIndex);
        }

        /// <summary>
        /// Forces associated carousel element to update itself to the appropriate setting. When using inheritance, make sure base.UpdateCarouselUI() is placed AFTER custom code.
        /// </summary>
        protected virtual void UpdateCarouselUI()
        {
            //PROCESSING HAPPENS HERE

            if (associatedCarousel.CurrentIndex != StoredSettingsIndex)
            {
                associatedCarousel.UpdateIndex(StoredSettingsIndex);
            }
        }

        /// <summary>
        /// Determines result of carousel update. When using inheritance, there is no need to use base.DetermineOutput().
        /// </summary>
        /// <param name="input">Integer of setting.</param>
        protected virtual void DetermineOutput(int input)
        {
            print($"Input for {this.gameObject.name} is {input}");
        }

    }
}