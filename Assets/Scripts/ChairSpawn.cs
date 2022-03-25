using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSpawn : MonoBehaviour
{
    [SerializeField] private GameObject chair;
    [SerializeField] private GameObject player;
    private bool _isChair = false;

    private Transform spawnPos;

    public void ToggleChair()
    {
        if (!_isChair)
        {
            // if the chair is not active, then spawn chair
            CreateAndSpawnChair();
        }
        else
        {
            DeleteChair();
        }
        
    }

    private void CreateAndSpawnChair()
    {
        Vector3 tempPos = player.transform.position; //get player position
        tempPos.x += 1; //modify position to be in front of player
        tempPos.z += 1;
        tempPos.y = 0;  
        Instantiate(chair, tempPos, player.transform.rotation);  //spawn chair
        _isChair = true;
    }

    private void DeleteChair()
    {
        Destroy(gameObject);
        _isChair = false;
    }

}
