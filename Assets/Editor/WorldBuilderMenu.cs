using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class WorldBuilderMenu
{
    [MenuItem("Tools/Isekai Virtual World/Create Virtual Space")]
    private static void CreateVirtualSpace()
    {
        WorldBuilder builder = Object.FindObjectOfType<WorldBuilder>();
        if (builder == null)
        {
            GameObject builderObject = new GameObject("Isekai World Builder");
            builder = builderObject.AddComponent<WorldBuilder>();
        }

        builder.BuildWorld();
        EditorSceneManager.MarkSceneDirty(builder.gameObject.scene);
        Selection.activeGameObject = builder.gameObject;
        Debug.Log("異世界仮想空間を作成しました。Sceneに 'Isekai World Builder' を確認してください。");
    }
}
