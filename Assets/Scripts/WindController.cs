using System;
using UnityEngine;

public sealed class WindController : MonoBehaviour
{
    public static WindController Instance { get; private set;}
    [SerializeField] private Vector3 _windDir = Vector3.forward;
    [SerializeField] private float _windSpeed = 10f;

    public Vector3 Dir => _windDir.normalized;
    public float Speed => _windSpeed;
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
        if (_windDir == Vector3.zero)
        {
            _windDir = Vector3.forward;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + Vector3.up * 2f;
        Gizmos.DrawRay(origin, Dir * 5f);
    }
}

