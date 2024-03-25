using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace CarouselUI.Demo
{
    public class PlayerPrefDisplayer : MonoBehaviour
    {
        [FormerlySerializedAs ("_canvasGroup")] [SerializeField] private CanvasGroup canvasGroup;
        [FormerlySerializedAs ("_preferenceToDisplay")] [SerializeField] private PreferenceEnum preferenceToDisplay;
        [FormerlySerializedAs ("_displayText")] [SerializeField] private TextMeshProUGUI displayText;

        private Coroutine fadeOut;

        private void OnEnable()
        {
            RefreshValue();
        }

        public void RefreshValue()
        {
            displayText.text = $"{PlayerPrefs.GetInt(preferenceToDisplay.ToString())}";

            if(fadeOut != null)
            {
                StopCoroutine(fadeOut);
            }

            fadeOut = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(3);

            canvasGroup.alpha = 0;

            yield break;
        }
    }
}