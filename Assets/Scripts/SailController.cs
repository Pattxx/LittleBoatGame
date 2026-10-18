using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Controls the mechanical state of te sail - sail triming angle and opening/closing sail
public class SailController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private Transform _mastPivot;

    [SerializeField] private Transform _sailMeshTransform;

    [Header("Settings")] 
    [SerializeField] private float _maxTrimAngle = 45f;
    [SerializeField] private float _trimSpeed = 40f;
    [SerializeField] private float _openSpeed = 50f;

    //current angle between the sail boom and the hull (+-45 deg)
    public float Angle { get; private set;}
    //sail open/close precentage (from 0 - closed to 100 - fully opened)
    public float OpenPrecentafe { get; private set;}
    //facing vector used by SHipMovement to calculate wind incidence
    public Vector3 WorldSailForward => _mastPivot != null ? _mastPivot.forward : transform.forward;

    public void AdjustSailTrim(float inputAxis, float deltaTime)
    {
        //Rotate the sail around the mast pivot
        Angle = Mathf.Clamp(Angle + inputAxis * _trimSpeed * deltaTime, -_maxTrimAngle, _maxTrimAngle);

        if (_mastPivot != null)
        {
            _mastPivot.localRotation = Quaternion.Euler(0f, Angle, 0f);
        }
    }
    public void AdjustSailOpen(float inputAxis, float deltaTime)
    {
        //Change the open area of the sail
        OpenPrecentafe = Mathf.Clamp(OpenPrecentafe + inputAxis * _openSpeed *  deltaTime, 0f, 100f);
        
        //sail scalling alont the Y axis to simulate lowering and raising the sail
        if (_sailMeshTransform != null)
        {
            //0.05f as the lower bound to prevent clipping
            float sailScaleRatio = Mathf.Clamp(OpenPrecentafe / 100f, 0.05f, 1f); 
            Vector3 currScale = _sailMeshTransform.localScale;
            
            _sailMeshTransform.localScale = new Vector3(currScale.x, sailScaleRatio*1f,currScale.z );
        }
    }
}
