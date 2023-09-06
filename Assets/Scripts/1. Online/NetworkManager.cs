using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public static NetworkManager instance;
    public int maxPlayers = 8;



    private void Awake()
    {
        
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);
        



    }

    // Start is called before the first frame update
    void Start()
    {

        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }


    public override void OnConnectedToMaster()
    {
        print("We Have Connected to master server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        print("Joined a room " + PhotonNetwork.CurrentRoom.Name);
    }

    public void createRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    /* public override void OnPlayerLeftRoom(Player otherPlayer)
     {
         GameManager.instance.players.Remove()
     }*/

    [PunRPC]
    public void changeScenes(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
