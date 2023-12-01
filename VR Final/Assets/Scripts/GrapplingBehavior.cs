using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrapplingBehavior : MonoBehaviour
{
    [Header("VR Controls")]
    [SerializeField] private InputDeviceCharacteristics inputDeviceCharacteristic;
    List<InputDevice> inputDevices = new List<InputDevice>();
    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotFrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;

    [SerializeField] Rigidbody playerRb;
    [SerializeField] Transform target;
    [Space]
    [Header("Spring")]
    [SerializeField] float handForce = 100f;
    [SerializeField] float climbDrag = 500f;
    private bool isColliding; // use for is grounded
    private Vector3 _previousPosition;
    [Header("Grappling")]
    //I already have refs to each controller
    //raycast from target, range, ?projectile?
    //AddForce (Hookes Law) in direction of collision point
    [SerializeField] private LayerMask whatCanGrapple;
    [SerializeField] private float range;
    [SerializeField] private float grappleForce = 500f;
    private bool isGrappling;
    private bool grapplePressed;
    private RaycastHit grappleHit;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        InitializeInputReader();
        transform.position = target.position;
        transform.rotation = target.rotation;
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity;
        _previousPosition = transform.position;
    }
    void InitializeInputReader()
    {
        InputDevices.GetDevices(inputDevices);
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristic | InputDeviceCharacteristics.Controller, inputDevices);

    }
    // Update is called once per frame
    void Update()
    {        
        if(inputDevices.Count < 2)
        {
            InitializeInputReader();
        }
    }
    void FixedUpdate()
    {

        PIDMovement();
        PIDRotation();
        if(isColliding)
        {
            HookesLaw();
        }
        UpdateGrappling();
        if(isGrappling)
        {
            PullGrapple();
        }
    }
    void PIDMovement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1+kd*Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;

        float kdg = (kd+kp * Time.fixedDeltaTime) * g;

        Vector3 force = (target.position - transform.position) * ksg + (playerRb.velocity - rb.velocity) * kdg;
        rb.AddForce(force, ForceMode.Acceleration);
    }
    void PIDRotation()
    {
        float kp = (6f * rotFrequency) * (6f * rotFrequency) * 0.25f;
        float kd = 4.5f * rotFrequency * rotDamping;
        float g = 1 / (1+kd*Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd+kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if(q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -rb.angularVelocity * kdg;
        rb.AddTorque(torque, ForceMode.Acceleration);
    }
    void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * handForce;
        float drag = GetDrag();

        playerRb.AddForce(force, ForceMode.Acceleration);
        playerRb.AddForce(drag * -playerRb.velocity * climbDrag, ForceMode.Acceleration);
    }
    float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - _previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;
        drag = drag > 1 ? 1 : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        _previousPosition = transform.position;
        return drag;
    }
    void PullGrapple()
    {
        Vector3 force = (playerRb.position - grappleHit.point).normalized * grappleForce;
        float drag = GetDrag();

        playerRb.AddForce(-force, ForceMode.Acceleration);
        playerRb.AddForce(drag * -playerRb.velocity * climbDrag, ForceMode.Acceleration);
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        isColliding = true;
    }
    /// <summary>
    /// OnCollisionExit is called when this collider/rigidbody has
    /// stopped touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }
    void UpdateGrappling()
    {   
        foreach (var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        
        if(Physics.Raycast(target.position, target.forward ,out grappleHit, range, whatCanGrapple)&& triggerValue > 0.8f)
        {
            isGrappling = true;
        }else
        {
            isGrappling = false;
        }
        }
    }
}
