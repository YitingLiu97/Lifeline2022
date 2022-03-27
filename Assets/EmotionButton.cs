using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionButton : MonoBehaviour
{

    public void EmotionHandler()
    {
        GameManager.Instance.currentState = GameManager.GameState.EmotionOnly;
        GameManager.Instance.StateHandler();
    }

}
