using UnityEditor;
using UnityEngine;

namespace MazeGeneration
{
    public class MazeGeneratorEditorWindow : EditorWindow
    {
        [SerializeField] private float wallSize = 6;

        [SerializeField] private int min = 1;
        [SerializeField] private int max = 10;
        [SerializeField] private int seedVal = 0;

        private int mazeRows = 10;
        private int mazeColumns = 10;

        private GameObject wallPrefab;
        private MazeLoader mazeGenerator;
        private GameObject parentObj;

        [MenuItem("Window/Maze Generator")]
        public static void ShowWindow()
        {
            GetWindow<MazeGeneratorEditorWindow>("Maze Generator");
        }

        private void OnGUI()
        {
            SetupGUI();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Maze", GUILayout.Width(200)))
            {
                if (parentObj == null)
                {
                    parentObj = new GameObject("MazeParent");
                }

                // Set the parameters and generate the maze.
                mazeGenerator.SetupMaze(mazeRows, mazeColumns, wallSize);
                mazeGenerator.CreateMaze(parentObj.transform, seedVal);

            }

            if (GUILayout.Button("Clear Maze", GUILayout.Width(200)))
            {
                DestroyImmediate(parentObj);
            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        private void SetupGUI()
        {
            HeaderSetup();
            PrefabSetting();
            MazeDimensionsSetup();
            SeedSettingSetup();

            if (mazeGenerator == null)
            {
                mazeGenerator = new MazeLoader(mazeRows, mazeColumns, wallPrefab);
            }

        }

        private void SeedSettingSetup()
        {
            //  Seed
            EditorGUILayout.LabelField("Pre-seed Setup", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Random Seed", GUILayout.Width(200)))
            {
                seedVal = UnityEngine.Random.Range(0, 10000);
            }

            seedVal = EditorGUILayout.IntField("Seed", seedVal);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void MazeDimensionsSetup()
        {
            // Rows * Columns
            EditorGUILayout.LabelField("Maze Size", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            // min and max values for slider used in creation of maze size
            min = EditorGUILayout.IntField("Min Value(Rows/Columns): ", min);
            max = EditorGUILayout.IntField("Max Value(Rows/Columns): ", max);
            // slider value to set rows and cols of the maze
            mazeRows = EditorGUILayout.IntSlider("Maze Rows: ", mazeRows, min, max);
            mazeColumns = EditorGUILayout.IntSlider("Maze Columns: ", mazeColumns, min, max);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        private void PrefabSetting()
        {
            // Prefabs - Walls / Floors
            wallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Wall.prefab");
            EditorGUILayout.LabelField("Maze Prefabs", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("Box");
            wallPrefab = EditorGUILayout.ObjectField("Wall Prefab", wallPrefab, typeof(GameObject), true) as GameObject;
            // change size of the wall prefab ( using same value for both x and y)
            wallSize = EditorGUILayout.FloatField("Wall Size: ", wallSize);
            wallPrefab.transform.localScale = new Vector3(wallSize, wallSize, wallSize / 10f);
            GUILayout.EndVertical();
        }

        private static void HeaderSetup()
        {
            // header
            GUILayout.BeginVertical();

            GUIStyle centeredBoldLabel = new GUIStyle(GUI.skin.label);
            centeredBoldLabel.alignment = TextAnchor.MiddleCenter;
            centeredBoldLabel.fontStyle = FontStyle.Bold;
            centeredBoldLabel.fontSize = 16;

            GUILayout.Label("Maze Generator", centeredBoldLabel, GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
        }
    }
}