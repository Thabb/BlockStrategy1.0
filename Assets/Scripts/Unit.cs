using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Unit : HealthEntity
{
    public Rigidbody rb;

    public int damage;
    public int range;
    public int attackSpeed;
    public int armor;

    public NavMeshAgent nav;
    public Vector3 destPoint;

    public PlayerInputController playerInputController;

    //private bool isInCombat { get; set; } = false;
    public bool isInCombat = false;
    //private Unit enemy;
    public Unit enemy;

    private double _lastAttackTime = 0;

    private new void Start()
    {
        base.Start();
        // sets the destination to the spawn point
        destPoint = transform.position;

        // if this is a player unit, add it to the list of active units
        if (team == 1)
        {
            playerInputController = FindObjectOfType<PlayerInputController>();
            playerInputController.AddActiveUnit(this);
        }
    }

    private void FixedUpdate()
    {
        if (isInCombat)
        {
            Combat();
        }
        else
        {
            Movement();
        }
        
    }

    private void Movement()
    {
        nav.destination = destPoint;
    }

    private void Combat()
    {
        if (enemy)
        {
            destPoint = enemy.transform.position;
        }
        else
        {
            destPoint = transform.position;
            isInCombat = false;
        }
        
        if (Vector3.Distance(transform.position, destPoint) < range)
        {
            nav.destination = transform.position;
            Attack();
        }
        else
        {
            nav.destination = destPoint;
        }
    }

    private void Attack()
    {
        if (_lastAttackTime + attackSpeed <= DateTimeOffset.Now.ToUnixTimeSeconds())
        {
            enemy.TakeDamage(this, damage);
            _lastAttackTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }

    public void TakeDamage(Unit attacker, float damage)
    {
        // TODO: include proper damage calculation
        base.TakeDamage(damage);
        
        if (currentHealth <= 0) DestroySelf();
        
        if (!isInCombat)
        {
            isInCombat = true;
            enemy = attacker;
        }
    }

    private void DestroySelf()
    {
        // if this is a player unit, remove it from the list of active units
        if (team == 1)
        {
            playerInputController.RemoveActiveUnit(this);
        }
        
        Destroy(gameObject);
    }
}
