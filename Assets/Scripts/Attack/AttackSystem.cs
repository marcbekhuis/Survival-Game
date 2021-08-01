using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public Inventory[] inventories;
    public float minChange = 0;
    public float maxChange = 100;
    public float bonusChange = 0;

    protected float damage = 1;
    protected float radius = 1;
    protected float arc = 90;

    public float change
    {
        get
        {
            return Random.Range(minChange, maxChange) + bonusChange;
        }
    }

    public virtual void Attack(Vector2 direction)
    {
    }

    public virtual void SetSettings(float damage, float radius, float arc)
    {
        this.damage = damage;
        this.radius = radius;
        this.arc = arc;
    }
}
