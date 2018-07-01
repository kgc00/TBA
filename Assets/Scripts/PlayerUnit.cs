using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour {
    [SyncVar]
    public Color color;

	void Start () {
        GetComponent<SpriteRenderer>().color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
