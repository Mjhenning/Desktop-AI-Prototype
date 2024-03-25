using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

//PUBLIC SCRIPT

namespace CarouselUI
{
    public class CarouselUIElement : MonoBehaviour
    {
        [FormerlySerializedAs ("_optionsObjects")]
        [Header("Carousel Members")]
        [SerializeField, Tooltip("Array containing gameobjects used for options.")] private GameObject[] optionsObjects;

        [FormerlySerializedAs ("_nextButton")] [SerializeField, Tooltip("Button that increments index by 1.")] private GameObject nextButton;

        [FormerlySerializedAs ("_prevButton")] [SerializeField, Tooltip("Button that decrements index by 1.")] private GameObject prevButton;

        [FormerlySerializedAs ("_resetDuration")]
        [Header("Settings")]
        [SerializeField, Tooltip("Time to deactivate inbetween refires.")] private float resetDuration = 0.1f;
        [FormerlySerializedAs ("_doesNotCycleBack")] [SerializeField, Tooltip("If true, when the index reaches either limit the next/previous buttons are hidden.")] private bool doesNotCycleBack = false;

        private int currentIndex = 0;

        public int CurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        public delegate void InputDetected();
        public event InputDetected InputEvent = delegate { };

        private bool isProcessing = false; //HERE TO DELAY REFIRES
        private WaitForSeconds resetDelay; //WORKS WITH DELAY COROUTINE

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (optionsObjects.Length == 0 || optionsObjects == null) //ERROR IF THE OPTIONS ARRAY IS EMPTY
            {
                Debug.LogError($"Carousel UI at {this.gameObject.name} has incomplete options array. Please fix.");

                return;
            }

            resetDelay = new WaitForSeconds(resetDuration);

            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (GameObject _text in optionsObjects)
            {
                _text.SetActive(false); //IF ONE OF THE OPTIONS IS NULL IT WILL CREATE AN ERROR HERE
            }

            optionsObjects[currentIndex].SetActive(true);

            if (doesNotCycleBack && currentIndex == optionsObjects.Length - 1)
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

            if (doesNotCycleBack && currentIndex == optionsObjects.Length - 1)
            {
                return;
            }

            StartCoroutine(LockoutDelay());

            if (currentIndex < (optionsObjects.Length - 1))
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
                currentIndex = (optionsObjects.Length - 1);

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
