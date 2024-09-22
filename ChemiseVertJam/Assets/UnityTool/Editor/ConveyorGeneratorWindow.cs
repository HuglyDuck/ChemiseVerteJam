using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace VekemanSacha
{
    public class ConveyorGeneratorWindow : EditorWindow
    {
        private ConveyorGenerator generator;

        [MenuItem("Tools/Conveyor Generator")]
        public static void ShowWindow()
        {
            GetWindow<ConveyorGeneratorWindow>("Conveyor Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Conveyor Generator", EditorStyles.boldLabel);

            generator = (ConveyorGenerator)EditorGUILayout.ObjectField("Target Generator", generator, typeof(ConveyorGenerator), true);

            if (GUILayout.Button("Create New Conveyor Generator"))
            {
                CreateNewConveyorGenerator();
            }

            if (generator == null)
            {
                EditorGUILayout.HelpBox("Please assign a ConveyorGenerator object or create a new one.", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();

            // Prefabs settings
            GUILayout.Label("Prefabs Settings", EditorStyles.boldLabel);
            generator.footPrefab = (GameObject)EditorGUILayout.ObjectField("Foot Prefab", generator.footPrefab, typeof(GameObject), false);
            generator.beltPrefab = (GameObject)EditorGUILayout.ObjectField("Belt Prefab", generator.beltPrefab, typeof(GameObject), false);

            // Conveyor settings
            GUILayout.Label("Conveyor Settings", EditorStyles.boldLabel);
            generator.pointSpacing = EditorGUILayout.FloatField("Point Spacing", generator.pointSpacing);

            // Belt settings
            GUILayout.Label("Belt Dimensions", EditorStyles.boldLabel);
            generator.beltWidth = EditorGUILayout.FloatField("Belt Width", generator.beltWidth);
            generator.beltHeight = EditorGUILayout.FloatField("Belt Height", generator.beltHeight);
            generator.beltLength = EditorGUILayout.FloatField("Belt Length", generator.beltLength);

            // Foot settings
            GUILayout.Label("Foot Dimensions", EditorStyles.boldLabel);
            generator.footWidth = EditorGUILayout.FloatField("Foot Width", generator.footWidth);
            generator.footLength = EditorGUILayout.FloatField("Foot Length", generator.footLength);
            generator.footPlacementFactor = EditorGUILayout.IntField("Number of feet", generator.footPlacementFactor);

            // Junction settings
            GUILayout.Label("Junction Settings", EditorStyles.boldLabel);
            generator.junctionHeight = EditorGUILayout.FloatField("Junction Height", generator.junctionHeight);
            generator.junctionYOffset = EditorGUILayout.FloatField("Junction Y Offset", generator.junctionYOffset);
            generator.conveyorMaterial = (Material)EditorGUILayout.ObjectField("Conveyor Material", generator.conveyorMaterial, typeof(Material), false);

            EditorGUILayout.Space();

            // Control Points List
            GUILayout.Label("Control Points List", EditorStyles.boldLabel);
            if (GUILayout.Button("Add Control Point"))
            {
                generator.AddToList();
            }

            if (GUILayout.Button("Remove Control Point"))
            {
                generator.RemoveFromList();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Conveyor"))
            {
                generator.GenerateConveyor();
            }

            if (GUILayout.Button("Clear Conveyor"))
            {
                generator.ClearConveyor();
            }
            if (GUILayout.Button("Freeze Conveyor"))
            {
                generator.FreezeConveyor();
            }

        }

        private void CreateNewConveyorGenerator()
        {
            GameObject newGameObject = new GameObject("New Conveyor Generator");
            generator = newGameObject.AddComponent<ConveyorGenerator>();
        }
    }

}
