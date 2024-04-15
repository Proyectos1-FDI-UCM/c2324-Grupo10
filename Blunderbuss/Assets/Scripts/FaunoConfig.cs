using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FaunoConfig", fileName ="FaunoConfiguration")]
public class FaunoConfig : ScriptableObject
{
    [SerializeField][Range(1f, 10f)] private float _walkSpeed;
    public float WalkSpeed => _walkSpeed;
}
