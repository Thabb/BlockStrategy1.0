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
                if (hit.transform.gameObject.CompareTag("Unit") && hit.transform.gameObject.GetComponent<Unit>().team == 1)
                {
                    Debug.Log("Unit found!");
                    controlledUnit = hit.transform.gameObject;
                }
            }
        }
    }
}
