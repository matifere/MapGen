using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapBushGen : MonoBehaviour
{
    public Tilemap noiseBushMap;
    //public Tilemap noiseBase;
    public TileBase[] bush;
    [SerializeField] private int layer = 1;
    [SerializeField] private int antiDensity = 10;
    //[SerializeField] private Material materialProp;

    private int mapWidth = 10;
    private int mapHeight = 10;
    private int seed = 12345;
    private int density = 35;
    private int iteraciones = 3;
    [SerializeField] private Transform Grid;


    // Start is called before the first frame update
    void Start()
    {
        MapGen4 mapGen4 = gameObject.GetComponent<MapGen4>();
        mapWidth = mapGen4.mapWidth;
        mapHeight = mapGen4.mapHeight;
        seed = mapGen4.seed;
        density = mapGen4.density - antiDensity;
        iteraciones = mapGen4.iteraciones;


        makeNoiseGrid();
        AutomataBasedOnNoise(1, noiseBushMap);
        for (int i = 1; i < iteraciones; i++)
        {
            Tilemap automataAnt = Grid.transform.GetChild(i).GetComponent<Tilemap>();
            //AutomataBasedOnNoise(i+1, automataAnt); //saca este comentario para ver como evoluciona
            AutomataBasedOnNoise(layer, automataAnt);

        }
        //Grid.transform.GetChild(iteraciones).GetComponent<TilemapRenderer>().material = materialProp;

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
                noiseBushMap.SetTile(new Vector3Int(x, y, 0), tile);

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
            return bush[0];
        }
        else
        {
            return bush[1];

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

        if (leftTile == bush[0] || leftTile == null) //borde
        {
            total += 1;
        }
        if (rightTile == bush[0] || rightTile == null)
        {
            total += 1;
        }
        if (topTile == bush[0] || topTile == null)
        {
            total += 1;
        }
        if (bottomTile == bush[0] || bottomTile == null)
        {
            total += 1;
        }

        if (leftTopTile == bush[0] || leftTopTile == null)//vertice
        {
            total += 1;
        }
        if (rightTopTile == bush[0] || rightTopTile == null)
        {
            total += 1;
        }
        if (leftBottomTile == bush[0] || leftBottomTile == null)
        {
            total += 1;
        }
        if (rightBottomTile == bush[0] || rightBottomTile == null)
        {
            total += 1;
        }


        if (total > 4)
        {
            return bush[0];
        }
        else
        {
            return bush[1];
        }
    }
}
