using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : MonoBehaviour
{
    // TODO: add reference to some kind of resource handler of the owning team
    public Base blueBase;
    public Base redBase;
    private int ownerTeam = 0;

    private List<Unit> unitsOnThePoint = new List<Unit>();

    public float generationAmount;
    
    private void Start()
    {
        StartCoroutine(GenerateResourcesForOwner());
    }

    private void FixedUpdate()
    {
        bool overtakable = true;
        
        foreach (Unit unit in unitsOnThePoint)
        {
            if (!unit)
            {
                unitsOnThePoint.Remove(unit);
            }
            else if (unit.team == ownerTeam)
            {
                overtakable = false;
            }
        }

        if (overtakable)
        {
            
            OvertakePoint();
        }
    }
    
    /// <summary>
    /// This function is only called if the point should be overtaken. It switches the ownerTeam to the new owner.
    /// </summary>
    private void OvertakePoint()
    {
        ownerTeam = ownerTeam switch
        {
            0 => unitsOnThePoint[0].team,
            1 => 2,
            2 => 1,
            _ => 0
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            unitsOnThePoint.Add(unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            unitsOnThePoint.Remove(unit);
        }
    }

    /// <summary>
    /// Asyc method to passivly generate income for the owner of the point, if there is one.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateResourcesForOwner()
    {
        while (true)
        {
            if (ownerTeam == 1)
            {
                blueBase.Gold += generationAmount;
            } else if (ownerTeam == 2)
            {
                redBase.Gold += generationAmount;
            }

            yield return new WaitForSeconds(1);
        }
    }
}
