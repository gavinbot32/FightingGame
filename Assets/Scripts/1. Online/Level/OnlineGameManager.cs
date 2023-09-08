using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Windows;


public class OnlineGameManager : MonoBehaviourPun
{
    public PlayerSettings settings;

    [Header("Player Refs")]
    public Dictionary<string,Sprite> player_badges_dict;
    public Animator[] anims;
    public List<PlayerController> players = new List<PlayerController>();
    public CharCursor_PN[] cursors;

    public Transform[] spawnPoints;
    public Transform[] localPoints;

    public List<PlayerController> winningPlayers;

    public GameObject cursorPrefab;

    [Header("Prefab Refs")]
    public GameObject deathEffectPrefab;

    public static OnlineGameManager instance;
    public GameObject playerPrefab;


    [Header("Character Grid")]
    public int rowCount;
    public int colCount;
    public GameObject canvas;
    public CharacterCell[,] rows;
    

    [Header("Compnents")]
    public AudioSource audio;
    public AudioClip[] playlist;
    //Cursor join = 0-1, lock = 2-3

    [Header("level Vars")]
    public int maxTime;
    public float curTime;
    public bool charSelectDone;
    public bool readyTime;
    private int playersInGame;
    public PlayerInputManager playerInputManager;


    [Header("UI Components")]
    public Transform playerContainerParent;
    public Transform playerControlsParent;
    public Transform textContainerParent;
    public TextMeshProUGUI timerTxt;
    public GameObject timerObj;
    public GameObject playerTextPrefab;

    [Header("Debug")]
    public TextMeshProUGUI errorTxt;
    public bool debug = false;

    private void Awake()
    {
        instance = this;
        charSelectDone = false;
        readyTime = false;
        timerObj.SetActive(false);
        maxTime = PlayerPrefs.GetInt("roundTimer",100);
        curTime = maxTime;
        audio = GetComponent<AudioSource>();
        player_badges_dict = new Dictionary<string,Sprite>();
        canvas = FindObjectOfType<CharacterSelectionManager>().canvas;
        rowCount = canvas.GetComponent<CharacterSelectionManager>().rowCount;
        colCount = canvas.GetComponent<CharacterSelectionManager>().colCount;
    }


