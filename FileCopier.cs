using UnityEngine;
using System.IO;
using System;
using UnityEditor;

public static class FileCopier
{
    public static void CopyFile(string sourceFilePath)
    {
        try
        {
            string destinationFolderPath = "Assets/";
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

            if (File.Exists(sourceFilePath))
            {
                FileUtil.CopyFileOrDirectory(sourceFilePath, destinationFilePath);
                Debug.Log($"File copied to {destinationFilePath}");
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
}
