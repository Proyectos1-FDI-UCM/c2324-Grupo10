using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelCamera : MonoBehaviour
{
    private Canvas _myCanvas;
    // Start is called before the first frame update
    void Start()
    {
        _myCanvas = GetComponent<Canvas>();
        _myCanvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
