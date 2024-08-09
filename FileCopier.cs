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

                // Ensure the asset database is updated
                AssetDatabase.Refresh();

                WaitForFileCopy(destinationFilePath);
                AssetDatabase.ImportPackage(destinationFilePath, true);
                // Load and instantiate the object
                ObjectMaker.CreateObj(destinationFilePath);
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
    private static void WaitForFileCopy(string filePath)
    {
        while (!File.Exists(filePath))
        {
            System.Threading.Thread.Sleep(300); // Wait for 100 milliseconds
        }

        long fileSize = new FileInfo(filePath).Length;
        long previousSize = 0;

        // Wait until the file size stops changing
        while (fileSize != previousSize)
        {
            previousSize = fileSize;
            System.Threading.Thread.Sleep(300); // Wait for 100 milliseconds
            fileSize = new FileInfo(filePath).Length;
        }
    }
}
