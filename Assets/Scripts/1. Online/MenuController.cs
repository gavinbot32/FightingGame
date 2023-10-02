using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using System;
using Photon.Chat.Demo;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    #region Variables

    public TextMeshProUGUI toastMsg;
    public GameObject loadingOBJ;

    [Header("Non-Menus")]
    public Button options_button;
    public Button back_button;

    [Header("Menu Screens")]
    public GameObject mainUI;
    public GameObject createRoom;
    public GameObject roomLobbyScreen;
    public GameObject roomSelectLobbyScreen;

    [Header("Main UI")]
    public TextMeshProUGUI mainUITitle;
    public TMP_InputField nameInput;
    public Button createRoomBttnMain;
    public Button joinRoomBttn;

    [Header("Create Room")]
    public TMP_InputField roomNameInput;
    public Button createRoomBttnCR;
    public Button backBttnCR;

    [Header("Room Lobby")]
    public GameObject leftColumn;
    public GameObject rightColumn;
    public GameObject hostColumn;
    
    [Header("Left Column")]
    public Image playerHeaderBG;
    public TextMeshProUGUI playerHeaderText;
    public Image playerBoardBG;
    public TextMeshProUGUI playerBoardText;
    public TMP_Dropdown inputSelectionDP;
    
    [Header("Right Column")]
    public Image roomHeaderBg;
    public TextMeshProUGUI roomHeaderText;
    public Image roomNameBG;
    public TextMeshProUGUI roomNameText;
    public Image optionsPanelBG;
    public GameObject optionSelectParent;
    public Image selectedMap;
    public GameObject timerParent;
    public TextMeshProUGUI timerHeaderText;
    public TextMeshProUGUI timerText;
    public Button startGameRL;
    public Button leaveLobby;

    [Header("Host Column")]
    public GameObject mapGrid;
    public GameObject abilityGrid;

    [Header("Room Selection Lobby Screen")]
    public RectTransform roomListContainer;
    public GameObject roomBttnPrefab;

    [Header("Universal Variables")]
    public string name;
    public string roomName;
    public string selected_level;
    public string default_level;

    [SerializeField]
    private List<GameObject> roomBttnList = new List<GameObject>();
    [SerializeField]
    private List<RoomInfo> roomInfoList = new List<RoomInfo>();
    #endregion


    private void Awake()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        createRoomBttnMain.interactable = false;
        joinRoomBttn.interactable = false;
        nameInput.interactable = false;
        Cursor.lockState = CursorLockMode.None;
        
        if(PhotonNetwork.IsConnected == false)
        {
            toastMsg.gameObject.SetActive(true);
            loadingOBJ.SetActive(true);
            showMessage("Connecting To Sever");
        }
    }

    public void setScreen(GameObject screen)
    {
        mainUI.SetActive(false);
        createRoom.SetActive(false);
        roomLobbyScreen.SetActive(false);
        roomSelectLobbyScreen.SetActive(false);

        screen.SetActive(true);
        if(screen == roomSelectLobbyScreen)
        {
            updateRoomSelectionLobby();
        }
    
    }

    public void leaveOnlineMode()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
        }

        if(GameSettingsManager.instance != null)
        {
            Destroy(GameSettingsManager.instance);
        }

        SceneManager.LoadScene("Title_Screen");
    }

    public override void OnConnectedToMaster()
    {
        nameInput.interactable = true;
        loadingOBJ.SetActive(false);
        showMessage("Connected To Server", 2f);
    }

    public void OnCreateRoomBttn_main()
    {
        PhotonNetwork.NickName = name;
        roomNameInput.text = null;
        createRoomBttnCR.interactable = false;
        setScreen(createRoom);
    }

    public void OnRoomNameChange()
    {
        roomName = roomNameInput.text;
        if(roomName.Length > 2 && roomName.Length < 13)
        {
            if (!createRoomBttnCR.interactable)
            {
                createRoomBttnCR.interactable=true;
                hideMessage();
            }
        }
        else
        {
            showMessage("Room Name must be between 3 and 13 characters");
            createRoomBttnCR.interactable = false;
        }
    }

    public void OnRoomNameSelect()
    {
        roomNameInput.text =null;
    }
    public void OnFindRoomBttn_main()
    {
        PhotonNetwork.NickName = name;
        setScreen(roomSelectLobbyScreen);
    }

    public void onBackBttn()
    {
        setScreen(mainUI);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    
    public void PlayerNameValueChanged()
    {
        name = nameInput.text;
        if(name.Length > 2 && name.Length < 13)
        {
            if (!createRoomBttnMain.interactable)
            {
                createRoomBttnMain.interactable=true;
                joinRoomBttn.interactable = true;
                hideMessage();
            }
        }
        else
        {
            showMessage("Name must be between 3 and 13 characters");
            createRoomBttnMain.interactable = false;
            joinRoomBttn.interactable = false;
        }
    }

    public void PlayerNameOnSelect()
    {
        nameInput.text = "";
    }
    
    public void onCreateRoom_CR()
    {
        NetworkManager.instance.createRoom(roomName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updateUI();
    }

    public void onLevelSelectChange()
    {
        //whatever is chosen from the map grid
    }

    public override void OnJoinedRoom()
    {
        setScreen(roomLobbyScreen);
        photonView.RPC("updateUI", RpcTarget.All);
    }

    [PunRPC]
    void updateUI()
    {
        roomNameText.text = "<b>" + PhotonNetwork.CurrentRoom.Name + "</b>";

        playerBoardText.text = "";

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerBoardText.text += (player.NickName + "\n");
        }

        startGameRL.interactable = PhotonNetwork.IsMasterClient;
        hostColumn.SetActive(PhotonNetwork.IsMasterClient);

        selected_level = default_level;

        //selected_level = mapGrid selection

    }

    public void onInputSelectionChange()
    {
        GameSettingsManager.instance.inputIndex = inputSelectionDP.value;
    }

    public void onCreateRoomBttn_rsl()
    {
        roomNameInput.text = null;
        createRoomBttnCR.interactable = false;
        setScreen(createRoom);
    }
    public void onStartGame_rl()
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;

        NetworkManager.instance.photonView.RPC("changeScenes", RpcTarget.All, selected_level);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfoList = roomList;   

    }

    public GameObject createNewRoomBttn()
    {
        GameObject bttn = Instantiate(roomBttnPrefab, roomListContainer);
        roomBttnList.Add(bttn);
        return bttn;
    }

    public void onJoinRoomBttn_rls(string roomName)
    {
        foreach(RoomInfo roomInfo in roomInfoList)
        {
            if(roomInfo.Name == roomName)
            {
                NetworkManager.instance.joinRoom(roomName);
            }
        }
    }

    public void onRefreshBttn()
    {
        updateRoomSelectionLobby();
    }
    public void updateRoomSelectionLobby()
    {
        foreach(GameObject bttn in roomBttnList)
        {
            bttn.SetActive(false);
        }
        foreach(RoomInfo room in roomInfoList)
        {
            if(room.PlayerCount <= 0)
            {
                roomInfoList.Remove(room);
            }
        }
        for(int i = 0; i < roomInfoList.Count; i++)
        {
            GameObject bttn = i >= roomBttnList.Count ? createNewRoomBttn() : roomBttnList[i];
            bttn.SetActive(true);
            bttn.transform.Find("roomName").GetComponent<TextMeshProUGUI>().text = roomInfoList[i].Name;
            bttn.transform.Find("roomCount").GetComponent <TextMeshProUGUI>().text = roomInfoList[i].PlayerCount.ToString()+"/"+ roomInfoList[i].MaxPlayers.ToString();
            //bttn.transform.Find("playerCountText").GetComponent<TextMeshProUGUI>().text = roomInfoList[i].PlayerCount.ToString() + " / " + roomInfoList[i].MaxPlayers.ToString();
            string rn = roomInfoList[i].Name;
            Button bttncomp = bttn.GetComponent<Button>();
            bttncomp.onClick.RemoveAllListeners();
            bttncomp.onClick.AddListener(() => { onJoinRoomBttn_rls(rn); });
        }
    }


    public void hideMessage()
    {
        toastMsg.gameObject.SetActive(false);
    }
    public void showMessage(string message)
    {
        toastMsg.text = message;
        toastMsg.gameObject.SetActive(true);
    }
    public void showMessage(string message, float timer)
    {
        toastMsg.text = message;
        StartCoroutine(toastAMessage(timer));
    }
    IEnumerator toastAMessage(float time)
    {
        toastMsg.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);
        toastMsg.gameObject.SetActive(false);

    }


   

    // Update is called once per frame
    void Update()
    {




        Toggle[] toggles = inputSelectionDP.GetComponentsInChildren<Toggle>();
        for (int i = 0; i<toggles.Length; i++)
        {
            if(i == 1)
            {
                bool connected = false;
                if(Keyboard.current != null) { connected = true;} else { connected = false; }
                toggles[i].interactable = connected;
               
            }
            else
            {
                bool connected = false;
                if (Gamepad.current != null) { connected = true; } else { connected = false; }
                toggles[i].interactable = connected;
                if (!connected)
                {
                    inputSelectionDP.value = 1;
                }
            }
        }
       
    }
}
