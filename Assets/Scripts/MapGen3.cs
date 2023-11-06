using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
public class MapGen3 : MonoBehaviour
{
    public Tilemap noiseMap;
    public TileBase[] tiles;

    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private int seed = 12345;
    [SerializeField] private int density = 35;
    [SerializeField] private int iteraciones = 3;
    [SerializeField] private Transform Grid;


    // Start is called before the first frame update
    void Start()
    {
        makeNoiseGrid();
        AutomataBasedOnNoise(1 , noiseMap);
        for (int i = 1; i < iteraciones; i++)
        {
            Tilemap automataAnt = Grid.transform.GetChild(i).GetComponent<Tilemap>();
            //AutomataBasedOnNoise(i+1, automataAnt); //saca este comentario para ver como evoluciona
            AutomataBasedOnNoise(0, automataAnt);

        }
        Tilemap baldosaMaker = Grid.transform.GetChild(iteraciones).GetComponent<Tilemap>();
        Baldosa(0, baldosaMaker);
        for (int i = 0; i < iteraciones + 1; i++) //eliminar hijos
        {
            Destroy(Grid.GetChild(i).gameObject);
        }

    }

    void makeNoiseGrid()
    {
        Random.State initialState = Random.state;
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
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
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                TileBase tile = Automata(x, y, anterior); 
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);

            }
        }
        
    }
    void Baldosa(int layer, Tilemap anterior)
    {
        GameObject tilemapIter = new GameObject("Baldosa");
        tilemapIter.transform.SetParent(Grid);
        Tilemap tilemap = tilemapIter.AddComponent<Tilemap>();
        tilemapIter.AddComponent<TilemapRenderer>();
        tilemap.GetComponent<Renderer>().sortingOrder = layer;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                TileBase tile = ReemplazarBaldosa(x, y, anterior);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);

            }
        }
    }
    TileBase Noise(int x, int y)
    {
        int random = Random.Range(0, 101);
        if (random > density)
        {
            return tiles[0];
        }
        else
        {
            return tiles[1];

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
        
        if (leftTile == tiles[0] || leftTile == null) //borde
        {
                total += 1;
        }
        if (rightTile == tiles[0] || rightTile == null)
        {
                total += 1;
        }
        if (topTile == tiles[0] || topTile == null)
        {
                total += 1;
        }
        if (bottomTile == tiles[0] || bottomTile == null)
        {
                total += 1;
        }

        if (leftTopTile == tiles[0] || leftTopTile == null)//vertice
        {
                total += 1;
        }
        if (rightTopTile == tiles[0] || rightTopTile == null)
        {
                total += 1;
        }
        if (leftBottomTile == tiles[0] || leftBottomTile == null)
        {
                total += 1;
        }
        if (rightBottomTile == tiles[0] || rightBottomTile == null)
        {
                total += 1;
        }
        

        if (total > 4)
        {
            return tiles[0];
        }
        else
        {
            return tiles[1];
        }
    }
    TileBase ReemplazarBaldosa(int x, int y, Tilemap automata)
    {
        TileBase leftTile = automata.GetTile(new Vector3Int(x - 1, y, 0));
        TileBase rightTile = automata.GetTile(new Vector3Int(x + 1, y, 0));
        TileBase topTile = automata.GetTile(new Vector3Int(x, y + 1, 0));
        TileBase bottomTile = automata.GetTile(new Vector3Int(x, y - 1, 0));
        TileBase leftTopTile = automata.GetTile(new Vector3Int(x - 1, y + 1, 0));
        TileBase rightTopTile = automata.GetTile(new Vector3Int(x + 1, y + 1, 0));
        TileBase leftBottomTile = automata.GetTile(new Vector3Int(x - 1, y - 1, 0));
        TileBase rightBottomTile = automata.GetTile(new Vector3Int(x + 1, y - 1, 0));

        //para esta parte lo mejor es hacer reglas.
        bool izq = leftTile == null && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1] ||
                   leftTile == tiles[1] && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1];
        bool der = rightTile == null && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1] ||
                   rightTile == tiles[1] && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1];
        bool arr = topTile == null && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1] ||
                   topTile == tiles[1] && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1];
        bool abj = bottomTile == null && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1] ||
                   bottomTile == tiles[1] && automata.GetTile(new Vector3Int(x, y, 0)) != tiles[1];

        //acordate de poner las cosas con predicados fuertes primero

        //esquinas
        if(izq && arr)
        {
            return tiles[9];
        }
        if (izq && abj)
        {
            return tiles[5];
        }
        if (der && arr)
        {
            return tiles[8];
        }
        if (der && abj)
        {
            return tiles[4];
        }
        
        //bordes
        if (izq)
        {
            return tiles[Random.Range(16,18)];
        }
        if (der)
        {
            return tiles[Random.Range(14, 16)];
        }
        if (arr)
        {
            return tiles[Random.Range(6, 8)];
        }
        if (abj)
        {
            return tiles[Random.Range(2, 4)];
        }

        //centro
        if (automata.GetTile(new Vector3Int(x, y, 0)) == tiles[0])
        {
            return tiles[Random.Range(10, 14)];
        }

        //agua
        else //devuelve la que ya estaba
        {
            return automata.GetTile(new Vector3Int(x , y , 0));
        }
    }
}
