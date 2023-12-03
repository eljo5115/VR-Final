using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrapplingBehavior : MonoBehaviour
{
    [Header("VR Controls")]
    [SerializeField] private InputDeviceCharacteristics inputDeviceCharacteristic;
    List<InputDevice> inputDevices = new List<InputDevice>();
    [Header("References")]
    [SerializeField] Rigidbody playerRb;
    [SerializeField] Transform target;
    [Space]
    [Header("Grappling")]
    private LineRenderer lr;
    private Vector3 swingPoint;
    private Vector3 currentGrapplePosition;
    private SpringJoint joint;
    private List<SpringJoint> persistJoints;
    [SerializeField] private LayerMask whatCanGrapple;
    [SerializeField] private float range = 25f;
    [SerializeField] private float grappleForce = 25f;
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
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity;
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
        foreach (var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed);
            inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryPressed);
            if (triggerValue > 0.9f && !isGrappling)
            {
                StartSwing();
            }
            else if (triggerValue < 0.2f && isGrappling)
            {
                StopSwing();
            }
            // if(primaryPressed)
            // {
            //     PersistJoint(joint);
            // }
            // if(secondaryPressed)
            // {
            //     RemovePersistJoint();
            // }
        }
    }
    void LateUpdate()
    {
        DrawRope();
    }
    void StartSwing()
    {
        isGrappling = true;
        RaycastHit hit;
        if (Physics.Raycast(target.position, target.forward, out hit, range, whatCanGrapple))
        {
            swingPoint = hit.point;
            joint = playerRb.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(target.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = grappleForce;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = target.position;
        }
    }
    void StopSwing()
    {
        // if(persistJoints.Contains(joint)) 
        // {
        //     return;
        // }
        isGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
    void PersistJoint(SpringJoint joint)
    {
        isGrappling = false;
        persistJoints.Add(joint);
        StopSwing();
    }
    void RemovePersistJoint()
    {
        Destroy(persistJoints[0]);
        persistJoints.RemoveAt(0);
    }
    void DrawRope()
    {
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, target.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
