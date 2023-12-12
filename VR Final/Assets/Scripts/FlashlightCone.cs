using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCone : MonoBehaviour
{
    public LayerMask whatIsEnemy;
    private RaycastHit lightHit;
    [SerializeField] private float maxDistance;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        if(Physics.Raycast(transform.position,transform.forward,out lightHit,maxDistance ,whatIsEnemy))
        {
            Debug.Log("Hit enemy");
            lightHit.collider.gameObject.GetComponent<ZombieEnemyController>().inLightRange = true;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawRay(transform.position,transform.forward*maxDistance);
    }
}
