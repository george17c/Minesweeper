using UnityEngine;
using UnityEngine.Tilemaps;

public class Tabla : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    public Tile celulaNecunoscuta;
    public Tile celulaGoala;
    public Tile celulaBomba;
    public Tile celulaExplodata;
    public Tile celulaMarcata;
    public Tile celula1;
    public Tile celula2;
    public Tile celula3;
    public Tile celula4;
    public Tile celula5;
    public Tile celula6;
    public Tile celula7;
    public Tile celula8;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Celula[,] status)
    {
        int width = status.GetLength(0);
        int height = status.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Celula celula = status[x, y];
                tilemap.SetTile(celula.pozitie, GetTile(celula));
            }
        }
    }

    private Tile GetTile(Celula celula)
    {
        if (celula.deschisa)
        {
            return GetCelulaDeschisa(celula);
        }
        else if (celula.marcata)
        {
            return celulaMarcata;
        }
        else
        {
            return celulaNecunoscuta;
        }
    }

    private Tile GetCelulaDeschisa(Celula celula)
    {
        switch (celula.type)
        { 
            case Celula.Type.Gol: return celulaGoala;
            case Celula.Type.Bomba: return celula.explozie ? celulaExplodata : celulaBomba;
            case Celula.Type.numar: return GetCelulaNumar(celula);
            default: return null;
        }
    }

    private Tile GetCelulaNumar(Celula celula)
    {
        switch (celula.numar)
        {
            case 1: return celula1;
            case 2: return celula2;
            case 3: return celula3;
            case 4: return celula4;
            case 5: return celula5;
            case 6: return celula6;
            case 7: return celula7;
            case 8: return celula8;
            default: return null;
        }
    }
}