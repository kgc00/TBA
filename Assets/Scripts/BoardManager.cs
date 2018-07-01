using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public GameObject Tile;

    public Vector3[,] positions;

    public int height = 6;
    public int width = 6;
    public Vector3 offset = new Vector3(2.5f, 2, 0);

	void Start () {
        positions = new Vector3[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject go = Instantiate(Tile, this.transform);
                Vector3 pos = new Vector3(i, -j, 0);
                go.transform.Translate(pos - offset);
                positions[i, j] = pos - offset;
            }
        }
        
	}

	void Update () {
		
	}

    public bool IsLegalMove(int row, int col)
    {
        return (row < height && row >= 0 && col < width && row >= 0);
    }
}
