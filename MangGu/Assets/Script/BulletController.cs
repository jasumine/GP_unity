using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed,lifeTime;

    private Rigidbody RB;

    public ParticleSystem impactEffect;

    public 

    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RB.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;
        if(lifeTime<=0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            
            Destroy(other.gameObject);
        }



        Destroy(gameObject);
        Instantiate(impactEffect, transform.position, transform.rotation);
    }
}
