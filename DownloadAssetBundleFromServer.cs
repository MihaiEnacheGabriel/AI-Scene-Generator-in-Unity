using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Text.RegularExpressions;

public class DownloadAssetBundleFromServer : MonoBehaviour
{
    public static void DownloadAssetBundle(string url)
    {
        GameObject go = new GameObject("AssetBundleDownloader");
        DownloadAssetBundleFromServer downloader = go.AddComponent<DownloadAssetBundleFromServer>();
        downloader.StartCoroutine(downloader.DownloadAssetBundleCoroutine(url));
    }

    private IEnumerator DownloadAssetBundleCoroutine(string url)
    {
        string fileName = Path.GetFileName(url);
        fileName = SanitizeFileName(fileName);
        string tempFolderPath = Path.Combine(Application.temporaryCachePath, "DownloadedAssetBundles");
        Directory.CreateDirectory(tempFolderPath);
        string filePath = Path.Combine(tempFolderPath, fileName);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            DownloadHandlerFile downloadHandler = new DownloadHandlerFile(filePath);
            www.downloadHandler = downloadHandler;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading AssetBundle: " + www.error);
            }
            else
            {
                Debug.Log($"AssetBundle downloaded to: {filePath}");
                FileCopier.CopyFile(filePath);
            }
        }
    }


    private string SanitizeFileName(string fileName)
    {
        return Regex.Replace(fileName, @"[<>:""/\\|?*]", "_");
    }
}
