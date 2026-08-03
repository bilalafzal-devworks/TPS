using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy; //seconds 
    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDestroy) Destroy(this.gameObject);
    }

    //if it collide with any object also destroy the bullet
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
