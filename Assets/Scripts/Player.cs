using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public int playerIndex;
    public int row;
    public int col;

    public int health = 3;

	void Start () {
	    if (isLocalPlayer == false)
        {
            return;
        }
        StartCoroutine(DelayedRegistration());
	}
	
	void Update () {

        if (isLocalPlayer == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CmdSendMove(CentralManager.Move.UP);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CmdSendMove(CentralManager.Move.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CmdSendMove(CentralManager.Move.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CmdSendMove(CentralManager.Move.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CmdSendMove(CentralManager.Move.ATTACK_UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CmdSendMove(CentralManager.Move.ATTACK_DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CmdSendMove(CentralManager.Move.ATTACK_LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CmdSendMove(CentralManager.Move.ATTACK_RIGHT);
        }
    }

    private IEnumerator DelayedRegistration()
    {
        while (CentralManager.instance == null)
        {
            yield return null;
        }
        CmdRegisterPlayer();
    }

    [Command]
    private void CmdRegisterPlayer()
    {
        CentralManager.instance.players.Add(this);
    }

    [Command]
    private void CmdSendMove(CentralManager.Move move)
    {
        Debug.Log("Command received");
        CentralManager.instance.RegisterMove(this, move);
    }
}
