using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform ballistaTop;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;
    Transform target;
    
    void Update() {
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget() {
        Enemy[] enemiesArray = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemiesArray) {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if(targetDistance < maxDistance) {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        target = closestTarget;
    }

    void AimWeapon() {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        ballistaTop.LookAt(target);

        if(targetDistance <= range) {
            Attack(true);
        }
        else { 
            Attack(false);
        }
    }

    void Attack(bool isActive) {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
