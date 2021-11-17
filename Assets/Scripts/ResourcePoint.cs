using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Health;
using UnityEngine;

public class ResourcePoint : MonoBehaviour
{
    public Base blueBase;
    public Base redBase;
    public int ownerTeam;

    private readonly List<Unit> _unitsOnThePoint = new List<Unit>();

    public float generationAmount;

    private Renderer _colorRenderer;
    
    /// <summary>
    /// Starts the Coroutine for resource generation as well as initializing the renderer and the color.
    /// </summary>
    private void Start()
    {
        StartCoroutine(GenerateResourcesForOwner());

        _colorRenderer = gameObject.GetComponent<Renderer>();
        _colorRenderer.material.color = Color.green;
    }

    /// <summary>
    /// Checks if there are units on the point and assigns the point to them, if they not already own the point and if there are no units of the owning team on the point.
    /// </summary>
    private void FixedUpdate()
    {
        bool overtakable = true;
        
        foreach (Unit unit in _unitsOnThePoint.ToList())
        {
            if (!unit)
            {
                _unitsOnThePoint.Remove(unit);
            }
            else if (unit.team == ownerTeam)
            {
                overtakable = false;
            }
        }

        if (overtakable && _unitsOnThePoint.Count > 0)
        {
            OvertakePoint();
        }
    }
    
    /// <summary>
    /// This function is only called if the point should be overtaken. It switches the ownerTeam to the new owner. Switches the color of the point as well.
    /// </summary>
    private void OvertakePoint()
    {
        ownerTeam = ownerTeam switch
        {
            0 => _unitsOnThePoint[0].team,
            1 => 2,
            2 => 1,
            _ => 0
        };

        switch (ownerTeam)
        {
            case 1:
                _colorRenderer.material.color = Color.blue;
                break;
            case 2:
                _colorRenderer.material.color = Color.red;
                break;
        }
    }

    /// <summary>
    /// Adds every incoming unit to the list of units on the point.
    /// </summary>
    /// <param name="other">Unit that just entered the point.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        
        if (other.TryGetComponent(out Unit unit))
        {
            _unitsOnThePoint.Add(unit);
        }
    }

    /// <summary>
    /// Removes every exiting unit from the list of units on the point.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        if (other.TryGetComponent(out Unit unit))
        {
            _unitsOnThePoint.Remove(unit);
        }
    }

    /// <summary>
    /// Asyc method to passivly generate income for the owner of the point, if there is one.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateResourcesForOwner()
    {
        while (true)
        {
            if (ownerTeam == 1)
            {
                blueBase.ChangeGoldAmount(generationAmount);
            } else if (ownerTeam == 2)
            {
                redBase.ChangeGoldAmount(generationAmount);
            }

            yield return new WaitForSeconds(1);
        }
    }
}
