using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCone : MonoBehaviour
{
    public LayerMask whatIsEnemy;
    public bool aimedAtEnemy;
    private RaycastHit lightHit;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        aimedAtEnemy = Physics.SphereCast(ray,1f,out lightHit, whatIsEnemy);
        if(aimedAtEnemy)
        {
            lightHit.collider.gameObject.GetComponent<ZombieEnemyController>().inLightRange = true;
        }
    }
}
