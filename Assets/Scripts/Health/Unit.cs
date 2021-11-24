using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.AI;

namespace Health
{
    public class Unit : HealthEntity
    {
        public Rigidbody rb;

        public NavMeshAgent nav;
        public Vector3 destPoint;

        private PlayerInputController playerInputController;
        private AIController aiController;

        //private bool isInCombat { get; set; } = false;
        public bool isInCombat = false;

        //private Unit enemy;
        public HealthEntity enemy;

        private double _lastAttackTime = 0;

        public List<Unit> enemiesInSight = new List<Unit>();

        /// <summary>
        /// Adds this unit to some lists of the player and AI controller, depending on which side it is.
        /// </summary>
        private new void Start()
        {
            base.Start();
            // sets the destination to the spawn point
            destPoint = transform.position;

            // add this units to different lists of the controllers depending which side its on
            aiController = FindObjectOfType<AIController>();
            playerInputController = FindObjectOfType<PlayerInputController>();
            if (team == 1)
            {
                playerInputController.AddActiveUnit(this);

                aiController.AddEnemyUnit(this);
            }
            else if (team == 2)
            {
                aiController.AddOwnUnit(this);
            }
        }

        /// <summary>
        /// Calls the Combat and Movement methods depending on if the unit is in combat or not.
        /// </summary>
        private void FixedUpdate()
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

        /// <summary>
        /// If another unit enters the range of this unit it will be checks and added to a list of enemies in the perimeter.
        /// </summary>
        /// <param name="other">The collider of the entering unit</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            if (!other.transform.TryGetComponent(out Unit enter)) return;

            if (enter.team != team)
            {
                enemiesInSight.Add(enter);
            }

            CheckForEnemiesNearYou();
        }
        
        /// <summary>
        /// If another unit exits the range of this unit it will be removed from the list of enemies in the perimeter.
        /// </summary>
        /// <param name="other">The collider of the exiting unit.</param>
        private void OnTriggerExit(Collider other)
        {
            Unit exit = other.transform.GetComponent<Unit>();
            enemiesInSight.Remove(exit);
        }

        /// <summary>
        /// This method handles movement. Also its responsible for a functionality of the AI (remarks).
        /// </summary>
        /// <remarks>The AI has the functionality to first check for units on a point before capturing it and attack them first.
        /// This is something the player does naturally and an AI without it is pretty useless.</remarks>
        /// Only the first line of this method is the one that manages movement. All other lines are for this functionality.
        private void Movement()
        {
            nav.destination = destPoint;
            
            if (team == 1) return;
            // From here one follows a functionality to stop units from walking towards an already captured point.
            // This functionality is only wanted for AI units, since it makes the work of the AI Controller more efficient.
            // Its probably faulty and might belong into the AIController
            if (transform.position == destPoint) return;

            // if this ever causes problems, Physics.OverlapSphereNonAlloc might be worth a try. Documentation is severely lacking here though, so im not using it.
            Collider[] collidersAtUnitDestination = Physics.OverlapSphere(nav.destination, 0.1f);

            // if any kind of enemy unit is found at the destination, you shall continue with walking there
            foreach (var unitCollider in collidersAtUnitDestination)
            {
                if (unitCollider.TryGetComponent(out Unit unit))
                {
                    if (unit.team == 1) return;
                }
            }
            
            foreach (var colliderAtDestination in collidersAtUnitDestination)
            {
                if (colliderAtDestination.TryGetComponent(out ResourcePoint rPoint))
                {
                    // if this is true the destination lies inside the collider of a resource point.
                    if (colliderAtDestination.ClosestPoint(nav.destination) == nav.destination && rPoint.ownerTeam == 2)
                    {
                        destPoint = transform.position;
                    }
                }
            }
            
        }

        /// <summary>
        /// If the unit has a set enemy it will walk towards it. If the enemy is already in range it will stop walking and attack the enemy instead.
        /// </summary>
        private void Combat()
        {
            if (enemy)
            {
                destPoint = enemy.hitbox.ClosestPoint(transform.position);
            }
            else
            {
                CheckForEnemiesNearYou();
            }

            // TODO: Check if this is easier done with NavMeshAgent.stoppingDistance
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

        /// <summary>
        /// The unit attacks its enemy with the given attack speed. This varies if the enemy is a unit or a base.
        /// </summary>
        private void Attack()
        {
            if (_lastAttackTime + attackSpeed <= DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                if (!enemy)
                {
                    isInCombat = false;
                    return;
                }

                if (enemy is Unit)
                {
                    Unit uEnemy = (Unit) enemy;
                    uEnemy.TakeDamage(this, attackDamage);
                }
                else
                {
                    enemy.TakeDamage(attackDamage);
                }

                _lastAttackTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// This method is called if the unit receives damage from another unit.
        /// </summary>
        /// <remarks>The damage is dealt by calling HealthEntity.TakeDamage(), more there.
        /// Also if the unit is not in combat and not moving it is set into combat mode against its attacker.</remarks>
        /// <param name="attacker"></param>
        /// <param name="damage"></param>
        public void TakeDamage(Unit attacker, float damage)
        {
            base.TakeDamage(damage);

            // before the unit is set into combat mode it is checked if the unit has a pending movement command
            // if so, its not set into combat mode until this path is complete
            if (!isInCombat && !IsMoving())
            {
                StartCoroutine(SetIntoCombat(attacker));
            }
        }

        /// <summary>
        /// Sets the unit into combat against its attacker after a half second delay.
        /// </summary>
        /// <remarks>The only cause for the half second delay is so that it wont strike back immediately. So that the
        /// attacker always wins a fight.</remarks>
        /// <param name="attacker">Reference to the attacking unit.</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator SetIntoCombat(Unit attacker)
        {
            yield return new WaitForSeconds(0.5f);
            isInCombat = true;
            enemy = attacker;
        }

        /// <summary>
        /// If this unit is destroyed it should be removed from the list of the player and AI controller that is was placed in during Start().
        /// </summary>
        private void OnDestroy()
        {
            // if this is a player unit, remove it from the list of active units
            if (team == 1)
            {
                playerInputController.RemoveActiveUnit(this);
                aiController.RemoveEnemyUnit(this);
            }
            else if (team == 2)
            {
                aiController.RemoveOwnUnit(this);
            }
        }

        /// <summary>
        /// Determines if this unit is moving or not.
        /// </summary>
        /// <returns>Is the unit moving (true) or not (false).</returns>
        private bool IsMoving()
        {
            return !(nav.remainingDistance <= nav.stoppingDistance + 1);
        }

        /// <summary>
        /// Checks if there are enemies inside the units range and attacks them if so.
        /// </summary>
        private void CheckForEnemiesNearYou()
        {
            if (IsMoving()) return;

            if (enemiesInSight.Count != 0)
            {
                // check for nearest enemy and set into combat against that enemy
                Unit nearestEnemy = null;
                float distanceToNearestEnemy = 25; // placeholder value way slightly above the 20 radius of the trigger

                enemiesInSight.RemoveAll(unit => !unit); // remove all units that where destroyed (are null)
                foreach (Unit enemyinSight in enemiesInSight)
                {
                    if (Vector3.Distance(enemyinSight.transform.position, transform.position) < distanceToNearestEnemy)
                    {
                        nearestEnemy = enemyinSight;
                        distanceToNearestEnemy = Vector3.Distance(enemyinSight.transform.position, transform.position);
                    }
                }

                enemy = nearestEnemy;
                isInCombat = true;
            }
            else
            {
                destPoint = transform.position;
                isInCombat = false;
            }
        }
    }
}
