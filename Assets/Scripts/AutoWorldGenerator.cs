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
    }
}
