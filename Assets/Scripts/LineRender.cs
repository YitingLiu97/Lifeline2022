using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    [SerializeField] List<GameObject> bubbles;

    private LineRenderer bubbleLine;
    private Vector3[] bubblePositions;


    void Start()
    {
        bubblePositions = new Vector3[bubbles.Count];
        SetBubbleLinePositions();
        bubbleLine = GetComponent<LineRenderer>(); 
        DrawBubbleLine();

        bubbleLine.startWidth = .2f;
        bubbleLine.endWidth = .2f;
        bubbleLine.numCornerVertices = 90; 
        bubbleLine.numCapVertices = 90;
    }

    void DrawBubbleLine()
    {
        bubbleLine.positionCount = bubbles.Count;
        bubbleLine.SetPositions(bubblePositions);
    }

    void SetBubbleLinePositions()
    {
        int index = 0;
        foreach (GameObject bubble in bubbles) {
            //Vector3 bubbleTemp = bubble.transform.position;
            //bubbleTemp.y -= 0.3f;  
            bubblePositions[index] = bubble.transform.position;
            index++;
        }
    }

}
