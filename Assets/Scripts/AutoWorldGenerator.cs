using UnityEngine;

public class AutoWorldGenerator : MonoBehaviour
{
    private void Start()
    {
        WorldBuilder builder = GetComponent<WorldBuilder>();
        if (builder == null)
        {
            builder = gameObject.AddComponent<WorldBuilder>();
        }
        builder.BuildWorld();

        // Ensure a Main Camera with AudioListener exists after world build
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            cam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
            cam.transform.position = builder.cameraPosition;
            cam.transform.rotation = Quaternion.Euler(builder.cameraRotation);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = builder.fogColor;
        }
        else
        {
            if (cam.gameObject.GetComponent<AudioListener>() == null)
                cam.gameObject.AddComponent<AudioListener>();
        }
    }
}
