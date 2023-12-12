using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonShower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode)) {
                Debug.Log("KeyCode down: " + kcode);
                TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
                textmeshPro.SetText("KeyCode: " + kcode);
            }
        }
    }
}
