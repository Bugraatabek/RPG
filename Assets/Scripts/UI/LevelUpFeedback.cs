using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpFeedback : MonoBehaviour
{
    [SerializeField] float onTime = 3f;
    [SerializeField] Canvas canvas = null;
    
    public void enableFX()
    {
        canvas.gameObject.SetActive(true);
        StartCoroutine(Deactivate());

    }

    public IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(onTime);
        canvas.gameObject.SetActive(false);

    }
}
