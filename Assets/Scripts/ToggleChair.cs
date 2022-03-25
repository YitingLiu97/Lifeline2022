using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChair : MonoBehaviour
{


    // 
    [SerializeField] Transform player;
    private bool isActive = false;

    public void ToggleTheChair()
    {
        if (!isActive)
        {
            Vector3 tempPos = player.transform.position;
            tempPos.y = 0;
            tempPos.z += 2;
            tempPos.x += 2;
            gameObject.transform.position = tempPos;
            gameObject.SetActive(true);
            isActive = true;
        }
        else
        {
            gameObject.SetActive(false);

        }
    }   

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
