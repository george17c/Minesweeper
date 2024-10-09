using UnityEngine;
using UnityEngine.SceneManagement;

public class AlgGreu : MonoBehaviour
{
    private int height = 16;
    private int width = 30;
    private int cateBombe = 99;

    private Tabla tabla;
    private Celula[,] status;
    private bool gameover;

    private void OnValidate()
    {
        cateBombe = Mathf.Clamp(cateBombe, 0, width * height);
    }

    private void Awake()
    {
        tabla = GetComponentInChildren<Tabla>();
    }

    private void Start()
    {
        JocNou();
    }

    public void JocNou()
    {

        status = new Celula[width, height];
        gameover = false;

        GeneratorCelule();
        GeneratorBombe();
        GeneratorNumere();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        tabla.Draw(status);
    }

    private void GeneratorCelule()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Celula celula = new Celula();
                celula.pozitie = new Vector3Int(x, y, 0);
                celula.type = Celula.Type.Gol;
                status[x, y] = celula;
            }
        }
    }

    private void GeneratorBombe()
    {
        for (int i = 0; i < cateBombe; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (status[x, y].type == Celula.Type.Bomba)
            {
                x = Random.Range(0, width);
                y = Random.Range(0, height);
            }

            status[x, y].type = Celula.Type.Bomba;
        }
    }

    private void GeneratorNumere()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Celula celula = status[x, y];

                if (celula.type == Celula.Type.Bomba) continue;

                celula.numar = bombeAdiacente(x, y);

                if (celula.numar > 0) celula.type = Celula.Type.numar;

                status[x, y] = celula;
            }
        }
    }

    private int bombeAdiacente(int celulaX, int celulaY)
    {
        int nr = 0;

        for (int adiacentX = -1; adiacentX <= 1; adiacentX++)
        {
            for (int adiacentY = -1; adiacentY <= 1; adiacentY++)
            {
                if (adiacentX == 0 && adiacentY == 0) continue;

                int x = celulaX + adiacentX;
                int y = celulaY + adiacentY;

                if (GetCelula(x, y).type == Celula.Type.Bomba) nr++;
            }
        }

        return nr;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Joc");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            JocNou();
            MeniuPierdere.SetActive(false);
            MeniuCastig.SetActive(false);
        }
        else if (!gameover)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Marcaj();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Deschide();
            }
        }
    }

    private void Marcaj()
    {
        Vector3 pozitieMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int pozitieCelula = tabla.tilemap.WorldToCell(pozitieMouse);
        Celula celula = GetCelula(pozitieCelula.x, pozitieCelula.y);

        if (celula.type == Celula.Type.Invalid || celula.deschisa) return;

        celula.marcata = !celula.marcata;
        status[pozitieCelula.x, pozitieCelula.y] = celula;
        tabla.Draw(status);
    }

    private void Deschide()
    {
        Vector3 pozitieMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int pozitieCelula = tabla.tilemap.WorldToCell(pozitieMouse);
        Celula celula = GetCelula(pozitieCelula.x, pozitieCelula.y);

        if (celula.type == Celula.Type.Invalid || celula.deschisa || celula.marcata) return;

        switch (celula.type)
        {
            case Celula.Type.Bomba:
                Explozie(celula);
                break;
            case Celula.Type.Gol:
                DescoperaTot(celula);
                VerificaCastig();
                break;
            default:
                celula.deschisa = true;
                status[pozitieCelula.x, pozitieCelula.y] = celula;
                VerificaCastig();
                break;
        }

        tabla.Draw(status);
    }

    private void DescoperaTot(Celula celula)
    {
        if (celula.deschisa) return;
        if (celula.type == Celula.Type.Bomba || celula.type == Celula.Type.Invalid) return;

        celula.deschisa = true;
        status[celula.pozitie.x, celula.pozitie.y] = celula;
        if (celula.type == Celula.Type.Gol)
        {
            DescoperaTot(GetCelula(celula.pozitie.x - 1, celula.pozitie.y));
            DescoperaTot(GetCelula(celula.pozitie.x, celula.pozitie.y - 1));
            DescoperaTot(GetCelula(celula.pozitie.x + 1, celula.pozitie.y));
            DescoperaTot(GetCelula(celula.pozitie.x, celula.pozitie.y + 1));
            DescoperaTot(GetCelula(celula.pozitie.x - 1, celula.pozitie.y - 1));
            DescoperaTot(GetCelula(celula.pozitie.x - 1, celula.pozitie.y + 1));
            DescoperaTot(GetCelula(celula.pozitie.x + 1, celula.pozitie.y - 1));
            DescoperaTot(GetCelula(celula.pozitie.x + 1, celula.pozitie.y + 1));
        }
    }

    private void Explozie(Celula celula)
    {
        MeniuPierdere.SetActive(true);
        gameover = true;

        celula.deschisa = true;
        celula.explozie = true;
        status[celula.pozitie.x, celula.pozitie.y] = celula;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                celula = status[x, y];
                if (celula.type == Celula.Type.Bomba)
                {
                    celula.deschisa = true;
                    status[x, y] = celula;
                }
            }
        }
    }

    private void VerificaCastig()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Celula celula = status[x, y];

                if (celula.type != Celula.Type.Bomba && !celula.deschisa)
                    return;
            }
        }

        MeniuCastig.SetActive(true);
        gameover = true;
    }

    public GameObject MeniuCastig;
    public GameObject MeniuPierdere;

    private Celula GetCelula(int x, int y)
    {
        if (valid(x, y))
            return status[x, y];
        else return new Celula();
    }

    private bool valid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}