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
    public enum GameState { PRE_GAME, IN_GAME, POST_GAME };
    public enum Move {NONE, UP, DOWN, LEFT, RIGHT, ATTACK_UP, ATTACK_DOWN, ATTACK_LEFT, ATTACK_RIGHT };
    [SyncVar]
    public GameState gameState = GameState.PRE_GAME;
    public Move movePlayer1 = Move.NONE;
    public Move movePlayer2 = Move.NONE;

    BoardManager bm;

    public Text timer;
    float timePerTurn = 5;
    [SyncVar]
    float timeLeftInTurn = 5;


    private void Awake()
    {
        instance = this;
        bm = GetComponent<BoardManager>();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (gameState == GameState.IN_GAME)
        {
            timer.text = timeLeftInTurn.ToString();
        }
        else if (gameState == GameState.POST_GAME)
        {
            timer.text = "Game Over";
        }
        
        

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
                Player p = players[i];
                p.playerIndex = i;              

                GameObject go = Instantiate(PlayerUnitPrefab);
                if (i == 0)
                {
                    
                    p.row = 0;
                    p.col = 0;
                    go.GetComponent<PlayerUnit>().color = Color.red;
                } else
                {                    
                    p.row = 5;
                    p.col = 5;
                    go.GetComponent<PlayerUnit>().color = Color.blue;
                }
                go.transform.position = bm.positions[p.row, p.col];
                playerUnits.Add(go);
                NetworkServer.Spawn(go);
            }
            gameState = GameState.IN_GAME;
        }
        
        if (gameState == GameState.IN_GAME)
        {
            if (timeLeftInTurn <= 0)
            {
                ApplyMoves();
                timeLeftInTurn = timePerTurn;
            }
            else
            {
                timeLeftInTurn -= Time.deltaTime;
            }

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].health <= 0)
                {
                    gameState = GameState.POST_GAME;
                }
            }
        }      

	}

    void ApplyMoves()
    {
        ApplyMove(0, movePlayer1);
        ApplyMove(1, movePlayer2);

        ApplyAttack(0, 1, movePlayer1);
        ApplyAttack(1, 0, movePlayer2);

        movePlayer1 = Move.NONE;
        movePlayer2 = Move.NONE;
    }    

    void ApplyAttack(int playerIndex, int enemyIndex, Move move)
    {
        Player p = players[playerIndex];
        Player e = players[enemyIndex];
        if (move == Move.ATTACK_UP)
        {
            if (p.col == e.col && p.row - e.row <= 2 && p.row - e.row > 0)
            {
                e.health -= 1;
            }
        }

        if (move == Move.ATTACK_DOWN)
        {
            if (p.col == e.col && e.row - p.row <= 2 && e.row - p.row > 0)
            {
                e.health -= 1;
            }
        }

        if (move == Move.ATTACK_LEFT)
        {
            if (p.row == e.row && p.col - e.col <= 2 && p.col - e.col > 0)
            {
                e.health -= 1;
            }
        }

        if (move == Move.ATTACK_RIGHT)
        {
            if (p.row == e.row && e.col - p.col<= 2 && e.col - p.col > 0)
            {
                e.health -= 1;
            }
        }

    }

    void ApplyMove(int unitIndex, Move move)
    {
        Player p = players[unitIndex];
        GameObject u = playerUnits[unitIndex];
        if (move == Move.UP)
        {
            if (bm.IsLegalMove(p.row - 1, p.col))
            {
                p.row -= 1;
                u.transform.Translate(Vector3.up);
            }
            
        }
        if (move == Move.DOWN)
        {
            if (bm.IsLegalMove(p.row + 1, p.col))
            {
                p.row += 1;
                u.transform.Translate(Vector3.down);
            }

        }
        if (move == Move.LEFT)
        {
            if (bm.IsLegalMove(p.row, p.col - 1))
            {
                p.col -= 1;
                u.transform.Translate(Vector3.left);
            }

        }
        if (move == Move.RIGHT)
        {
            if (bm.IsLegalMove(p.row, p.col + 1))
            {
                p.col += 1;
                u.transform.Translate(Vector3.right);
            }
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
