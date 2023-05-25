using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class TimedLife : MonoBehaviour
{
    WaitForSeconds seconds = new WaitForSeconds(1f);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }
    private void OnEnable()
    {
        StartCoroutine(Die());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Die()
    {
        yield return seconds;
        KillPS();
    }

    void KillPS()
    {
        LeanPool.Despawn(this.gameObject);
    }

}
