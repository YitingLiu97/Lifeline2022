using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChair : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] float zOffset = 2;
    [SerializeField] float xOffset = 2;
    [SerializeField] float yOffset = -2;

    public void ChairHandler()
    {
        GameManager.Instance.currentState = GameManager.GameState.ChairOnly;
        GameManager.Instance.StateHandler();
        MoveChair();
    }
    
    private void MoveChair()
    {
        Vector3 tempPos = player.transform.position;
        tempPos.z += zOffset;
        tempPos.x += xOffset;
        tempPos.y += yOffset;
        gameObject.transform.position = tempPos;
    }

}
