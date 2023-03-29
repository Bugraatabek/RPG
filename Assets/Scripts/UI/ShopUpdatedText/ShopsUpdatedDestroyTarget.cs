using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopsUpdatedDestroyTarget : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy;
    public void DestroyTheTarget()
    {
        Destroy(targetToDestroy, 6f);
    }
}