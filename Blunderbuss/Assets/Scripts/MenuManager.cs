using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   public void CargarEscena(int escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
