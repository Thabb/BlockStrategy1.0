using System;
using System.Collections.Generic;
using Health;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Controller
{
    public class AIController : MonoBehaviour
    {
        public Base enemyBase;
        public Base ownBase;

        private List<Unit> ownUnits = new List<Unit>();
        private List<Unit> enemyUnits = new List<Unit>();

        private Tuple<Vector3, Vector3> dangerZone =
            new Tuple<Vector3, Vector3>(new Vector3(80, 0, -45), new Vector3(-50, 0, 45));

        private String nextUnitToBuild = "Soldier";
        
        public List<ResourcePoint> nearResourcePoints;
        public List<ResourcePoint> farResourcePoints;

        private void Update()
        {
            DecisionMaking();
            UnitBuilding();
        }

        private void DecisionMaking()
        {
            Unit closestUnit = GetClosestEnemyUnit();
            ResourcePoint nextNearResourcePoint = GetNextNearResourcePoint();
            ResourcePoint nextFarResourcePoint = GetNextFarResourcePoint();
            
            // 1. Clear a circle with a radius of 30 around the base from all enemy units.
            if (closestUnit && Vector3.Distance(closestUnit.transform.position, ownBase.transform.position) < 30)
            {
                SendTroopsAgainst(closestUnit);
            }
            // 2. Conquer the near resource points (behind the own base and in the middle, in that order).
            else if (nextNearResourcePoint)
            {
                // 2.1 But before just walking towards the point, kill the enemy units on it first.
                
                // check for enemies on/near the point (function that returns an enemy)
                Unit possibleEnemy = CheckForEnemiesOnResourcePoint(nextNearResourcePoint);
                if (possibleEnemy)
                {
                    // attack that enemy
                    SendTroopsAgainst(possibleEnemy);
                }
                else
                {
                    // only walk onto the point if its free of enemies
                    SendTroopsToPosition(nextNearResourcePoint.transform.position);
                }
            }
            // 3. Clear the danger zone from all enemy units.
            else if (closestUnit && 
                     closestUnit.transform.position.x < dangerZone.Item1.y &&
                     closestUnit.transform.position.x > dangerZone.Item2.x &&
                     closestUnit.transform.position.z > dangerZone.Item1.z &&
                     closestUnit.transform.position.z < dangerZone.Item2.z)
            {
                SendTroopsAgainst(closestUnit);
            }
            // 4. Conquer the resource points behind the enemy base.
            else if (nextFarResourcePoint)
            {
                // 4.1 TODO: But before just walking towards the point, kill the enemy units on it first.
                
                // check for enemies on/near the point (function that returns an enemy)
                Unit possibleEnemy = CheckForEnemiesOnResourcePoint(nextFarResourcePoint);
                if (possibleEnemy)
                {
                    // attack that enemy
                    SendTroopsAgainst(possibleEnemy);
                }
                else
                {
                    // only walk onto the point if its free of enemies
                    SendTroopsToPosition(nextFarResourcePoint.transform.position);
                }
            }
            // 5. Attack the enemy base.
            else
            {
                SendTroopsAgainst(enemyBase);
            }
        }

        private Unit GetClosestEnemyUnit()
        {
            Unit closestUnit = null;
            float
                clostestDistance =
                    1000000000; // 1 billion, a number so high that no real unit should ever be that far away

            foreach (Unit enemyUnit in enemyUnits)
            {
                float distance = Vector3.Distance(ownBase.transform.position, enemyUnit.transform.position);
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
            List<Unit> inactiveUnits = GetAllCurrentlyInactiveUnits();
            foreach (Unit unit in inactiveUnits)
            {
                // send unit on its way
                unit.destPoint = destination;

                // get unit out of combat mode
                unit.isInCombat = false;
                unit.enemy = null;
            }
        }

        private void SendTroopsAgainst(HealthEntity enemy)
        {
            List<Unit> inactiveUnits = GetAllCurrentlyInactiveUnits();
            foreach (Unit unit in inactiveUnits)
            {
                unit.isInCombat = true;
                unit.enemy = enemy;
            }
        }

        private List<Unit> GetAllCurrentlyInactiveUnits()
        {
            List<Unit> inactiveUnits = new List<Unit>();
            foreach (Unit ownUnit in ownUnits)
            {
                if (!ownUnit.isInCombat && ownUnit.nav.remainingDistance < 1)
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

        private ResourcePoint GetNextNearResourcePoint()
        {
            foreach (ResourcePoint rPoint in nearResourcePoints)
            {
                if (rPoint.ownerTeam != 2) return rPoint;
            }

            return null;
        }
        private ResourcePoint GetNextFarResourcePoint()
        {
            foreach (ResourcePoint rPoint in farResourcePoints)
            {
                if (rPoint.ownerTeam != 2) return rPoint;
            }

            return null;
        }

        private Unit CheckForEnemiesOnResourcePoint(ResourcePoint resourcePoint)
        {
            foreach (var enemyUnit in enemyUnits)
            {
                if (Vector3.Distance(enemyUnit.transform.position, resourcePoint.transform.position) < 15)
                {
                    return enemyUnit;
                }
            }

            return null;
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

        /// <summary>
        /// Decides which Unit should be build next, after an unit was build
        /// </summary>
        /// <remarks>
        /// Decision making: Based on the distance the closest enemy unit has to the base.
        /// If there are enemies in a radius of under 20, soldiers/lancers will be build 50/50.
        /// If there are enemies outside of that radius soldiers/archers will be build 50/50.
        /// And if there are no more enemy units but the base, only archers will be build to block in the enemy.
        /// To top this of with some randomization, 1/3 of the units will always be build by chance.
        /// </remarks>
        /// <returns></returns>
        private string FindNextUnitToBuild()
        {
            float random = Random.value;
            if (random > 0.33)
            {
                Unit closestUnit = GetClosestEnemyUnit();

                // if there are no enemy units, just spam archers and block the enemy in
                if (!closestUnit) return "Archer";

                // check if there is an unit near to the base, if yes build soldiers/lancers
                if (Vector3.Distance(closestUnit.transform.position, ownBase.transform.position) <= 20)
                {
                    if (random > 0.66)
                    {
                        return "Soldier";
                    }
                    else
                    {
                        return "Lancer";
                    }
                }
                else // if there is no unit close to the base build soldiers/archers
                {
                    if (random > 0.66)
                    {
                        return "Soldier";
                    }
                    else
                    {
                        return "Archer";
                    }
                }
            }
            else // randomization, 1/3 chance to build whatever
            {
                if (random > 0.22)
                {
                    return "Soldier";
                }
                else if (random > 0.11)
                {
                    return "Archer";
                }
                else
                {
                    return "Lancer";
                }
            }
        }
    }
}
