using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] Transform recoilFollowPos;
    [SerializeField] float kickBackAmount = -1;

    [SerializeField] float returnAmount = 10f, kickBackSpeed = 20f;

    float currentRecoilPos, finalRecoilPos;

    Transform backupTransform;



    void Update()
    {   //backupTransform = recoilFollowPos;
        currentRecoilPos = Mathf.Lerp(currentRecoilPos, 0, returnAmount * Time.deltaTime);
        finalRecoilPos = Mathf.Lerp(finalRecoilPos, currentRecoilPos, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(recoilFollowPos.localPosition.x, recoilFollowPos.localPosition.y, finalRecoilPos);
    }
    public void TriggerRecoil() => currentRecoilPos += kickBackAmount;
    //public void ResetRecoilPos() => currentRecoilPos += backupTransform;

}
