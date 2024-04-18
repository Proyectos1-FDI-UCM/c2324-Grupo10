using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaunoAnimator : MonoBehaviour
{
    private Animator _faunoAnim;
    // Start is called before the first frame update
    void Start()
    {
        _faunoAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cuchillada()
    {
        _faunoAnim.SetTrigger("Cuchillada");
    }

    
}
