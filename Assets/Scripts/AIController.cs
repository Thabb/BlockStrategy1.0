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
