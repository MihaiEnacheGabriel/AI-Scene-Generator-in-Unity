using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class MyListener : MonoBehaviour
{
    Thread thread;
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;


    void Start()
    {
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
    }

    void GetData()
    {
        server = new TcpListener(IPAddress.Any, connectionPort);
        server.Start();

        // Create a client
        client = server.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
        server.Stop();
    }

    void Connection()
    {
        // Read data from stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        // Decode
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        
        if (dataReceived != null && dataReceived != "")
        {
            // Convert
            position = ParseData(dataReceived);
            nwStream.Write(buffer, 0, bytesRead);
        }
    }

    //Interpreter
    public static Vector3 ParseData(string dataString)
    {
        Debug.Log(dataString);
        // Remove the parentheses
        if (dataString.StartsWith("(") && dataString.EndsWith(")"))
        {
            dataString = dataString.Substring(1, dataString.Length - 2);
        }

        // Split into an array
        string[] stringArray = dataString.Split(',');

        //Store
        Vector3 result = new Vector3(
            float.Parse(stringArray[0]),
            float.Parse(stringArray[1]),
            float.Parse(stringArray[2]));

        return result;
    }

    Vector3 position = Vector3.zero;

    void Update()
    {
        // Set this object's position
        transform.position = position;
    }
}