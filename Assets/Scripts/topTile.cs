using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class topTile : MonoBehaviour
{
    [SerializeField] private Tilemap Map;
    [SerializeField] private Tilemap Top;
    private int mapSize; // lo voy a tomar de LFF *
    [SerializeField] private int proba = 10;
    [SerializeField] private int modulo = 3;

    [SerializeField] private TileBase[] Grasstiles; //es importante que la primer tile sea nula
    private TileBase[] mapTiles;

    void Start()
    {
        LazyFloodFill.ScriptAComplete += OnScriptAComplete; //chusmea si se termino de ejecutar lff
        
    }
    void OnScriptAComplete()
    {
        mapSize = gameObject.GetComponent<LazyFloodFill>().n; // *
        mapTiles = gameObject.GetComponent<LazyFloodFill>().tiles;

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                TileBase tile = ReemplazarBaldosa(x, y, mapTiles);
                Top.SetTile(new Vector3Int(x, y, 0), tile);

            }
        }
    }

    private TileBase ReemplazarBaldosa(int x, int y, TileBase[] tileBases)
    {
        int probabilidad = Random.Range(1, proba);
        TileBase actual = Map.GetTile(new Vector3Int(x , y, 0));
        if (tileBases[1] == actual && 1 == probabilidad % modulo) //esto hay que generalizarlo mas
        {
            return Grasstiles[1];
        }
        if (tileBases[1] == actual && 2 == probabilidad % modulo) 
        {
            return Grasstiles[2];
        }


        return Grasstiles[0];
    }
}
