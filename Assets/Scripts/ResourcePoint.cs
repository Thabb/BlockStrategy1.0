using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : MonoBehaviour
{
    // TODO: add reference to some kind of resource handler of the owning team
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GenerateResourcesForOwner());
    }

    private void OvertakePoint()
    {
        // should take a few seconds of repeated input
    }
    
    private IEnumerator GenerateResourcesForOwner()
    {
        while (true)
        {
            // There should be a reference to a resource handler for the owning team
        }
    }
}
