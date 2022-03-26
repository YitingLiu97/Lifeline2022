using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get 
        {
            if (_instance == null)
                Debug.LogError("Game Manager null!");
            return _instance;
        }
    }

    //references to the objects
    [SerializeField] GameObject Logo;
    [SerializeField] GameObject Graph;
    [SerializeField] GameObject Moments;
    [SerializeField] GameObject Chair;
    [SerializeField] GameObject EMDR;
    [SerializeField] GameObject EmotionWheel;

    private void Awake()
    {
        _instance = this;   
    }

    public enum GameState { Initial, Transition, Analysis, Discovery, Emdr, ChairOnly, EmotionOnly}
    public GameState currentState; 

    private void Start()
    {
        currentState = GameState.Initial;
        StateHandler();
    }

    public void StateHandler()
    {
        if (currentState == GameState.Initial)
        {
            //turn everything else off except the logo and the moments, in their OG position
            Logo.SetActive(true);
            Graph.SetActive(false);
            Moments.SetActive(true);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);

        }
        else if(currentState == GameState.Transition)
        {
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(true);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.Analysis)
        {
            Logo.SetActive(false);
            Graph.SetActive(true);
            Moments.SetActive(true);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.Discovery)
        {
            //one moment for discovery
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.ChairOnly)
        {
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(true);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.Emdr)
        {
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(true);
        }
        else if (currentState == GameState.EmotionOnly)
        {
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(true);
            EMDR.SetActive(false);
        }
    }




}
