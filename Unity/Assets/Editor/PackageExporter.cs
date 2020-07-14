using System.IO;
using UnityEditor;
using UnityEngine;

namespace Development
{
    public static class PackageExporter
    {
        [MenuItem("Dev/Export Package")]
        public static void Export()
        {
            var directories = new[]
            {
                "Assets/BridgeJsCore",
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            var outputPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "BridgeJsCore.unitypackage");

            AssetDatabase.ExportPackage(directories, outputPath, ExportPackageOptions.Recurse);

            Debug.LogFormat("ExportPackage {0}", outputPath);
        }
    }
}
