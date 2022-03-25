using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChair : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] float zOffset = 2;
    [SerializeField] float xOffset = 2;

    private bool isActive = false;


    public void ToggleTheChair()
    {
        if (!isActive)
        {
            Vector3 tempPos = player.transform.position;
            tempPos.y = 0;
            tempPos.z += zOffset;
            tempPos.x += xOffset;
            gameObject.transform.position = tempPos;
            gameObject.SetActive(true);
            isActive = true;
        }
        else
        {
            gameObject.SetActive(false);
            isActive = false;
        }
    }   

}
