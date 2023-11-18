using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LazyFloodFill : MonoBehaviour
{

    [SerializeField] private Tilemap Map;
    [SerializeField] public TileBase[] tiles;
    [SerializeField] public int n = 10; // Tamaño de la matriz
    [SerializeField] private int indiceDeProb = 1;
    [SerializeField] private int Iter = 10;


    //esto de aca es para el pasto:
    public delegate void OnScriptAComplete();
    public static event OnScriptAComplete ScriptAComplete;

    void Start()
    {
        int[,] grid = new int[n, n];

        for (int i = 0; i < n; i++) //esto vendria a ser el noisemap (aca hay un problema, no siempre se generan todos los tiles)
        {
            for (int j = 0; j < n; j++)
            {
                if (LazyBoolFill(i, j))
                {
                    grid[i, j] = Random.Range(0,tiles.Length);//revisar si devuelve el ultimo
                }
                else
                {
                    grid[i, j] = 0; // aca va el indice del tilemap base
                }
                
            }
        }
        for (int t = 0; t < Iter; t++)
        {
            for (int i = 0; i < n; i++) //iter
            {
                for (int j = 0; j < n; j++)
                {
                    if (i - 1 >= 0 &&
                        i + 1 < n &&
                        j - 1 >= 0 &&
                        j + 1 < n)
                    {
                        grid[i, j] = FloodFill(grid[i, j], grid[i - 1, j], grid[i + 1, j], grid[i, j + 1], grid[i, j - 1]);
                    }

                }
            }
        }

        //notar que el mapa genera sobre los bordes del bioma pequeños puntos de agua, el sig for es para eliminarlos
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int actual = 0;
                if (i - 1 >= 0 &&
                        i + 1 < n &&
                        j - 1 >= 0 &&
                        j + 1 < n)
                {
                    actual = grid[i, j + 1]; //veo el de arriba (vale para cualquier direccion)
                    if ((grid[i - 1, j] == actual && grid[i + 1, j] == actual && grid[i, j + 1] == actual && grid[i, j - 1] == actual) && actual != grid[i, j])
                    {
                        grid[i, j] = actual;
                    }
                }
                

            }
        }
        //nota, no esta del todo arreglado esto, en el borde de los biomas sigue pasando pero en menor medida, creo que la mejor forma de arreglarlo es tomar el promedio y aplicarlo

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                TileBase tile = tiles[grid[i, j]];
                Map.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }

        if (ScriptAComplete != null)
        {
            ScriptAComplete();
        }
    }
    bool LazyBoolFill(int x, int y)
    {
        //la idea es empezar en algun lugar random de la matriz
        int randomX = Random.Range(0,n);
        int randomY = Random.Range(0, n);

        for (int i = 0; i < indiceDeProb; i++)
        {
            if (x == randomX && y == randomY ||
            x == randomX - i && y == randomY - i ||
            x == randomX + i && y == randomY + i ||
            x == randomX + i && y == randomY - i ||
            x == randomX - i && y == randomY + i)

            {
                return true;
            }
            
        }

        return false;
    }

    int FloodFill(int actual, int izq, int der, int arr, int abj)
    {
        int random = Random.Range(0, indiceDeProb);
        bool prob = (random > indiceDeProb / 2);
        if (actual == 0) //las prioridades nos matan
        {
            if (izq != 0 && prob)
            {
                return izq;
            }
            if (abj != 0 && prob)
            {
                return abj;
            }
            if (arr != 0 && prob)
            {
                return arr;
            }
            if (der != 0 && prob)
            {
                return der;
            }
        }

        return actual;
    }

}
