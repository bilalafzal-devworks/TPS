using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBloom : MonoBehaviour
{

    #region Bloom-Angles-Multiplayer
    float defaultBloomAngle = 3f;
    float WalkBloomAngle = 1.5f;
    float RunBloomAngle = 2f;
    float crouchBloomAngle = 0.5f;
    float adsBloomAngle = 0.5f;
    #endregion
    [Header("Current Bloom Angle")]
    [SerializeField] float currentBloomAngle;
    MovementStateManager movement;
    AimStateManager aim;
    void Start()
    {
        movement = GetComponentInParent<MovementStateManager>();
        aim = GetComponentInParent<AimStateManager>();
    }

    public Vector3 BloomAngles(Transform barrelPos)
    {
        //first check for its movement
        if (movement.currentState == movement.idleState) currentBloomAngle = defaultBloomAngle;
        else if (movement.currentState == movement.walkingState) currentBloomAngle = defaultBloomAngle * WalkBloomAngle;
        else if (movement.currentState == movement.runningState) currentBloomAngle = defaultBloomAngle * RunBloomAngle;
        else if (movement.currentState == movement.crouchState)
        {
            if (movement.dir.magnitude == 0) currentBloomAngle = defaultBloomAngle * crouchBloomAngle;
            else currentBloomAngle = defaultBloomAngle * crouchBloomAngle * RunBloomAngle;
        }
        // check in which aim state the player is currently in
        if (aim.currentState == aim.hipFireState || aim.currentState == aim.adsState)
            currentBloomAngle *= adsBloomAngle;
        //generate random points 
        float randX = Random.Range(-currentBloomAngle, currentBloomAngle);
        float randy = Random.Range(-currentBloomAngle, currentBloomAngle);
        float randZ = Random.Range(-currentBloomAngle, currentBloomAngle);

        //create new Vector3
        Vector3 randomRotation = new Vector3(randX, randy, randZ);
        return barrelPos.localEulerAngles + randomRotation;
    }
}
