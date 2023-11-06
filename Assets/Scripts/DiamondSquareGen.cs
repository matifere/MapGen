using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DiamondSquareGen : MonoBehaviour
{
    public Tilemap Map;
    public TileBase[] tiles;
    public int n = 4;
    public int roughness = 2;

    private int[,] matriz;

    void Start()
    {
        n = (int)(Mathf.Pow(2, n) + 1); // Debe tener esta relación
        matriz = new int[n, n];
        DiamondSquare();

        // Accede a los elementos de la matriz
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                TileBase tile = Replace(i, j);
                Map.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }

    void DiamondSquare()
    {
        int step = n - 1;

        matriz[0, 0] = Random.Range(0, tiles.Length);
        matriz[0, n - 1] = Random.Range(0, tiles.Length);
        matriz[n - 1, 0] = Random.Range(0, tiles.Length);
        matriz[n - 1, n - 1] = Random.Range(0, tiles.Length);

        while (step > 1)
        {
            DiamondStep(step);
            SquareStep(step);
            step /= 2;
            roughness /= 2; // Reduce la rugosidad con cada iteración
        }
    }

    private void DiamondStep(int step)
    {
        int half = step / 2;
        for (int i = half; i < n - 1; i += step)
        {
            for (int j = half; j < n - 1; j += step)
            {
                matriz[i, j] = (matriz[i - half, j - half] + matriz[i - half, j + half] + matriz[i + half, j - half] + matriz[i + half, j + half]) / 4 + Random.Range(-roughness, roughness + 1);
                
                matriz[i, j] = Mathf.Clamp(matriz[i, j], 0, tiles.Length - 1);
            }
        }
    }

    void SquareStep(int step)
    {
        int half = step / 2;
        for (int i = 0; i < n; i += half)
        {
            for (int j = (i + half) % step; j < n; j += step)
            {
                int count = 0;
                int total = 0;

                if (i - half >= 0)
                {
                    total += matriz[i - half, j];
                    count++;
                }

                if (i + half < n)
                {
                    total += matriz[i + half, j];
                    count++;
                }

                if (j - half >= 0)
                {
                    total += matriz[i, j - half];
                    count++;
                }

                if (j + half < n)
                {
                    total += matriz[i, j + half];
                    count++;
                }

                matriz[i, j] = total / count + Random.Range(-roughness, roughness + 1);
                
                matriz[i, j] = Mathf.Clamp(matriz[i, j], 0, tiles.Length - 1);
            }
        }
    }

    TileBase Replace(int x, int y)
    {
        return tiles[matriz[x, y]];
    }
}
