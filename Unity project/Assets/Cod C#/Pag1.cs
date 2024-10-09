using UnityEngine;
using UnityEngine.SceneManagement;


public class Pag1 : MonoBehaviour
{
    public void iesi()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void usor()
    {
        SceneManager.LoadScene("Usor");
    }

    public void mediu()
    {
        SceneManager.LoadScene("Mediu");
    }

    public void greu()
    {
        SceneManager.LoadScene("Greu");
    }

    public void ajutor()
    {
        SceneManager.LoadScene("Ajutor");
    }

    public void inapoi()
    {
        SceneManager.LoadScene("Joc");
    }
}
