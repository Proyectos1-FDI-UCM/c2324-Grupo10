using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FaunoConfig", fileName ="FaunoConfiguration")]
public class FaunoConfig : ScriptableObject
{
    [SerializeField][Range(1f, 10f)] private float _walkSpeed;
    public float WalkSpeed => _walkSpeed;

    [SerializeField][Range(3f, 15f)] private float _runSpeed;
    public float RunSpeed => _runSpeed;
    
    [SerializeField] private float _airTime;
    public float AirTime => _airTime;
    
    [SerializeField][Range(10000f, 30000f)] private float _jumpForce;
    public float JumpForce => _jumpForce;

    [SerializeField][Range(2f,15f)] private float _distCuchilla;
    public float DistCuchilla => _distCuchilla;

    [SerializeField] private int _subeSpeed;
    public int SubeSpeed => _subeSpeed;

    [SerializeField] private int _bajaSpeed;
    public int BajaSpeed => _bajaSpeed;

    [SerializeField] private Vector2 _vectMina;
    public Vector2 VectMina => _vectMina;

    [SerializeField][Range(5f, 20f)] private int _minaTime;
    public int MinaTime => _minaTime;

    [SerializeField][Range(1f, 3.5f)] private float _closeRange;
    public float CloseRange => _closeRange;

    [SerializeField][Range(7f, 10f)] private float _longRange;
    public float LongRange => _longRange;
}
