using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Base enemyBase;
    public Base ownBase;
    
    private List<Unit> ownUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    private Tuple<Vector3, Vector3> dangerZone = new Tuple<Vector3, Vector3>(new Vector3(80,0,-45), new Vector3(-50,0,45));
    
    private String nextUnitToBuild = "Soldier";

    private void Update()
    {
        DecisionMaking();
        UnitBuilding();
    }

    private void DecisionMaking()
    {
        Unit closestUnit = GetClosestEnemyUnit();
        
        if (closestUnit)
        {
            // 1. Clear a circle with a radius of 30 around the base from all enemy units.
            if (Vector3.Distance(closestUnit.transform.position, transform.position) < 30)
            {
                SendTroopsAgainst(closestUnit);
            }
            // 2. Clear the danger zone from all enemy units.
            else if (closestUnit.transform.position.x < dangerZone.Item1.y && closestUnit.transform.position.x > dangerZone.Item2.x && closestUnit.transform.position.z > dangerZone.Item1.z && closestUnit.transform.position.z < dangerZone.Item2.z)
            {
                SendTroopsAgainst(closestUnit);
            }
        }

        // 3. Attack the enemy base.
        else
        {
            SendTroopsAgainst(enemyBase);
        }
    }

    private Unit GetClosestEnemyUnit()
    {
        Unit closestUnit = null;
        float clostestDistance = 1000000000; // 1 billion, a number so high that no real unit should ever be that far away

        foreach (Unit enemyUnit in enemyUnits)
        {
            float distance = Vector3.Distance(transform.position, enemyUnit.transform.position);
            if (distance < clostestDistance)
            {
                clostestDistance = distance;
                closestUnit = enemyUnit;
            }
        }

        return closestUnit;
    }

    private void SendTroopsToPosition(Vector3 destination)
    {
        foreach (Unit ownUnit in ownUnits)
        {
            // send unit on its way
            ownUnit.destPoint = destination;
            
            // get unit out of combat mode
            ownUnit.isInCombat= false;
            ownUnit.enemy = null;
        }
    }

    private void SendTroopsAgainst(HealthEntity enemy)
    {
        foreach (Unit ownUnit in ownUnits)
        {
            ownUnit.isInCombat = true;
            ownUnit.enemy = enemy;
        }
    }

    private List<Unit> GetAllCurrentlyInactiveUnits()
    {
        List<Unit> inactiveUnits = new List<Unit>();
        foreach (Unit ownUnit in ownUnits)
        {
            if (!ownUnit.isInCombat)
            {
                inactiveUnits.Add(ownUnit);
            }
        }

        return inactiveUnits;
    }

    public void AddOwnUnit(Unit unit)
    {
        ownUnits.Add(unit);
    }

    public void RemoveOwnUnit(Unit unit)
    {
        ownUnits.Remove(unit);
    }

    public void AddEnemyUnit(Unit unit)
    {
        enemyUnits.Add(unit);
    }

    public void RemoveEnemyUnit(Unit unit)
    {
        enemyUnits.Remove(unit);
    }

    private void UnitBuilding()
    {
        switch (nextUnitToBuild)
        {
            case "Soldier":
                if (ownBase.GenerateSoldier()) nextUnitToBuild = FindNextUnitToBuild();
                break; 
            case "Archer":
                if (ownBase.GenerateArcher()) nextUnitToBuild = FindNextUnitToBuild();
                break;
            case "Lancer":
                if (ownBase.GenerateLancer()) nextUnitToBuild = FindNextUnitToBuild();
                break;
        }
    }

    private string FindNextUnitToBuild()
    {
        return "Soldier";
    }
}
