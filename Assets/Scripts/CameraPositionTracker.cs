using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraPositionTracker : MonoBehaviour
{
    private Vector3 startPosition;
    public TextMeshPro countdownText;
    public float standingThreshold = .35f;

    public GameObject breatheIn;
    public GameObject breatheOut;

    public float breatheInDuration = 2f, breatheOutDuration = 2f, holdDuration = 1f, breatheInDelay = 2f;

    private int state = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Recording start position {transform.position}");
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Camera y: {transform.position.y}");
        //text.text = ""+transform.position.y;

        switch (state)
        {
            case 0:
                if (transform.position.y - startPosition.y >= standingThreshold)
                {
                    countdownText.text = "Breath In";                    
                    state = 1;

                    //TODO: State Change
                }
                break;
            case 1:
                breatheInDelay -= Time.deltaTime;

                countdownText.text = $"Breathing Exercise\n {(int)breatheInDelay+1}";

                if (breatheInDelay <= 0)
                {
                    breatheIn.SetActive(true);
                    state = 2;
                }

                break;
            case 2:
                breatheInDuration -= Time.deltaTime;
                //Debug.Log(breatheInTransitionTime);

                countdownText.text = $"In {(int)breatheInDuration+1}";

                if (breatheInDuration <= 0)
                {
                    breatheIn.SetActive(false);
                    state = 3;
                }
                break;
            case 3:
                holdDuration -= Time.deltaTime;
                countdownText.text = $"Hold {(int)holdDuration+1}";
                if (holdDuration <= 0)
                {
                    breatheOut.SetActive(true);
                    state = 4;
                    //TODO: Change to new game state
                }
                break;
            case 4:
                breatheOutDuration -= Time.deltaTime;
                countdownText.text = $"Out {(int)breatheOutDuration+1}";
                if (breatheOutDuration <= 0)
                {
                    breatheIn.SetActive(false);
                    breatheOut.SetActive(false);

                    //TODO: Change to new game state
                    countdownText.gameObject.SetActive(false);

                    GameManager.Instance.currentState = GameManager.GameState.Analysis;
                    GameManager.Instance.StateHandler();
                    state = 5;
                }
                break;
            case 5:
                break;
        }

                
    }
}
