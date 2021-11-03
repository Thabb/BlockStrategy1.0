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

    private void OvertakePoint()
    {
        // should take a few seconds of repeated input
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
