using UnityEditor;

public class DisableWebGLCompression : UnityEditor.Build.IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        if (report.summary.platform == UnityEditor.BuildTarget.WebGL)
        {
            PlayerSettings.WebGL.compressionFormat = UnityEditor.WebGLCompressionFormat.Disabled;
        }
    }
}