using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInputController : MonoBehaviour
{

    public GameObject controlledUnit;

    public Image dragBox;
    private Vector2 _leftMouseDownPosition;
    private Vector2 _leftMouseUpPosition;

    // list of units that exist at that moment
    public List<Unit> _activeUnits;

    void Update()
    {
        MouseClicks();
    }

    private void MouseClicks()
    {
        // left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // set corner 1 for the dragBox
            _leftMouseDownPosition = Input.mousePosition;
            dragBox.gameObject.SetActive(true);

            // under this comment is the stuff needed for a one-click
            // TODO: maybe rewrite this once the drag select stuff is done
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200))
            {
                if (controlledUnit)
                {
                    controlledUnit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(false);
                    controlledUnit = null;
                }
                
                if (hit.transform.gameObject.CompareTag("Unit") && hit.transform.gameObject.GetComponent<Unit>().team == 1)
                {
                    Debug.Log("Unit found!");
                    controlledUnit = hit.transform.gameObject;
                    controlledUnit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(true);
                }
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            dragBox.rectTransform.sizeDelta= new Vector2(Mathf.Abs(_leftMouseDownPosition.x - Input.mousePosition.x), Mathf.Abs(_leftMouseDownPosition.y - Input.mousePosition.y));
            dragBox.rectTransform.position = new Vector2((_leftMouseDownPosition.x + Input.mousePosition.x) / 2, (_leftMouseDownPosition.y + Input.mousePosition.y)/2);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // save mouse up position and deactivate dragBox
            _leftMouseUpPosition = Input.mousePosition;
            dragBox.gameObject.SetActive(false);
            
            // TODO: Check if any untis are in the area of the dragBox
        }
        
        // right mouse click
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 200))
            {
                // transcribe the x and z value of the hit in a new Vector
                Vector3 destination = hit.point;
                destination.y = 1;

                // case: clicked on open ground
                if (hit.transform.gameObject.CompareTag("Ground") && controlledUnit)
                {
                    controlledUnit.GetComponent<Unit>().destPoint = destination;
                }
                
                // TODO: case: clicked on friendly unit or structure
                

                // TODO: case: clicked on enemy unit or structure


                // TODO: case: clicked on enemy structure
                
                
            }
        }
        
    }
    
    public void AddActiveUnit(Unit unit)
    {
        _activeUnits.Add(unit);
    }

    public void RemoveActiveUnit(Unit unit)
    {
        _activeUnits.Remove(unit);
    }
}
