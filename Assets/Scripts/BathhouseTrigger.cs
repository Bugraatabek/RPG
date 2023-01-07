using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathhouseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            print("Bathhouse is passable");
            GameObject obstacle1 = GameObject.FindWithTag("ObstacleBathhouse");
            GameObject obstacle2 = GameObject.FindWithTag("ObstacleBathhouse2");
            Destroy(obstacle1);
            Destroy(obstacle2);
            Destroy(gameObject);
        }    
    }
}
