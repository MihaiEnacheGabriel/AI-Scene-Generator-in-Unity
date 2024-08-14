using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;

public class MyListener : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;
    ConcurrentQueue<string> fileDownloadQueue = new ConcurrentQueue<string>();
    Vector3 position = Vector3.zero;

    void Start()
    {
        position = transform.position;
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
    }

    void Update()
    {
        while (fileDownloadQueue.TryDequeue(out string assetBundleUrl))
        {
            Debug.Log($"Received AssetBundle URL: {assetBundleUrl}");
            DownloadAssetBundleFromServer.DownloadAssetBundle(assetBundleUrl); // Initiate the download process
        }
        transform.position = position;
    }

    void GetData()
    {
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        Debug.Log("Server Started");
        client = server.AcceptTcpClient();
        Debug.Log("Client Found");

        running = true;
        while (running)
        {
            Connection();
        }
        server.Stop();
    }

    void Connection()
    {
        try
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (!string.IsNullOrEmpty(dataReceived))
            {
                string[] parts = dataReceived.Split(',');
                if (parts.Length > 3)
                {
                    string assetBundleUrl = parts[3];
                    fileDownloadQueue.Enqueue(assetBundleUrl); // Enqueue the URL for downloading
                }

                position = ParseData(string.Join(",", parts[0], parts[1], parts[2]));
                nwStream.Write(buffer, 0, bytesRead);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in Connection: {ex.Message}");
        }
    }

    public static Vector3 ParseData(string dataString)
    {
        if (dataString.StartsWith("(") && dataString.EndsWith(")"))
        {
            dataString = dataString.Substring(1, dataString.Length - 2);
        }
        string[] stringArray = dataString.Split(',');
        return new Vector3(
            float.Parse(stringArray[0]),
            float.Parse(stringArray[1]),
            float.Parse(stringArray[2]));
    }
}
