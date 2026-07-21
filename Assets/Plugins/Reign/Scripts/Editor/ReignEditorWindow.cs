using Reign.Generic;
using Reign.Utility;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Reign.Editor
{
    public class ReignEditorWindow : EditorWindow
    {
        bool tryAddReign = true;
        bool tryCreateCam = true;

        [MenuItem("Reign/Reign Service")]
        public static void ShowWindow()
        {
            GetWindow<ReignEditorWindow>($"Reign Service");
        }

        private void OnGUI()
        {
            string contributors = "";

            for (int i = 0; i < ReignServiceDetails.CONTRIBUTORS.Count; ++i)
            {
                if (i <= 0)
                {
                    contributors += $"{ReignServiceDetails.CONTRIBUTORS.ElementAt(i)}";
                }
                else
                {
                    contributors += $", {ReignServiceDetails.CONTRIBUTORS.ElementAt(i)}";
                }
            }

            GUILayout.Label("Reign Details", EditorStyles.whiteLargeLabel);

            GUILayout.Label($"Version: {ReignServiceDetails.REIGN_VERSION}\nTools Version: {ReignServiceDetails.REIGN_TOOLS_VERSION}\nDate: {ReignServiceDetails.RELEASE_DATE}\nContributors: {contributors}", EditorStyles.label);

            GUILayout.Space(20);

            GUILayout.Label($"Reign Tools", EditorStyles.whiteLargeLabel);

            tryAddReign = GUILayout.Toggle(tryAddReign, "Try Add Reign Prefab");
            tryCreateCam = GUILayout.Toggle(tryCreateCam, "Try Add Camera");

            GUILayout.Space(5);
            if (GUILayout.Button($"Add Reign Dependencies To Current Scene"))
            {
                SetupScene();
            }
        }

        private void SetupScene()
        {
            if (tryAddReign)
            {
                if (FindAnyObjectByType<Reign>())
                {
                    Debug.LogWarning("Reign prefab was already present in the scene.");
                }
                else
                {
                    ReignUtility.InstantiatePrefabInEditor("Assets/Plugins/Reign/Prefabs/Reign.prefab");
                }
            }

            if (tryCreateCam)
            {
                Camera[] cameras = FindObjectsByType<Camera>();

                if (cameras.Length == 0)
                {
                    var newCam = new GameObject("Camera").AddComponent<Camera>();
                    newCam.name = "Camera";
                    newCam.GetUniversalAdditionalCameraData().renderPostProcessing = true;
                }

                foreach (Camera cam in cameras)
                {
                    cam.GetUniversalAdditionalCameraData().renderPostProcessing = true;
                }
            }
        }
    }
}
