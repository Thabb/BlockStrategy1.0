using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInputController : MonoBehaviour
{

    public List<GameObject> controlledUnits = new List<GameObject>();

    public Image dragBox;
    private Vector3 _leftMouseDownPosition;
    private Vector3 _leftMouseUpPosition;

    // list of units that exist at that moment
    private List<Unit> _activeUnits = new List<Unit>();

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
                if (controlledUnits.Count != 0)
                {
                    foreach (GameObject unit in controlledUnits)
                    {
                        unit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(false);
                    }
                    controlledUnits.Clear();
                }
                
                if (hit.transform.gameObject.CompareTag("Unit") && hit.transform.gameObject.GetComponent<Unit>().team == 1)
                {
                    controlledUnits.Add(hit.transform.gameObject);
                    controlledUnits[0].GetComponent<HealthEntity>().healthBar.gameObject.SetActive(true);
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
            // deactivate dragBox
            dragBox.gameObject.SetActive(false);

            Vector2 min = dragBox.rectTransform.anchoredPosition - (dragBox.rectTransform.sizeDelta / 2);
            Vector2 max = dragBox.rectTransform.anchoredPosition + (dragBox.rectTransform.sizeDelta / 2);

            foreach (Unit unit in _activeUnits)
            {
                Vector3 unitPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (unitPos.x >= min.x && unitPos.x <= max.x && unitPos.y >= min.y && unitPos.y <= max.y)
                {
                    controlledUnits.Add(unit.gameObject);
                    unit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(true);
                }
            }

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
                if (hit.transform.gameObject.CompareTag("Ground") && controlledUnits.Count != 0)
                {
                    foreach (GameObject unit in controlledUnits)
                    {
                        unit.GetComponent<Unit>().destPoint = destination;
                    }
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
    
    private bool IsCBetweenAB (Vector2 A , Vector2 B , Vector2 C ) {
        return Vector2.Dot( (B-A).normalized , (C-B).normalized )<0f && Vector2.Dot( (A-B).normalized , (C-A).normalized )<0f;
    }

}
