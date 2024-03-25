using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyTrigger : MonoBehaviour
{
    #region parameters
    [SerializeField] private string Escena;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(Escena);
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