    // Start is called before the first frame update
    void Start()
    {
        cursors = new CharCursor_PN[PhotonNetwork.PlayerList.Length];

        timerTxt.text = curTime.ToString();
        winningPlayers = new List<PlayerController>();
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        playerInputManager.gameObject.SetActive(true);
        for (int i = 0; i < settings.player_badges.Length; i++)
        {
            string key = settings.player_strings[i];
            Sprite value = settings.player_badges[i];
            player_badges_dict.Add(key, value);

        }


        rows = canvas.GetComponent<CharacterSelectionManager>().rows;

       
        localPoints = spawnPoints;
        photonView.RPC("spawnEvent", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void spawnEvent()
    {
        playersInGame++;
        if (PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("spawnCursor", RpcTarget.All);
        }
        

        

    }
    [PunRPC]
    void spawnCursor()
    {
        GameObject cursor = PhotonNetwork.Instantiate("Cursor", Vector3.zero, Quaternion.identity);
        cursor.GetComponent <CharCursor_PN>().photonView.RPC("Initialized", RpcTarget.All, PhotonNetwork.LocalPlayer);
        cursor.transform.SetParent(canvas.transform);
        cursor.GetComponent<CharCursor_PN>().playerIndex = cursors.Length + 1;
        GameObject playerchcktxt = Instantiate(playerTextPrefab, textContainerParent);
        playerchcktxt.GetComponent<PlayerCheckText_PN>().owner = cursor.GetComponent<CharCursor_PN>();
        if (photonView.IsMine)
        {
            playerchcktxt.GetComponent<PlayerCheckText_PN>().playerName = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    private void FixedUpdate()
    {
        if (readyTime)
        {
            if (!timerObj.active)
            {
                timerObj.SetActive(true);
            }
            curTime -= Time.deltaTime;
        }
        timerTxt.text = ((int)curTime).ToString();
        debugCheck();
    }
    // Update is called once per frame
    void Update()
    {
        if (curTime <= 0) {
            winningPlayers.Clear();
            int highscore = 0;
            int index = 0;
            foreach(PlayerController player in players)
            {
                if (player.score > highscore)
                {
                    index = player.skinIndex;
                    winningPlayers.Clear();
                    highscore = player.score;
                    winningPlayers.Add(player);
                }
                else if(player.score == highscore)
                {
              
                    winningPlayers.Add(player);
                    
                }
            }

            if(winningPlayers.Count > 1)
            {
                //tie
                foreach(PlayerController player in players)
                {
                    if (!winningPlayers.Contains(player))
                    {
                        player.enabled = false;
                        player.youSuck();
                    }
                    curTime = 30;
                }
            }
            else
            {
                PlayerPrefs.SetInt("skinIndex", index);
                SceneManager.LoadScene("WinScene");
            }
        
        }
    }


    public void debugCheck()
    {
        foreach (Transform x in spawnPoints)
        {
            x.GetComponent<SpriteRenderer>().enabled = debug;
        }
    }   
    /*public void playerJoin(PlayerInput player)
    {
        player.GetComponentInChildren<SpriteRenderer>().color = player_colors[players.Count];
        player.GetComponentInChildren<SpriteRenderer>().sprite = player_skins[players.Count];
        player.GetComponent<Animator>().SetInteger("skinIndex", players.Count);
        players.Add(player.GetComponent<PlayerController>());
        int x = UnityEngine.Random.Range(0, localPoints.Length);
        player.transform.position = spawnPoints[x].position;
        RemoveAt(ref localPoints, x);
    }*/
    /*public void playerJoin(PlayerInput pi)
    {
        if (!charSelectDone)
        {
            photonView.RPC("spawnCursor", RpcTarget.AllBuffered, pi);
        }
    }*/

/*    [PunRPC]
    public void spawnCursor(int piIndex)
    {
        // PlayerInput cursor = PlayerInput.Instantiate(cursorPrefab, playerIndex: pi.playerIndex, controlScheme: pi.currentControlScheme, pairWithDevice: pi.devices[0]);
        PlayerCursorBuffer[] pcbs = FindObjectsOfType<PlayerCursorBuffer>();
        PlayerInput pi = new PlayerInput();
        bool moveOn = false;
        foreach(PlayerCursorBuffer pcb in pcbs)
        {
            if (pcb.index == piIndex)
            {
                pi = pcb.GetComponent<PlayerInput>();
            }
            else
            {
                moveOn = true;
                return;
            }
        }
        if (moveOn)
        {
            return;
        }
        GameObject cursor = PhotonNetwork.Instantiate("Cursor", Vector3.zero, Quaternion.identity);
        PlayerInput cpi = cursor.GetComponent<PlayerInput>();
        cpi.SwitchCurrentControlScheme(pi.currentControlScheme);
        audio.PlayOneShot(playlist[Random.Range(0, 2)]);
        cursor.transform.SetParent(canvas.transform);
        cursor.GetComponentInChildren<Image>().color = settings.player_colors[cursors.Count];
        cursor.GetComponent<CharCursor_PN>().cursorColor = settings.player_colors[cursors.Count];
        cursor.GetComponent<CharCursor_PN>().playerIndex = cursors.Count + 1;
        cursors.Add(cursor.GetComponent<CharCursor_PN>());
        GameObject playerchcktxt = Instantiate(playerTextPrefab, textContainerParent);
        playerchcktxt.GetComponent<PlayerCheckText_PN>().owner = cursor.GetComponent<CharCursor_PN>();

    }*/

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }

    public void localPointChange(int x)
    {
        RemoveAt(ref localPoints, x);
    }
 
    public void onPlayerDeath(PlayerController player, PlayerController attacker)
    {
        if(attacker != null)
        {
            attacker.addScore();
        }
        player.die(attacker);
    }

    public void errorDialog(string message,float waitTime)
    {
        StartCoroutine(ErrorCour(message, waitTime));
    }

    private IEnumerator ErrorCour(string message, float waitTime = 2f)
    {

        errorTxt.text = message;
        errorTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorTxt.gameObject.SetActive(false);
    }

}
