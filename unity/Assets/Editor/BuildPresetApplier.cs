using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildPresetApplier : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("Applying presets before build...");

        AppluSfxPresets();
    }
    
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("Build finished. Post-build actions can run here.");
    }

    private void AppluSfxPresets()
    {
        SFXDatabase.Instance.musicVolume = 70f;
        SFXDatabase.Instance.uiVolume = 70f;
        SFXDatabase.Instance.interactionVolume = 70f;
    }
}