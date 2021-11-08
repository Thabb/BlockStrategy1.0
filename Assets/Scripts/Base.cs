using System.Collections;
using TMPro;
using UnityEngine;

public class Base : HealthEntity
{
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    public GameObject lancerPrefab;

    private float Gold = 200;
    public TMP_Text goldCounter;

    private new void Start()
    {
        base.Start();
        StartCoroutine(HealthRegeneration());
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
    
    private void Update()
    {
        UnitGeneration();
    }

    private void UnitGeneration()
    {
        // check if spawn position is free
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        // spawn a soldier if the button was pressed
        if (team == 1 && Input.GetButtonDown("BuildSoldier"))
        {
            GenerateSoldier();
        }
        else if (team == 1 && Input.GetButtonDown("BuildArcher"))
        {
            GenerateArcher();
        }
        else if (team == 1 && Input.GetButtonDown("BuildLancer"))
        {
            GenerateLancer();
        }
        else if (team == 2 && Input.GetKeyDown(KeyCode.A))
        {
            GenerateSoldier();
        }
        else if (team == 2 && Input.GetKeyDown(KeyCode.S))
        {
            GenerateArcher();
        }
        else if (team == 2 && Input.GetKeyDown(KeyCode.D))
        {
            GenerateLancer();
        }
    }

    public void GenerateSoldier()
    {
        // check if spawn position is free
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 30)
        {
            Instantiate(soldierPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
            ChangeGoldAmount(-30);
        }
        else
        {
            Debug.Log("Not enough gold to build soldier!");
        }
    }
    
    public void GenerateArcher()
    {
        // check if spawn position is free
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 50)
        {
            Instantiate(archerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
            ChangeGoldAmount(-50);
        }
        else
        {
            Debug.Log("Not enough gold to build archer!");
        }
    }
    
    public void GenerateLancer()
    {
        // check if spawn position is free
        // the position is the position in front of the the base, slightly offset on the y-axis to avoid spawning in the ground
        if (Physics.CheckSphere((transform.position + transform.forward * 5) + new Vector3(0, 1, 0), 0.1f)) return;
        
        if (Gold >= 60)
        {
            Instantiate(lancerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
            ChangeGoldAmount(-60);
        }
        else
        {
            Debug.Log("Not enough gold to build lancer!");
        }
    }

    public void ChangeGoldAmount(float gold)
    {
        Gold += gold;
        goldCounter.text = "Gold: " + Gold;
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
