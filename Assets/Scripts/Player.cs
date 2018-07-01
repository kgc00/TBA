using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public int playerIndex;

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
