using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Unit : HealthEntity
{
    public Rigidbody rb;

    public int team;
    
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

    private void Update()
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
        destPoint = enemy.transform.position;
        
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
        // TODO: Add intervall for attacking

        enemy.TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        // TODO: if unit is not already in combat, set in combat against its attacker
        // TODO: include proper damage calculation
        currentHealth -= damage;
    }

    private void DestroySelf()
    {
        // if this is a player unit, remove it from the list of active units
        if (team == 1)
        {
            playerInputController.RemoveActiveUnit(this);
        }
    }
}
