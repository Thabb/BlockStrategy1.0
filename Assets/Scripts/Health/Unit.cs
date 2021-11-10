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

        private void OnTriggerExit(Collider other)
        {
            Unit exit = other.transform.GetComponent<Unit>();
            enemiesInSight.Remove(exit);
        }

        private void Movement()
        {
            nav.destination = destPoint;
        }

        private void Combat()
        {
            if (enemy)
            {
                destPoint = enemy.transform.position;
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

        private void Attack()
        {
            if (_lastAttackTime + attackSpeed <= DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                if (!enemy) return;

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

        public void TakeDamage(Unit attacker, float damage)
        {
            // TODO: include proper damage calculation
            base.TakeDamage(damage);

            if (!isInCombat)
            {
                // before the unit is set into combat mode it is checked if the unit has a pending movement command
                // if so, its not set into combat mode until this path is complete
                if (!IsMoving())
                {
                    StartCoroutine(SetIntoCombat(attacker));
                }
            }
        }

        private IEnumerator SetIntoCombat(Unit attacker)
        {
            yield return new WaitForSeconds(0.5f);
            isInCombat = true;
            enemy = attacker;
        }

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

        private bool IsMoving()
        {
            return !(nav.remainingDistance <= nav.stoppingDistance + 1);
        }

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
