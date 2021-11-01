using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private List<Unit> ownUnits = new List<Unit>();
    private List<Unit> enemyUnits = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Unit CheckForClosestUnit()
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
}