using System.Collections;
using TMPro;
using UnityEngine;

public class Base : HealthEntity
{
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    public GameObject lancerPrefab;

    public float Gold { get; set; }= 0;
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
            } else if (currentHealth < maxHealth && currentHealth + 10 > maxHealth)
            {
                currentHealth = maxHealth;
                healthBar.SetHealth(this);
            }
        }
    }

    public bool GenerateSoldier()
    {
        // check if spawn position is free
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 30)
        {
            Instantiate(soldierPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
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
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 50)
        {
            Instantiate(archerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
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
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 60)
        {
            Instantiate(lancerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
            ChangeGoldAmount(-60);
            return true;
        }
        else
        {
            return false;
        }
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
        } else if (team == 2)
        {
            Debug.Log("YOU WON!");
        }
        Application.Quit();
    }
}
