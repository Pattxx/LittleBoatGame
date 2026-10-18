using UnityEngine;
//Simulates realistic sail dynamics, hydrodynamics, and rudder turning without rigidbodies
public class ShipMovement : MonoBehaviour
{
    [Header("Sail Controller")]
    [SerializeField] private SailController _sailController;

    [Header("Thrust")] 
    [SerializeField] private float _maxThrust = 15f;
    [Header("Steering")] 
    [SerializeField] private float _turnSpeed = 35f;
    
    [Header("Drag")] 
    [SerializeField] private float _forwardDrag = 0.5f;
    [SerializeField] private float _sideDrag = 5.5f;
    private Vector3 _velocity;
 
    // Current forward speed relative to the ship's facing direction
    public float CurrentSpeed => Vector3.Dot(_velocity, transform.forward);

    public void Steer(float inputAxis, float deltaTime)
    {
       
        //turning authority scales with forward speed
        float speedFactor = Mathf.Clamp01(Mathf.Abs(CurrentSpeed) / 5f);
        float rotationAmount = inputAxis * _turnSpeed * speedFactor * deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }

    public void SimulateShipMovement(float deltaTime)
    {
        if (_sailController == null)
        {
            return;
        }

        //Sample environmental wind or default to a baseline breeze if unassigned
        Vector3 windVector = WindController.Instance != null 
            ? WindController.Instance.WindVector 
            : Vector3.forward * 8f;

        Vector3 windDir = windVector.normalized;
        float windSpeed = windVector.magnitude;
        
        //Evaluates the "no-go" dead zone when pointing directly into the wind source
        float headingDotWind = Vector3.Dot(transform.forward, -windDir);
        float headwindPenalty = Mathf.Clamp01((1f - headingDotWind) / 0.8f);
        
        //measure wind incidence against the sail sheet angle
        Vector3 sailNormal = _sailController.WorldSailForward;
        float sailWindAlignment = Mathf.Abs(Vector3.Dot(sailNormal, windDir));

        //Compute effective thrust vector
        float sailAreaRatio = _sailController.OpenPrecentafe / 100f;
        float sailEfficiency = sailAreaRatio * sailWindAlignment * headwindPenalty;
        Vector3 thrustForce = transform.forward * (sailEfficiency * windSpeed * _maxThrust);

        _velocity += thrustForce * deltaTime;
        
        //Decompose velocity into surge (forward) and sway (leeway/drift) to simulate keel lift
        Vector3 forwardVelocity = Vector3.Project(_velocity, transform.forward);
        Vector3 sideVelocity = Vector3.Project(_velocity, transform.right);

        //strong lateral drag cancels leeway drift while letting the hull slide smoothly forward
        forwardVelocity = Vector3.Lerp(forwardVelocity, Vector3.zero, _forwardDrag * deltaTime);
        sideVelocity = Vector3.Lerp(sideVelocity, Vector3.zero, _sideDrag * deltaTime);

        _velocity = forwardVelocity + sideVelocity;
        
        //apply linear translation
        transform.position += _velocity * deltaTime;
    }

}
