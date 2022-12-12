using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteCanvas;
    [SerializeField] private GameObject messageUI;

    private bool _isActivated = false;

    public void Activate()
    {
        Debug.Log("Finish activated");
        _isActivated = true;
        messageUI.SetActive(false);
    }

    public void FinishLevel()
    {
        if (_isActivated)
        {
            gameObject.SetActive(false);
            levelCompleteCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            messageUI.SetActive(true);
        }
    }
}
