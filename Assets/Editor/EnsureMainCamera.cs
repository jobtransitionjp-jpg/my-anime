using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class EnsureMainCamera
{
    [MenuItem("Tools/Isekai Virtual World/Ensure Main Camera")]
    private static void Ensure()
    {
        Camera cam = Camera.main;
        GameObject camObj = null;

        if (cam == null)
        {
            camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            cam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();

            cam.transform.position = new Vector3(-30f, 18f, -24f);
            cam.transform.rotation = Quaternion.Euler(20f, 35f, 0f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = RenderSettings.fog ? RenderSettings.fogColor : new Color(0.03f, 0.02f, 0.05f);

            Debug.Log("Main Camera を作成し、AudioListener を追加しました。");
        }
        else
        {
            camObj = cam.gameObject;
            AudioListener al = camObj.GetComponent<AudioListener>();
            if (al == null)
            {
                camObj.AddComponent<AudioListener>();
                Debug.Log("AudioListener を Main Camera に追加しました。");
            }
        }

        if (camObj != null && camObj.GetComponent<CameraController>() == null)
            camObj.AddComponent<CameraController>();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Selection.activeGameObject = camObj;
    }
}
