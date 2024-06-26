using UnityEngine;

internal class SettingsMonitor : ResourceMonitor
{
    private string _applicationVersion;
    private string _applicationUnityVersion;
    private string _defaultShaderChunkCount;
    private string _defaultShaderChunkSizeInMB;

    internal override string GetResourceLog()
    {
        return $@"Settings:
Version: {_applicationVersion}
Unity version: {_applicationUnityVersion}
Application.runInBackground: {Application.runInBackground}
QualitySettings.vSyncCount: {QualitySettings.vSyncCount}
Application.targetFrameRate: {Application.targetFrameRate}
DefaultShaderChunkCount: {_defaultShaderChunkCount}
DefaultShaderChunkSizeInMB: {_defaultShaderChunkSizeInMB}
QualityLevel: {QualitySettings.GetQualityLevel()} - {QualitySettings.names[QualitySettings.GetQualityLevel()]}";
    }

    private void Awake()
    {
        _applicationVersion = Application.version;
        _applicationUnityVersion = Application.unityVersion;

#if UNITY_EDITOR
        _defaultShaderChunkCount = UnityEditor.PlayerSettings.GetDefaultShaderChunkCount().ToString();
        _defaultShaderChunkSizeInMB = UnityEditor.PlayerSettings.GetDefaultShaderChunkSizeInMB().ToString();
#else
        _defaultShaderChunkCount = "n/a";
        _defaultShaderChunkSizeInMB = "n/a";
#endif
    }
}
