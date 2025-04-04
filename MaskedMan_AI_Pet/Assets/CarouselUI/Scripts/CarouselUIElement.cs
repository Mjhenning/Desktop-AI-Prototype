using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//PUBLIC SCRIPT

namespace CarouselUI
{
    public class CarouselUIElement : MonoBehaviour
    {
        [FormerlySerializedAs ("_optionsObjects")]
        [Header("Carousel Members")]
        [SerializeField, Tooltip("Array containing gameobjects used for options.")] public List<GameObject> optionsObjects = new List<GameObject>();

        [FormerlySerializedAs ("_nextButton")] [SerializeField, Tooltip("Button that increments index by 1.")] private GameObject nextButton;

        [FormerlySerializedAs ("_prevButton")] [SerializeField, Tooltip("Button that decrements index by 1.")] private GameObject prevButton;

        [FormerlySerializedAs ("_resetDuration")]
        [Header("Settings")]
        [SerializeField, Tooltip("Time to deactivate inbetween refires.")] private float resetDuration = 0.1f;
        [FormerlySerializedAs ("_doesNotCycleBack")] [SerializeField, Tooltip("If true, when the index reaches either limit the next/previous buttons are hidden.")] private bool doesNotCycleBack = false;

        public int currentIndex = 0;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        public delegate void InputDetected();
        public event InputDetected InputEvent = delegate { };

        private bool isProcessing = false; //HERE TO DELAY REFIRES
        private WaitForSeconds resetDelay; //WORKS WITH DELAY COROUTINE


        void OnEnable () { //on-enable so that it detects index change while inactive properly and android section so that swipes will work and player doesn't have to press buttons
#if UNITY_ANDROID
            EventsManager.OnSwipeDirection.AddListener (EvaluateSwipe);
#endif
            UpdateUI ();
        }

        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (optionsObjects.Count == 0 || optionsObjects == null) //ERROR IF THE OPTIONS ARRAY IS EMPTY
            {
                Debug.LogError($"Carousel UI at {this.gameObject.name} has incomplete options array. Please fix.");

                return;
            }

            resetDelay = new WaitForSeconds(resetDuration);

            //Custom Edit for android compatibility and tp
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            UpdateUI();
            
#elif UNITY_ANDROID //de-activates both buttons if played on android
            nextButton.GetComponent<Button> ().interactable = false;
            prevButton.GetComponent<Button> ().interactable = false;
#endif


        }

        private void UpdateUI()
        {
            foreach (GameObject _text in optionsObjects)
            {
                _text.SetActive(false); //IF ONE OF THE OPTIONS IS NULL IT WILL CREATE AN ERROR HERE
            }

            optionsObjects[currentIndex].SetActive(true);

            if (doesNotCycleBack && currentIndex == optionsObjects.Count - 1)
            {
                nextButton.SetActive(false);
            }
            else
            {
                nextButton.SetActive(true);
            }

            if (doesNotCycleBack && currentIndex == 0)
            {
                prevButton.SetActive(false);
            }
            else
            {
                prevButton.SetActive(true);
            }

        }

#if UNITY_ANDROID //used to grab swipe direction and press next / previous according to that direction
        public void EvaluateSwipe (DraggedDirection direction) {
            switch (direction) {
                case DraggedDirection.Right:
                    PressNext ();
                    break;
                case DraggedDirection.Left:
                    PressPrevious ();
                    break;
            }
        }
#endif

        /// <summary>
        /// Prevents further refires until duration ends.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LockoutDelay()
        {
            isProcessing = true; //PREVENTS BUTTON MASHING

            yield return resetDelay;

            isProcessing = false;

            yield break;
        }

        //METHOD ACCESSED BY NEXT BUTTON
        public void PressNext()
        {
            if (isProcessing)
            {
                return;
            }

            if (doesNotCycleBack && currentIndex == optionsObjects.Count - 1)
            {
                return;
            }

            StartCoroutine(LockoutDelay());

            if (currentIndex < (optionsObjects.Count - 1))
            {
                currentIndex += 1;

                UpdateUI();
            }
            else
            {
                currentIndex = 0;

                UpdateUI();
            }

            InputEvent?.Invoke();
        }

        //METHOD ACCESSED BY PREVIOUS BUTTON
        public void PressPrevious()
        {
            if (isProcessing)
            {
                return;
            }

            if (doesNotCycleBack && currentIndex == 0)
            {
                return;
            }

            StartCoroutine(LockoutDelay());

            if (currentIndex > 0)
            {
                currentIndex -= 1;

                UpdateUI();
            }
            else
            {
                currentIndex = (optionsObjects.Count - 1);

                UpdateUI();
            }

            InputEvent?.Invoke();
        }

        /// <summary>
        /// Used by an associated processor to update the index of this carousel.
        /// </summary>
        /// <param name="input"></param>
        public void UpdateIndex(int input)
        {
            currentIndex = input;
            UpdateUI();
        }
    }
}
