using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGen4 : MonoBehaviour //si bien es el 4 contiene menos cosas que el 3 ya q estoy usando TilemapExtras
{
    public Tilemap noiseMap;

    public TileBase[] baseTiles;


    [SerializeField] public int mapWidth = 10;
    [SerializeField] public int mapHeight = 10;
    [SerializeField] public int seed = 12345;
    [SerializeField] public int density = 35;
    [SerializeField] public int iteraciones = 3;
    [SerializeField] private Transform Grid;


    // Start is called before the first frame update
    void Start()
    {
        makeNoiseGrid();
        AutomataBasedOnNoise(1, noiseMap);
        for (int i = 1; i < iteraciones; i++)
        {
            Tilemap automataAnt = Grid.transform.GetChild(i).GetComponent<Tilemap>();
            //AutomataBasedOnNoise(i+1, automataAnt); //saca este comentario para ver como evoluciona
            AutomataBasedOnNoise(0, automataAnt);

        }

        for (int i = 0; i < iteraciones; i++) //eliminar hijos
        {
            Destroy(Grid.GetChild(i).gameObject);
        }

    }

    void makeNoiseGrid()
    {
        Random.State initialState = Random.state;

        for (int x = -mapWidth; x < mapWidth; x++)
        {
            for (int y = -mapHeight; y < mapHeight; y++)
            {
                TileBase tile = Noise(x, y);
                noiseMap.SetTile(new Vector3Int(x, y, 0), tile);

            }
        }
        Random.InitState(seed);
    }
    void AutomataBasedOnNoise(int layer, Tilemap anterior)
    {
        //GameObject Tilemap
        GameObject tilemapIter = new GameObject("TileIter");
        tilemapIter.transform.SetParent(Grid);
        Tilemap tilemap = tilemapIter.AddComponent<Tilemap>();
        tilemapIter.AddComponent<TilemapRenderer>();
        tilemap.GetComponent<Renderer>().sortingOrder = layer;
        for (int x = -mapWidth; x < mapWidth; x++)
        {
            for (int y = -mapHeight; y < mapHeight; y++)
            {
                TileBase tile = Automata(x, y, anterior);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);

            }
        }

    }
    
    TileBase Noise(int x, int y)
    {
        int random = Random.Range(0, 101);
        if (random > density)
        {
            return baseTiles[0];
        }
        else
        {
            return baseTiles[1];

        }
    }
    TileBase Automata(int x, int y, Tilemap noiseMap)
    {
        TileBase leftTile = noiseMap.GetTile(new Vector3Int(x - 1, y, 0));
        TileBase rightTile = noiseMap.GetTile(new Vector3Int(x + 1, y, 0));
        TileBase topTile = noiseMap.GetTile(new Vector3Int(x, y + 1, 0));
        TileBase bottomTile = noiseMap.GetTile(new Vector3Int(x, y - 1, 0));
        TileBase leftTopTile = noiseMap.GetTile(new Vector3Int(x - 1, y + 1, 0));
        TileBase rightTopTile = noiseMap.GetTile(new Vector3Int(x + 1, y + 1, 0));
        TileBase leftBottomTile = noiseMap.GetTile(new Vector3Int(x - 1, y - 1, 0));
        TileBase rightBottomTile = noiseMap.GetTile(new Vector3Int(x + 1, y - 1, 0));
        int total = 0;

        if (leftTile == baseTiles[0] || leftTile == null) //borde
        {
            total += 1;
        }
        if (rightTile == baseTiles[0] || rightTile == null)
        {
            total += 1;
        }
        if (topTile == baseTiles[0] || topTile == null)
        {
            total += 1;
        }
        if (bottomTile == baseTiles[0] || bottomTile == null)
        {
            total += 1;
        }

        if (leftTopTile == baseTiles[0] || leftTopTile == null)//vertice
        {
            total += 1;
        }
        if (rightTopTile == baseTiles[0] || rightTopTile == null)
        {
            total += 1;
        }
        if (leftBottomTile == baseTiles[0] || leftBottomTile == null)
        {
            total += 1;
        }
        if (rightBottomTile == baseTiles[0] || rightBottomTile == null)
        {
            total += 1;
        }


        if (total > 4)
        {
            return baseTiles[0];
        }
        else
        {
            return baseTiles[1];
        }
    }
    
}
