using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GunItem", menuName = "Scriptable Objects/GunItem")]
public class GunItem : _BaseItem
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;

    public void Shoot(Transform shooterTransform, Vector3 bulletStartingPosition){
        GameObject bullet = Instantiate(bulletPrefab, bulletStartingPosition, shooterTransform.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if(rb != null) {
            rb.linearVelocity = shooterTransform.forward * bulletSpeed;
        }
    }

    
}
