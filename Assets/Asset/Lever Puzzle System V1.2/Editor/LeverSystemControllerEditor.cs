using UnityEditor;
using UnityEngine;

namespace LeverSystem
{
    [CustomEditor(typeof(LeverSystemController))]
    public class LeverSystemControllerEditor : Editor
    {
        #region SerializedProperties
        SerializedProperty leverOrder;
        SerializedProperty pullLimit;
        SerializedProperty pullTimer;
        SerializedProperty interactiveObjects;

        SerializedProperty readySwitch;
        SerializedProperty limitReachedSwitch;
        SerializedProperty acceptedSwitch;
        SerializedProperty resettingSwitch;

        SerializedProperty readyLight;
        SerializedProperty limitReachedLight;
        SerializedProperty acceptedLight;
        SerializedProperty resettingLight;

        SerializedProperty testButton;
        SerializedProperty resetButton;

        SerializedProperty switchOnName;
        SerializedProperty switchOffName;
        SerializedProperty redButtonName;

        SerializedProperty switchPullSound;
        SerializedProperty switchFailSound;
        SerializedProperty switchDoorSound;

        SerializedProperty LeverPower;
        #endregion

        bool animatedSwitchesGroup, controlLightsGroup, confirmButtonsGroup, animationNamesGroup, soundEffectsGroup, eventGroup = false;

        private void OnEnable()
        {
            #region OnEnable Properties to find
            leverOrder = serializedObject.FindProperty(nameof(leverOrder));
            pullLimit = serializedObject.FindProperty(nameof(pullLimit));
            pullTimer = serializedObject.FindProperty(nameof(pullTimer));
            interactiveObjects = serializedObject.FindProperty(nameof(interactiveObjects));

            readySwitch = serializedObject.FindProperty(nameof(readySwitch));
            limitReachedSwitch = serializedObject.FindProperty(nameof(limitReachedSwitch));
            acceptedSwitch = serializedObject.FindProperty(nameof(acceptedSwitch));
            resettingSwitch = serializedObject.FindProperty(nameof(resettingSwitch));

            readyLight = serializedObject.FindProperty(nameof(readyLight));
            limitReachedLight = serializedObject.FindProperty(nameof(limitReachedLight));
            acceptedLight = serializedObject.FindProperty(nameof(acceptedLight));
            resettingLight = serializedObject.FindProperty(nameof(resettingLight));

            testButton = serializedObject.FindProperty(nameof(testButton));
            resetButton = serializedObject.FindProperty(nameof(resetButton));

            switchOnName = serializedObject.FindProperty(nameof(switchOnName));
            switchOffName = serializedObject.FindProperty(nameof(switchOffName));
            redButtonName = serializedObject.FindProperty(nameof(redButtonName));

            switchPullSound = serializedObject.FindProperty(nameof(switchPullSound));
            switchFailSound = serializedObject.FindProperty(nameof(switchFailSound));
            switchDoorSound = serializedObject.FindProperty(nameof(switchDoorSound));

            LeverPower = serializedObject.FindProperty(nameof(LeverPower));
            #endregion
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((LeverSystemController)target), typeof(LeverSystemController), false);
            GUI.enabled = true;

            EditorGUILayout.Space(5);

            LeverSystemController _leverSystemController = (LeverSystemController)target;

            EditorGUILayout.LabelField("Custom Order", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(leverOrder);

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("Lever Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(pullLimit);
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(pullTimer);

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Lever & Button Interactive References", EditorStyles.toolbarTextField);
            EditorGUILayout.PropertyField(interactiveObjects);

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Control Box GameObject References", EditorStyles.toolbarTextField);

            EditorGUILayout.Space(5);

            ControlBoxItems();

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Animation Fields", EditorStyles.toolbarTextField);

            AnimationFields();

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Sound References", EditorStyles.toolbarTextField);

            SoundReferences();

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Unity Event", EditorStyles.toolbarTextField);

            PowerUpEvent();

            EditorGUILayout.Space(10);

            OpenEditorScript();

            serializedObject.ApplyModifiedProperties();
        }

        void ControlBoxItems()
        {
            animatedSwitchesGroup = EditorGUILayout.BeginFoldoutHeaderGroup(animatedSwitchesGroup, "Switches");
            if (animatedSwitchesGroup)
            {
                EditorGUILayout.PropertyField(readySwitch);
                EditorGUILayout.PropertyField(limitReachedSwitch);
                EditorGUILayout.PropertyField(acceptedSwitch);
                EditorGUILayout.PropertyField(resettingSwitch);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space(5);

            controlLightsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(controlLightsGroup, "Lights");
            if (controlLightsGroup)
            {
                EditorGUILayout.PropertyField(readyLight);
                EditorGUILayout.PropertyField(limitReachedLight);
                EditorGUILayout.PropertyField(acceptedLight);
                EditorGUILayout.PropertyField(resettingLight);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space(5);

            confirmButtonsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(confirmButtonsGroup, "Buttons");
            if (confirmButtonsGroup)
            {
                EditorGUILayout.PropertyField(testButton);
                EditorGUILayout.PropertyField(resetButton);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void AnimationFields()
        {
            animationNamesGroup = EditorGUILayout.BeginFoldoutHeaderGroup(animationNamesGroup, "Animation Names");
            if (animationNamesGroup)
            {
                EditorGUILayout.PropertyField(switchOnName);
                EditorGUILayout.PropertyField(switchOffName);
                EditorGUILayout.PropertyField(redButtonName);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void SoundReferences()
        {
            soundEffectsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(soundEffectsGroup, "Sound Effects");
            if (soundEffectsGroup)
            {
                EditorGUILayout.PropertyField(switchPullSound);
                EditorGUILayout.PropertyField(switchFailSound);
                EditorGUILayout.PropertyField(switchDoorSound);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void PowerUpEvent()
        {
            eventGroup = EditorGUILayout.BeginFoldoutHeaderGroup(eventGroup, "Power Up Event");
            if (eventGroup)
            {
                EditorGUILayout.PropertyField(LeverPower);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        void OpenEditorScript()
        {
            if (GUILayout.Button("Open Editor Script"))
            {
                string scriptFilePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(scriptFilePath));
            }
        }
    }
}
