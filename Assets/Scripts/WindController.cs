using System;
using UnityEngine;

//Provides global wind vector and visual gizmo for simulation
[DisallowMultipleComponent]
public sealed class WindController : MonoBehaviour
{
    public static WindController Instance { get; private set;}
    [Header("Wind Configuration")]
    [Tooltip("Global worldspace direction the wind blows toward.")]
    [SerializeField] private Vector3 _windDir = Vector3.forward;

    [Tooltip("Base velocity of the wind in world units per second.")]
    [SerializeField] private float _windSpeed = 10f;

    //Normalized direction of the wind
    public Vector3 Dir => _windDir.normalized;
    //Wind speed magnitude
    public float Speed => _windSpeed;
    //Full wind vector (dir *speed)
    public Vector3 WindVector => Dir * _windSpeed;

    private void Awake()
    {
        //Destroy if an Instance of windController already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnValidate()
    {
        //Guards against zero length vector in editor
        if (_windDir.sqrMagnitude < 0.001f)
        {
            _windDir = Vector3.forward;
        }
    }
    private void OnDrawGizmos()
    {
        //render guide in Scene view
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + Vector3.up * 2f;
        Gizmos.DrawRay(origin, Dir * 5f);
    }
}

