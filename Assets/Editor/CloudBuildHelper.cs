using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class CloudBuildHelper : MonoBehaviour
{
    /*In order to get things running on unity cloud build, don't forget to add this script and method names on advanced settings pre/post-export method!*/
#if UNITY_CLOUD_BUILD
    public static void IncrementBuildNumber(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        string buildNumber = manifest.GetValue("buildNumber", "0");
        Debug.LogWarning("Setting build number to " + buildNumber);
        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);
        PlayerSettings.iOS.buildNumber = buildNumber;
    }
#endif


    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // ModifyFrameworks(path);
            ModifyPlist(path);
        }
    }

    private static void ModifyPlist(string path)
    {
        string plistPath = path + "/Info.plist";
        Debug.Log("Plist path: " + plistPath);
       
        PlistDocument plistDocument = new PlistDocument();
        plistDocument.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict plistDocumentRootDict = plistDocument.root;
        plistDocumentRootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

    }

    private static void ModifyFrameworks(string path)
    {
        string projPath = PBXProject.GetPBXProjectPath(path);

        var project = new PBXProject();
        project.ReadFromFile(projPath);

        string mainTargetGuid = project.GetUnityMainTargetGuid();

        foreach (var targetGuid in new[] {mainTargetGuid, project.GetUnityFrameworkTargetGuid()})
        {
            project.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
        }

        project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

        project.WriteToFile(projPath);
    }
}