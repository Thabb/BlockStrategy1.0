using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Rigidbody rb;

    public Material blueTeamTexture;
    public Material redTeamTexture;
    
    public int team;
    
    public int damage;
    public int range;
    public int attackSpeed;
    public int armor;

    public NavMeshAgent nav;
    public Vector3 destPoint;

    void Start()
    {
        SetColor();
        // sets the destination to the spawn point
        destPoint = transform.position;
    }

    private void SetColor()
    {
        if (team == 1)
        {
            gameObject.GetComponent<MeshRenderer>().material = blueTeamTexture;
        }
        else if (team == 2)
        {
            gameObject.GetComponent<MeshRenderer>().material = redTeamTexture;
        }
    }

    void Update()
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
