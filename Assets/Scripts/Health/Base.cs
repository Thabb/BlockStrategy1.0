using System.Collections;
using TMPro;
using UnityEngine;

namespace Health
{
    /// <summary>
    /// The base is responsible for generating the units the player wants to build and manages the amount of gold a player has in their possession.
    /// </summary>
    public class Base : HealthEntity
    {
        public GameObject soldierPrefab;
        public GameObject archerPrefab;
        public GameObject lancerPrefab;

        public float Gold { get; set; } = 0;
        public TMP_Text goldCounter;

        /// <summary>
        /// Starts the Coroutine for health regeneration and sets the amount of start gold.
        /// </summary>
        private new void Start()
        {
            base.Start();
            StartCoroutine(HealthRegeneration());
            ChangeGoldAmount(200);
        }

        /// <summary>
        /// An asynchronous function with the sole responsibility of regenerating the bases health, 10 points every second.
        /// </summary>
        /// <returns>The necessary IEnumerator needed for a Coroutine.</returns>
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

        /// <summary>
        /// Generates a soldier unit at an empty space around the base.
        /// </summary>
        /// <returns>True if the generation was successful, false if there was a problem, either with the amount of gold or the available space.</returns>
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

        /// <summary>
        /// Generates an archer unit at an empty space around the base.
        /// </summary>
        /// <returns>True if the generation was successful, false if there was a problem, either with the amount of gold or the available space.</returns>
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

        /// <summary>
        /// Generates a lancer unit at an empty space around the base.
        /// </summary>
        /// <returns>True if the generation was successful, false if there was a problem, either with the amount of gold or the available space.</returns>
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

        /// <summary>
        /// Checks if there is an empty space around the base to build a new unit at.
        /// </summary>
        /// <returns>An empty position where the unit should be build. Return value equals new Vector() if there is no free position.</returns>
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

        /// <summary>
        /// Changes the amount of gold the player/ai has. This also automatically changes the UI gold counter if it is the player.
        /// </summary>
        /// <param name="gold"></param>
        public void ChangeGoldAmount(float gold)
        {
            Gold += gold;
            if (team == 1) goldCounter.text = "Gold: " + Gold;
        }

        /// <summary>
        /// Displays a win/loose message, depending on which base was destroyed.
        /// </summary>
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
        }
    }
}
