using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public delegate void Moving(Vector2 velocity);
    public event Moving OnWalk = delegate { };
    public event Moving OnRunning = delegate { };
    public event Moving OnIdle = delegate { };

    public bool exhausted = false;
    private Rigidbody2D rigidbody;
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private BiomeMultiplier[] biomeMultipliers;
    [SerializeField] private TileDetector tileDetector;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, bool running) 
    {
        if (running && !exhausted)
        {
            rigidbody.velocity = direction * runSpeed * GetMultiplier();
            OnRunning.Invoke(rigidbody.velocity);
        }
        else
        {
            rigidbody.velocity = direction * walkSpeed * GetMultiplier();
            if(rigidbody.velocity == Vector2.zero) OnIdle.Invoke(rigidbody.velocity);
            else OnWalk.Invoke(rigidbody.velocity);
        }
    }

    private float GetMultiplier()
    {
        foreach (var biomeMultiplier in biomeMultipliers)
        {
            if (biomeMultiplier.biome == tileDetector.biome) return biomeMultiplier.multiplier;
        }
        return 1;
    }
}
