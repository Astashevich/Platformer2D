using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private bool isActivated = false;

    public void Activate()
    {
        Debug.Log("Finish activated");
        isActivated = true;
    }

    public void FinishLevel()
    {
        if (isActivated) gameObject.SetActive(false);
    }
}
