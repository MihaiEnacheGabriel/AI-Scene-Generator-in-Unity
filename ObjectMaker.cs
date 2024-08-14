using UnityEngine;
using UnityEditor;

public static class ObjectMaker
{
    public static void LoadAndInstantiateAssetBundle(string filePath)
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(filePath);
        if (bundle == null)
        {
            Debug.LogError($"Failed to load AssetBundle from {filePath}");
            return;
        }

        string[] assetNames = bundle.GetAllAssetNames();
        if (assetNames.Length > 0)
        {
            GameObject go = bundle.LoadAsset<GameObject>(assetNames[0]);
            if (go != null)
            {
                InstantiatePrefab(go);
                Debug.Log($"Asset '{assetNames[0]}' instantiated from AssetBundle.");
            }
            else
            {
                Debug.LogError("Failed to load GameObject from AssetBundle.");
            }
        }
        else
        {
            Debug.LogError("No assets found in AssetBundle.");
        }

        bundle.Unload(false);
    }

    private static void InstantiatePrefab(GameObject prefab)
    {
        GameObject newObj = GameObject.Instantiate(prefab);
        Debug.Log($"Prefab instantiated: {newObj.name}");
    }
}
