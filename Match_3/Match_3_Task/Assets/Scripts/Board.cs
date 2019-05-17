using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int Width;
    public int Height;
    public GameObject TilePrefab;
    private Background[,] Tiles;
    
    // Start is called before the first frame update
    void Start()
    {
        Tiles = new Background[Width, Height];
        SetUp();

    }

    private void SetUp()
    {
        for ( int i = 0; i < Width; i++)
        {
           for (int w = 0; w < Height; w++)
            {
                Vector2 tempPosition = new Vector2(i, w);
                Instantiate(TilePrefab, tempPosition ,Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
