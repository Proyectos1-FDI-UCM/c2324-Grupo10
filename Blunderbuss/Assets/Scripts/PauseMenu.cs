using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    #region references

    #endregion

    #region parameters
    [SerializeField]
    GameObject CanvasPausa;
    #endregion

    #region methods
    public void pause()
    {
        CanvasPausa.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Continue ()
    {
        CanvasPausa.SetActive(false);
        Time.timeScale = 1f;
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

    private void Awake()
    {
        CanvasPausa.SetActive(false);
    }
}
