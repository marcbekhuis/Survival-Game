using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackSystem
{
    [SerializeField] private LayerMask layerMask;

    public override void Attack(Vector2 direction)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (var collider2D in collider2Ds)
        {
            if(collider2D.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                healthSystem.StatValue -= damage;
            }
        }
    }
}
