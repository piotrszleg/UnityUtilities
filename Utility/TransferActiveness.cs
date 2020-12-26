using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferActiveness : MonoBehaviour {

    public GameObject[] targets;

    private void OnEnable()
    {
        foreach (var target in targets)
        {
            target.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (var target in targets)
        {
            target.SetActive(false);
        }
    }
}
