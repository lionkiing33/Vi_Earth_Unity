using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketManger : MonoBehaviour
{
    public static SocketManger socketmanager;
    string url = "http://" + ip.IP;
    public static Client Socket { get; private set; }

    void Awake()
    {
        socketmanager = this;
        Socket = new Client(url);
        Socket.Opened += SocketOpened;
        Socket.Connect();
        Debug.Log("소켓 연결됨");
        DontDestroyOnLoad(gameObject);
    }

    private void SocketOpened(object sender, System.EventArgs e)
    {
        Debug.Log("Socket Opened");
    }

    void OnDisable()
    {
        Socket.Close();
    }
}
