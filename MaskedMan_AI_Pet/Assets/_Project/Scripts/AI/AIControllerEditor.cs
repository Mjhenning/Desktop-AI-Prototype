#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIController))]
public class AIControllerEditor : Editor {
    [SerializeField] private StateType selectStateToTest;  // Ensure it is serialized

    public override void OnInspectorGUI() {
        // Draw the default inspector
        DrawDefaultInspector();

        // Draw the enum dropdown
        selectStateToTest = (StateType)EditorGUILayout.EnumPopup("Select State to Test", selectStateToTest);

        // Add the button and handle the state change
        if (GUILayout.Button("Test Selected State")) {
            switch (selectStateToTest) {
                case StateType.Idle:
                    AIController.Instance.ChangeState(new StateIdle(AIController.Instance));
                    break;
                case StateType.ShopPrompt:
                    AIController.Instance.ChangeState(new StateShopPrompt(AIController.Instance));
                    break;
                case StateType.Sleep:
                    AIController.Instance.ChangeState(new StateSleep(AIController.Instance));
                    break;
                case StateType.Agressive:
                    AIController.Instance.ChangeState(new StateAgressive(AIController.Instance));
                    break;
            }
        }
    }
}
#endif