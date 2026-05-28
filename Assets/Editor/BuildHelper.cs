using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildHelper
{
    [MenuItem("Tools/Isekai Virtual World/Configure Build Settings")]
    private static void ConfigureBuildSettings()
    {
        string scenePath = "Assets/Scenes/FinalIsekaiScene.unity";
        EditorBuildSettingsScene[] buildScenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };
        EditorBuildSettings.scenes = buildScenes;
        Debug.Log("Build settings configured with scene: " + scenePath);
    }

    [MenuItem("Tools/Isekai Virtual World/Build Standalone OSX")]
    private static void BuildStandaloneOSX()
    {
        ConfigureBuildSettings();
        string outputPath = "Build/Standalone/FinalIsekai.app";
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/FinalIsekaiScene.unity" },
            locationPathName = outputPath,
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.None
        };
        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result == BuildResult.Succeeded)
            Debug.Log("Standalone OSX build succeeded: " + outputPath);
        else
            Debug.LogError("Standalone OSX build failed: " + report.summary.result);
    }

    [MenuItem("Tools/Isekai Virtual World/Build Standalone Windows64")]
    private static void BuildStandaloneWindows()
    {
        ConfigureBuildSettings();
        string outputPath = "Build/Standalone/FinalIsekai.exe";
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/FinalIsekaiScene.unity" },
            locationPathName = outputPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };
        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result == BuildResult.Succeeded)
            Debug.Log("Standalone Windows64 build succeeded: " + outputPath);
        else
            Debug.LogError("Standalone Windows64 build failed: " + report.summary.result);
    }
}
