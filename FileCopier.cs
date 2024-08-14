using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class FileCopier
{
    public static void CopyFile(string sourceFilePath)
    {
        string destinationFolderPath = "Assets/";
        string fileName = Path.GetFileName(sourceFilePath);
        fileName = SanitizeFileName(fileName);
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

        try
        {
            if (File.Exists(sourceFilePath))
            {
                if (File.Exists(destinationFilePath))
                {
                    Debug.Log($"File already exists at {destinationFilePath}. Overwriting...");
                    File.Delete(destinationFilePath);
                }

                FileUtil.CopyFileOrDirectory(sourceFilePath, destinationFilePath);
                Debug.Log($"File copied to {destinationFilePath}");

                AssetDatabase.Refresh();
                ObjectMaker.LoadAndInstantiateAssetBundle(destinationFilePath);
            }
            else
            {
                Debug.LogError($"Source file does not exist: {sourceFilePath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error copying file: {ex.Message}");
        }
    }

    private static string SanitizeFileName(string fileName)
    {
        return Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");
    }
}
