using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalysisButton : MonoBehaviour
{

    public void AnalysisHandler()
    {
        GameManager.Instance.currentState = GameManager.GameState.Analysis;
        GameManager.Instance.StateHandler();
    }

}
