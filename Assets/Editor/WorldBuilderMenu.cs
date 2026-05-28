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

    [MenuItem("Tools/Isekai Virtual World/Complete Setup (World + Particles + Camera)")]
    private static void CompleteSetup()
    {
        CreateVirtualSpace();

        WorldBuilder builder = Object.FindObjectOfType<WorldBuilder>();
        if (builder != null)
        {
            // Add particle effects
            ParticleEffectSpawner spawner = builder.gameObject.AddComponent<ParticleEffectSpawner>();
            spawner.SpawnParticles();

            // Setup camera with controller
            Camera camera = Camera.main;
            if (camera != null)
            {
                camera.gameObject.AddComponent<CameraController>();
                Debug.Log("カメラコントローラーを追加しました。矢印キーやマウススクロールで視点移動できます。");
            }

            EditorSceneManager.MarkSceneDirty(builder.gameObject.scene);
        }

        Debug.Log("異世界仮想空間の完全セットアップが完了しました。");
    }

    [MenuItem("Tools/Isekai Virtual World/Add Auto-Generation")]
    private static void AddAutoGeneration()
    {
        GameObject setupObject = Object.FindObjectOfType<AutoWorldGenerator>()?.gameObject;
        if (setupObject == null)
        {
            setupObject = new GameObject("Isekai Auto Setup");
            setupObject.AddComponent<AutoWorldGenerator>();
            setupObject.AddComponent<WorldBuilder>();
            setupObject.AddComponent<ParticleEffectSpawner>();
            Debug.Log("オブジェクト 'Isekai Auto Setup' を作成しました。このオブジェクトはシーン起動時に自動的に世界を生成します。");
        }

        EditorSceneManager.MarkSceneDirty(setupObject.scene);
    }
}
