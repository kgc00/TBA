using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public GameObject Tile;

    int heigth = 6;
    int width = 6;

	void Start () {
        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {

                GameObject go = Instantiate(Tile, this.transform);
                go.transform.Translate(new Vector3(i, j, 0));
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
