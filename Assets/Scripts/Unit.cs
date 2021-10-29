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

    public PlayerInputController playerInputController;

    private void Start()
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

    private void DestroySelf()
    {
        // if this is a player unit, remove it from the list of active units
        if (team == 1)
        {
            playerInputController.RemoveActiveUnit(this);
        }
    }
}
