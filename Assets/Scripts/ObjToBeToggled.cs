using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjToBeToggled : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] public GameObject ToggledObject;

    public void Toggle()
    {
        if (!isActive)
        {
            ToggledObject.SetActive(true);
            isActive = true;
        }
        else
        {
            ToggledObject.SetActive(false);
            isActive = false;
        }
    }
}
