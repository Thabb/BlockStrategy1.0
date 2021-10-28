using UnityEngine;
using UnityEngine.AI;

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

    private void Start()
    {
        base.Start();
        // sets the destination to the spawn point
        destPoint = transform.position;
    }

    private void Update()
    {
        Movement();
        Combat();
    }

    private void Movement()
    {
        nav.destination = destPoint;
    }

    private void Combat()
    {
        
    }
}
