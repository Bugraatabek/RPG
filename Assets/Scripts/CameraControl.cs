using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;

    int cameraCounter = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cameraCounter++;
            ChangeCamera();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameraCounter--;
            ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        if (cameraCounter <= -1)
        {
            cameraCounter = 3;
        }
        if (cameraCounter >= 4)
        {
            cameraCounter = 0;
        }
        if (cameraCounter == 0)
        {
            target.SetActive(true);
            target1.SetActive(false);
            target2.SetActive(false);
            target3.SetActive(false);
        }
        if (cameraCounter == 1)
        {
            target.SetActive(false);
            target1.SetActive(true);
            target2.SetActive(false);
            target3.SetActive(false);
        }
        if (cameraCounter == 2)
        {
            target.SetActive(false);
            target1.SetActive(false);
            target2.SetActive(true);
            target3.SetActive(false);
        }
        if (cameraCounter == 3)
        {
            target.SetActive(false);
            target1.SetActive(false);
            target2.SetActive(false);
            target3.SetActive(true);
        }
    }
}
