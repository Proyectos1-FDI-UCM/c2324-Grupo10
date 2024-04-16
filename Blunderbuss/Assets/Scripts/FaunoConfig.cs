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
}
