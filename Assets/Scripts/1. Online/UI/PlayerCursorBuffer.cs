using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;

public class PlayerCursorBuffer : MonoBehaviourPun
{
    private OnlineGameManager gameManager;
    public PlayerInput pi;
    public int index;

    private void Awake()
    {
        gameManager = FindObjectOfType<OnlineGameManager>();
        index = pi.playerIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.photonView.RPC("spawnCursor", RpcTarget.AllBuffered, index);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
