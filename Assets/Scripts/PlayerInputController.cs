using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{

    public GameObject controlledUnit;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        MouseClicks();
    }

    private void MouseClicks()
    {
        // left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200))
            {
                if (controlledUnit)
                {
                    controlledUnit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(false);
                }
                
                if (hit.transform.gameObject.CompareTag("Unit") && hit.transform.gameObject.GetComponent<Unit>().team == 1)
                {
                    Debug.Log("Unit found!");
                    controlledUnit = hit.transform.gameObject;
                    controlledUnit.GetComponent<HealthEntity>().healthBar.gameObject.SetActive(true);
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
                if (hit.transform.gameObject.CompareTag("Ground"))
                {
                    controlledUnit.GetComponent<Unit>().destPoint = destination;
                }
                
                // TODO: case: clicked on friendly unit or structure
                

                // TODO: case: clicked on enemy unit or structure


                // TODO: case: clicked on enemy structure
                
                
            }
        }
    }
}
