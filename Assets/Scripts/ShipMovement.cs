using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header("Sail Controller")]
    [SerializeField] private SailController _sailController;

    [SerializeField] private Transform _flapPivot;

    [Header("Thrust")] 
    [SerializeField] private float _maxThrust = 15f;
    [Header("Steering")] 
    [SerializeField] private float _turnSpeed = 35f;

    [SerializeField] private float _maxFlapAngle = 15f;
    [SerializeField] private float _maxFlapReturnSpeed = 5f;
    
    [Header("Drag")] 
    [SerializeField] private float _forwardDrag = 0.5f;
    [SerializeField] private float _sideDrag = 5.5f;
    private Vector3 _velocity;
    private float _currentFlapAngle;
    public float CurrentSpeed => Vector3.Dot(_velocity, transform.forward);

    public void Steer(float inputAxis, float deltaTime)
    {
        float targetFlapAngle = inputAxis * _maxFlapAngle *0.35f * _maxFlapReturnSpeed;
        _currentFlapAngle = Mathf.MoveTowards(_currentFlapAngle, targetFlapAngle, _turnSpeed  * deltaTime
        );
        if (_flapPivot != null)
        {
            // roattion along Y axis of flap
            _flapPivot.localRotation = Quaternion.Euler(0f, _currentFlapAngle, 0f);
        }
        //proportional to velocity in water
        float speedFactor = Mathf.Clamp01(Mathf.Abs(CurrentSpeed) / 5f);
        float rotationAmount = inputAxis * _turnSpeed * speedFactor * deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }

    public void SimulateShipMovement(float deltaTIme)
    {
        Vector3 windVector = WindController.Instance != null ? WindController.Instance.WindVector : Vector3.forward * 8f;
        Vector3 windDir = windVector.normalized;
        float windSpeed = windVector.magnitude;
        
        float headingDotWind = Vector3.Dot(transform.forward, -windDir);
        float headwindPenalty = Mathf.Clamp01((1f - headingDotWind) / 0.8f);
        
        Vector3 sailNormal = _sailController.WorldSailForward;
        float sailWindAlignment = Mathf.Abs(Vector3.Dot(sailNormal, windDir));

        float sailEfficiency = (_sailController.OpenPrecentafe / 100f) * sailWindAlignment * headwindPenalty;
        Vector3 thrustForce = transform.forward * (sailEfficiency * windSpeed * _maxThrust);
        
        _velocity += thrustForce * deltaTIme;
        
        Vector3 forwardVelocity = Vector3.Project(_velocity, transform.forward);
        Vector3 sideVelocity = Vector3.Project(_velocity, transform.right);

        forwardVelocity = Vector3.Lerp(forwardVelocity, Vector3.zero, _forwardDrag * deltaTIme);
        sideVelocity = Vector3.Lerp(sideVelocity, Vector3.zero, _sideDrag * deltaTIme);

        _velocity = forwardVelocity + sideVelocity;
        
        transform.position += _velocity * deltaTIme;
    }

}
