using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerS : MonoBehaviour
{
    #region parameters
    public string Escena;
    #endregion

    #region methods
    public void CargarEscena()
    {
        SceneManager.LoadScene(Escena);
    }
    public void CargarEscenaActual()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
