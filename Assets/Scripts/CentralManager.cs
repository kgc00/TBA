using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CentralManager : NetworkBehaviour {

    static public CentralManager instance = null;
    public GameObject PlayerUnitPrefab;
    public List<Player> players = new List<Player>();
    public List<GameObject> playerUnits = new List<GameObject>();
    public enum GameState { PRE_GAME, IN_GAME };
    public enum Move {NONE, UP, DOWN, LEFT, RIGHT, ATTACK_1 };
    public GameState gameState = GameState.PRE_GAME;
    public Move movePlayer1 = Move.NONE;
    public Move movePlayer2 = Move.NONE;

    public Text timer;
    float timePerTurn = 10;
    [SyncVar]
    float timeLeftInTurn = 10;


    private void Awake()
    {
        instance = this;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        timer.text = timeLeftInTurn.ToString();

		if (isServer == false)
        {
            return;
        }

        if (players.Count < 2 && gameState == GameState.PRE_GAME)
        {
            return;
        }

        if (gameState == GameState.PRE_GAME)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].playerIndex = i;
                GameObject go = Instantiate(PlayerUnitPrefab);
                if (i == 0)
                {
                    go.transform.Translate(new Vector3(-2.5f, 3, 0));
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                } else
                {
                    go.transform.Translate(new Vector3(2.5f, -2, 0));
                    go.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                playerUnits.Add(go);
                NetworkServer.Spawn(go);
            }
            gameState = GameState.IN_GAME;
        }

        if (timeLeftInTurn <= 0)
        {
            ApplyMoves();
            timeLeftInTurn = timePerTurn;
        }
        else
        {
            timeLeftInTurn -= Time.deltaTime;
        }


	}

    void ApplyMoves()
    {
        ApplyMove(0, movePlayer1);
        ApplyMove(1, movePlayer2);

        movePlayer1 = Move.NONE;
        movePlayer2 = Move.NONE;
    }    

    void ApplyMove(int unitIndex, Move move)
    {
        if (move == Move.UP)
        {
            playerUnits[unitIndex].transform.Translate(Vector3.up);
        }
        if (move == Move.DOWN)
        {
            playerUnits[unitIndex].transform.Translate(Vector3.down);
        }
        if (move == Move.LEFT)
        {
            playerUnits[unitIndex].transform.Translate(Vector3.left);
        }
        if (move == Move.RIGHT)
        {
            playerUnits[unitIndex].transform.Translate(Vector3.right);
        }
    }

    public void RegisterMove(Player player, Move move)
    {
        if (player.playerIndex == 0 && movePlayer1 == Move.NONE)
        {
            movePlayer1 = move;
        }

        if (player.playerIndex == 1 && movePlayer2 == Move.NONE)
        {
            movePlayer2 = move;
        }
    }
}
