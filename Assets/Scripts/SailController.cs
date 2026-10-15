using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SailController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private Transform _sailPivot;

    [SerializeField] private Transform _sailMeshTransform;

    [Header("Settings")] 
    [SerializeField] private float _maxTrimAngle = 45f;
    [SerializeField] private float _trimSpeed = 40f;
    [SerializeField] private float _openSpeed = 50f;

    public float Angle { get; private set;}
    public float OpenPrecentafe { get; private set;}
    public Vector3 WorldSailForward => _sailPivot != null ? _sailPivot.forward : transform.forward;

    public void FixTrim(float inputAxis, float deltaTime)
    {
        Angle = Mathf.Clamp(Angle + inputAxis * _trimSpeed * deltaTime, -_maxTrimAngle, _maxTrimAngle);

        if (_sailPivot != null)
        {
            _sailPivot.localRotation = Quaternion.Euler(0f, Angle, 0f);
        }
    }


}
