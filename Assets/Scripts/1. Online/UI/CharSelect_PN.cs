using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class CharSelect_PN : MonoBehaviourPun
{
    public List<CharCursor_PN> cursor_PNs = new List<CharCursor_PN>();
    public GameObject playerTextPrefab;
    public Transform textContainerParent;
    private CharCursor_PN[] tempChars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void cursorUpdate()
    {

        foreach(CharCursor_PN tempCursor in cursor_PNs)
        {
            print("Before setting parent"+tempCursor);
            tempCursor.transform.SetParent(this.transform);
            print("Before Check Text Instansiate " + tempCursor);
            
            print("After init");
            /*if (photonView.IsMine)
            {
                playerchcktxt.GetComponent<PlayerCheckText_PN>().playerName = PhotonNetwork.LocalPlayer.NickName;
            }*/

        }
       

        /*
                CharCursor_PN[] tempCursors = FindObjectsOfType<CharCursor_PN>();
                for (int i = 0; i < tempCursors.Length; i++)
                {
                    CharCursor_PN tempCursor = tempCursors[i];
                    if (!cursor_PNs.Contains(tempCursor))
                    {
                        cursor_PNs.Add(tempCursor);
                        tempCursor.transform.SetParent(this.transform);
                        GameObject playerchcktxt = PhotonNetwork.Instantiate("playerCheckText", Vector3.zero, Quaternion.identity);
                        playerchcktxt.transform.SetParent(textContainerParent);
                        print("Before init");
                        playerchcktxt.GetComponent<PlayerCheckText_PN>().initialize(cursor_PNs.Count -1);
                        print("After init");

                        *//*if (photonView.IsMine)
                        {
                            playerchcktxt.GetComponent<PlayerCheckText_PN>().playerName = PhotonNetwork.LocalPlayer.NickName;
                        }*//*
                    }
                }*/

    }

}