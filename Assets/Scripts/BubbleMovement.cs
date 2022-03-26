using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public GameObject bubbleModel;
    public float bubbleMovingTime;
    public float buttonToBubbleDist = 0f;
    public bool isSelected = false;
    public Vector3 buttonPosition;

    public void ButtonMoveToBubble(PressableButton button)
    {
        buttonPosition = button.gameObject.transform.position;
        isSelected = true;
        Debug.Log("start moving");
        Debug.Log("button moves to bubble" + button.name);
     
    }


    Vector3 LerpPosition(Vector3 fromPos, Vector3 toPos, float t)
    {

        float x = Mathf.Lerp(fromPos.x, toPos.x, t);
        float y = Mathf.Lerp(fromPos.y, toPos.y, t);
        float z = Mathf.Lerp(fromPos.z, toPos.z, t);

        toPos = new Vector3(x, y, z);

        return toPos;



    }
    private void Update()
    {

        if (isSelected) {

            buttonPosition = LerpPosition(buttonPosition, bubbleModel.transform.position, bubbleMovingTime);

        }



        /* if (Vector3.Distance(buttonPosition, bubbleModel.transform.position) < buttonToBubbleDist)
         {
             buttonPosition = bubbleModel.transform.position;

         }
         else
         {
             buttonPosition = LerpPosition(buttonPosition, bubbleModel.transform.position, bubbleMovingTime);

         }*/

        // Debug.Log("button position is " + buttonPosition);
    }
}
