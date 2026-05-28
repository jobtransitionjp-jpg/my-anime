using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Immediately create the world and audio in edit mode as well.
        CreateVirtualSpace();

        EditorSceneManager.MarkSceneDirty(setupObject.scene);
    }

    [MenuItem("Tools/Isekai Virtual World/Create Final Isekai Scene")]
    private static void CreateFinalIsekaiScene()
    {
        CreateVirtualSpace();
        EnsureMainCameraItem();

        string sceneFolder = "Assets/Scenes";
        if (!Directory.Exists(sceneFolder))
            Directory.CreateDirectory(sceneFolder);

        string finalScenePath = Path.Combine(sceneFolder, "FinalIsekaiScene.unity");
        SceneAsset existingScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(finalScenePath);

        if (existingScene != null)
        {
            Debug.Log("FinalIsekaiScene.unity を上書き保存します。");
        }
        else
        {
            Debug.Log("FinalIsekaiScene.unity を作成して保存します。");
        }

        Scene currentScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(currentScene, finalScenePath);
        AssetDatabase.Refresh();

        Debug.Log("Final Isekai Scene を保存しました: " + finalScenePath);
    }

    private static void EnsureMainCameraItem()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            camera.transform.position = new Vector3(-30f, 18f, -24f);
            camera.transform.rotation = Quaternion.Euler(20f, 35f, 0f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = RenderSettings.fog ? RenderSettings.fogColor : new Color(0.03f, 0.02f, 0.05f);
        }
        else if (camera.gameObject.GetComponent<AudioListener>() == null)
        {
            camera.gameObject.AddComponent<AudioListener>();
        }
    }
}
