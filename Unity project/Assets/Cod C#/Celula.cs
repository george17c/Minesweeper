using UnityEngine;

public struct Celula
{
    public enum Type
    {
        Invalid,
        Gol,
        Bomba,
        numar,
    }

    public Vector3Int pozitie;
    public Type type;
    public int numar;
    public bool marcata;
    public bool deschisa;
    public bool explozie;
}