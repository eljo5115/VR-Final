using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShootBomb : MonoBehaviour
{
    [Header("VR Controls")]
    [SerializeField] private InputDeviceCharacteristics inputDeviceCharacteristic;
    List<InputDevice> inputDevices = new List<InputDevice>();
    [SerializeField] Transform target;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float shootForce;
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputReader();
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
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed);
            if(primaryPressed)
            {
                Debug.Log("Primary pressed");
                LaunchBomb();
            }
        }
    }
    void LaunchBomb()
    {
        GameObject bomb = Instantiate(bombPrefab) as GameObject;
        bomb.GetComponent<Rigidbody>().AddForce(target.forward * shootForce);
        bomb.transform.parent = null;
    }
}
