using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy;
    public void DestroyTheTarget()
    {
        Destroy(targetToDestroy);
    }
}
