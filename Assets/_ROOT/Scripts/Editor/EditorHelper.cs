#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class EditorHelper
    {
        [MenuItem("Game/Easy Obby - Create Stage")]
        private static void EasyObbyStagePrefab()
        {
            string path = "Assets/_ROOT/Prefabs/EasyObby/Stage";

            int index = 0;

            // Keep track of the currently selected GameObject(s)
            GameObject[] objectArray = Selection.gameObjects;

            // Loop through every GameObject in the array above
            foreach (GameObject gameObject in objectArray)
            {
                if (gameObject.name.Contains("Checkpoint"))
                    continue;

                // Create folder Prefabs and set the path as within the Prefabs folder,
                // and name it as the GameObject's name with the .Prefab format
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (gameObject.GetComponent<EasyObby_Stage>() == null)
                    gameObject.AddComponent<EasyObby_Stage>();

                gameObject.transform.localPosition = Vector3.zero;

                //string localPath = "Assets/Prefabs/" + gameObject.name + ".prefab";
                string localPath = $"{path}/EasyObby_Stage{index + 1}.prefab";

                // Make sure the file name is unique, in case an existing Prefab has the same name.
                //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                PrefabUtility.SaveAsPrefabAsset(gameObject, localPath);

                index++;
            }
        }
    }
}
#endif