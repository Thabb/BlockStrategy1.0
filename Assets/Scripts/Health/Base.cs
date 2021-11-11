using System.Collections;
using TMPro;
using UnityEngine;

namespace Health
{
    public class Base : HealthEntity
    {
        public GameObject soldierPrefab;
        public GameObject archerPrefab;
        public GameObject lancerPrefab;

        public float Gold { get; set; } = 0;
        public TMP_Text goldCounter;

        private new void Start()
        {
            base.Start();
            StartCoroutine(HealthRegeneration());
            ChangeGoldAmount(200);
        }

        private IEnumerator HealthRegeneration()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (currentHealth + 10 <= maxHealth)
                {
                    currentHealth += 10;
                    healthBar.SetHealth(this);
                }
                else if (currentHealth < maxHealth && currentHealth + 10 > maxHealth)
                {
                    currentHealth = maxHealth;
                    healthBar.SetHealth(this);
                }
            }
        }

        public bool GenerateSoldier()
        {
            // check if spawn position is free
            Vector3 checkPosition = GetUnitGenerationPosition();
            if (checkPosition == new Vector3()) return false;

            if (Gold >= 30)
            {
                Instantiate(soldierPrefab, checkPosition, Quaternion.identity);
                ChangeGoldAmount(-30);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GenerateArcher()
        {
            // check if spawn position is free
            Vector3 checkPosition = GetUnitGenerationPosition();
            if (checkPosition == new Vector3()) return false;

            if (Gold >= 50)
            {
                Instantiate(archerPrefab, checkPosition, Quaternion.identity);
                ChangeGoldAmount(-50);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GenerateLancer()
        {
            // check if spawn position is free
            Vector3 checkPosition = GetUnitGenerationPosition();
            if (checkPosition == new Vector3()) return false;

            if (Gold >= 60)
            {
                Instantiate(lancerPrefab, checkPosition, Quaternion.identity);
                ChangeGoldAmount(-60);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Vector3 GetUnitGenerationPosition()
        {
            Vector3 checkPosition;
            for (float turnDegrees = 0; turnDegrees < 360; turnDegrees += 18)
            {
                checkPosition = (transform.position + (Quaternion.Euler(0, turnDegrees, 0) * transform.forward * 5)) + new Vector3(0, 1, 0);
                if (!Physics.CheckSphere(checkPosition, 0.1f))
                {
                    return checkPosition;
                }
            }

            return new Vector3();
        }

        /// <summary>
        /// Only exists as a callable function for buttons.
        /// </summary>
        /// <remarks>Yes, this is ugly!</remarks>
        public void BuildSoldier()
        {
            GenerateSoldier();
        }

        /// <summary>
        /// Only exists as a callable function for buttons.
        /// </summary>
        /// <remarks>Yes, this is ugly!</remarks>
        public void BuildArcher()
        {
            GenerateArcher();
        }

        /// <summary>
        /// Only exists as a callable function for buttons.
        /// </summary>
        /// <remarks>Yes, this is ugly!</remarks>
        public void BuildLancer()
        {
            GenerateLancer();
        }

        public void ChangeGoldAmount(float gold)
        {
            Gold += gold;
            if (team == 1) goldCounter.text = "Gold: " + Gold;
        }

        private void OnDestroy()
        {
            // TODO: If one base was destroyed the game should end and depending on whos base its was the player will see a "You won" or "You lost" message (UI)
            if (team == 1)
            {
                Debug.Log("YOU LOST!");
            }
            else if (team == 2)
            {
                Debug.Log("YOU WON!");
            }

            Application.Quit();
        }
    }
}
