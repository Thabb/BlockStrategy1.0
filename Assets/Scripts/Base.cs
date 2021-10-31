using System;
using UnityEngine;
using UnityEngine.Android;

public class Base : HealthEntity
{
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    public GameObject lancerPrefab;

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
        if (Input.GetButtonDown("BuildSoldier"))
        {
            GenerateSoldier();
        }
        else if (Input.GetButtonDown("BuildArcher"))
        {
            GenerateArcher();
        }
        else if (Input.GetButtonDown("BuildLancer"))
        {
            GenerateLancer();
        }
    }

    private void GenerateSoldier()
    {
        Instantiate(soldierPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
    }
    
    private void GenerateArcher()
    { 
        Instantiate(archerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
    }
    
    private void GenerateLancer()
    {
        Instantiate(lancerPrefab, (transform.position + transform.forward * 5) + new Vector3(0, 1, 0), Quaternion.identity);
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
