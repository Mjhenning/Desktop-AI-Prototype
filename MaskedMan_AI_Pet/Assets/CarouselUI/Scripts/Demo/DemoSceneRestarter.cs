using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CarouselUI.Demo
{
    public class DemoSceneRestarter : MonoBehaviour
    {
        [FormerlySerializedAs ("_preferenceTracker")] [SerializeField] private PreferenceTracker preferenceTracker;
        [FormerlySerializedAs ("_uiCanvas")] [SerializeField] private GameObject uiCanvas;

        public async void ResetScene()
        {
            uiCanvas.SetActive(false);

            preferenceTracker.ResetAllPeferenceToZero(); //RESETS ALL TO ZERO

            await Task.Delay(250);

            uiCanvas.SetActive(true); //RESETS SHOWN VALUES
        }

        public void EndGame()
        {
            Debug.LogError("Quit Game");
            Application.Quit();
        }

    }
}