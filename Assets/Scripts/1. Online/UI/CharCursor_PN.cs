using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class CharCursor_PN : MonoBehaviourPun
{
    public PlayerSettings settings;

    public Transform selected;
    public PlayerInput input;
    public CharacterCell[,] rows;
    public Color cursorColor;
    public int playerIndex;
    public bool selectDone;
    OnlineGameManager manager;
    private Player photonPlayer;
    public PlayerContainerUI playerContainerPrefab;
    public PlayerControlUI playerControlPrefab;

    private bool navLock;
    public int rIndex = 0;
    public int cIndex = 0;
    public int lifetime;
    public int skinIndex;
    private int punId;
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0;
        selectDone = false;
        manager = FindObjectOfType<OnlineGameManager>();
        input = GetComponent<PlayerInput>();
        rows = manager.rows;
    }

    // Update is called once per frame
    void Update()
    {

        if (!photonView.IsMine)
        {
            GetComponent<PlayerInput>().enabled = false;
            return;
        }

        if (lifetime < 30)
        {
            lifetime++;
        }
        selected = rows[rIndex,cIndex].transform;
        transform.position = selected.position;

    }
    [PunRPC]
    public void Initialized(Player player)
    {

        if (GameSettingsManager.instance != null)
        {
            string inputString = GameSettingsManager.instance.inputList[GameSettingsManager.instance.inputIndex].ToString();
        
            if (inputString == "keyboard") {
                input.SwitchCurrentControlScheme("keyboard", Keyboard.current, Mouse.current);
            }
            else
            {
                input.SwitchCurrentControlScheme("gamePad", Gamepad.current);
            }
        }

        punId = player.ActorNumber;
        photonPlayer = player;
        cursorColor = settings.player_colors[OnlineGameManager.instance.charSelect.cursor_PNs.Count];
        OnlineGameManager.instance.cursors[punId - 1] = this;

        if (!photonView.IsMine)
        {
            GetComponent<PlayerInput>().enabled = false;
        }
       gameObject.GetComponentInChildren<Image>().color = cursorColor;
       transform.SetParent(OnlineGameManager.instance.charSelect.transform);
       GameObject playerchcktxt = Instantiate(OnlineGameManager.instance.playerTextPrefab, OnlineGameManager.instance.textContainerParent);
       print("Before init");
       playerchcktxt.GetComponent<PlayerCheckText_PN>().initialize(this);
    }
    private void navUp()
    {
        if (lifetime >= 30)
        {
            rIndex--;
            checkIndex();
        }

    }
    private void navDown()
    {
        if (lifetime >= 30)
        {
            rIndex++;
            checkIndex();
        }
        

    }
    private void navLeft()
    {
        if (lifetime >= 30)
        {
            cIndex--;
            checkIndex();
        }

    }
    private void navRight()
    {
        if (lifetime >= 30)
        {
            cIndex++;
            checkIndex();
        }
    }
    

    private void checkOverlap()
    {
        CharCursor_PN[] cursors = FindObjectsOfType<CharCursor_PN>();
        foreach(CharCursor_PN cursor in cursors)
        {
            if (cursor.gameObject != this.gameObject)
            {
                if (cursor.selected == selected)
                {
                    transform.SetSiblingIndex(cursor.transform.GetSiblingIndex() -1);
                }
            }
        }
    }


    private void charSelect()
    {
        if (lifetime >= 30)
        {
            bool skinOpen = true;
            foreach(CharCursor_PN i in manager.cursors)
            {
                if (i.selectDone)
                {
                    if (i.skinIndex == rows[rIndex, cIndex].index)
                    {
                        skinOpen = false;
                    }
                }
            }
            if (!rows[rIndex, cIndex].avaliable)
            {
                skinOpen = false;
            }
            if (skinOpen && manager.cursors.Length > 1)
            {
                manager.audio.PlayOneShot(manager.playlist[UnityEngine.Random.Range(2, 3)]);
                selectDone = true;
                navLock = true;
                skinIndex = rows[rIndex, cIndex].index;
                print(skinIndex);
                int doneCount = 0;
                foreach (CharCursor_PN i in manager.cursors)
                {
                    if (i.selectDone)
                    {
                        doneCount++;
                    }
                }
                if (doneCount == manager.cursors.Length)
                {
                    foreach (CharCursor_PN i in manager.cursors)
                    {
                        i.spawnCharacter();
                    }
                    manager.readyTime = true;
                    manager.canvas.SetActive(false);
                    manager.charSelectDone = true;
                    manager.playerInputManager.gameObject.SetActive(false);

                }
            }else if(manager.cursors.Length <= 1) {

                manager.errorDialog("Must Have Atleast 2 Players!", 2f);

            }
        }

    }

     
    public void spawnCharacter()
    {
        //Spawn PLayer
        PlayerInput p = PlayerInput.Instantiate(manager.playerPrefab, playerIndex: input.playerIndex, controlScheme: input.currentControlScheme, pairWithDevice: input.devices[0]);
        p.transform.SetParent(null);
        //Setting Skin
        p.GetComponentInChildren<SpriteRenderer>().sprite = settings.player_skins[skinIndex];
        p.GetComponent<Animator>().SetInteger("skinIndex", skinIndex);
        p.GetComponent<PlayerController>().skinIndex = skinIndex;

        //Create ui container
        PlayerContainerUI containerUI = Instantiate(playerContainerPrefab, manager.playerContainerParent).GetComponent<PlayerContainerUI>();
        containerUI.intialized(Color.green, manager.player_badges_dict[settings.player_strings[skinIndex]]);
        p.GetComponent<PlayerController>().setUIContainer(containerUI);


        //Create Control Unit
        bool keyboard = false;
        PlayerControlUI controlUI = Instantiate(playerControlPrefab, manager.playerControlsParent).GetComponent<PlayerControlUI>();
        if(p.currentControlScheme == "keyboard" || p.currentControlScheme == "Keyboard")
        {
            keyboard = true;
        }
        PlayerController player = p.GetComponent<PlayerController>();
        Ability ability = player.attackPrefab.GetComponent<Ability>();
        controlUI.intialized(ability.badge, keyboard,ability.description);
        p.GetComponent<PlayerController>().setUIControls(controlUI);

        //Add Player to list
        manager.players.Add(p.GetComponent<PlayerController>());
        //change position
        int x = UnityEngine.Random.Range(0, manager.localPoints.Length);
        p.transform.position = manager.spawnPoints[x].position;
        manager.localPointChange(x);
       
    }

    private void checkIndex()
    {
        if(cIndex > manager.colCount)
        {
            cIndex = 0;
            rIndex++;
        }
        else if(cIndex < 0)
        {
            cIndex = manager.colCount;
            rIndex--;
        }
        if(rIndex > manager.rowCount)
        {
            rIndex = 0;
        }
        else if(rIndex < 0)
        {
            rIndex = manager.rowCount;
        }
    }

    public void onUpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!navLock)
            {
                if (photonView.IsMine)
                {
                    navUp();
                }

            }
        }
    }
    public void onDownInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!navLock)
            {

                if (photonView.IsMine)
                {
                    navDown();
                }
            }
        }
    }
    public void onLeftInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!navLock)
            {
                if (photonView.IsMine)
                {
                    navLeft();
                }
            }
        }
    }
    public void onRightInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!navLock)
            {
                if (photonView.IsMine)
                {
                    navRight();
                }
            }
        }
    }
    public void onSelectInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (photonView.IsMine)
            {
                charSelect();
            }

        }
    }
}

