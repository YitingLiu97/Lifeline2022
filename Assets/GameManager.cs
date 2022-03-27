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
    [SerializeField] AudioSource Music;
    [SerializeField] Bubble Bubble;

    [SerializeField] int loadSeconds = 3;

    private void Awake()
    {
        _instance = this;
        Music.Play();
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
     /*   switch (currentState) {
            case GameState.Initial:
                CheckandPlay();
                Logo.SetActive(true);
                Graph.SetActive(false);
                Moments.SetActive(true);
                Chair.SetActive(false);
                EmotionWheel.SetActive(false);
                EMDR.SetActive(false);
                break;

        
        
        
        }*/
        if (currentState == GameState.Initial)
        {
            CheckandPlay();
            Logo.SetActive(true);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
            StartCoroutine(MomentWait());

            

        }
        else if(currentState == GameState.Transition)
        {
            CheckandPlay();
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(true);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.Analysis)
        {
            Bubble.SendBubblesToGraphPosition();
            CheckandPlay();
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
            CheckandPlay();
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.ChairOnly)
        {

            CheckandPlay();
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(true);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(false);
        }
        else if (currentState == GameState.Emdr)
        {
            Music.Pause();
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(false);
            EMDR.SetActive(true);
        }
        else if (currentState == GameState.EmotionOnly)
        {
            CheckandPlay();
            Logo.SetActive(false);
            Graph.SetActive(false);
            Moments.SetActive(false);
            Chair.SetActive(false);
            EmotionWheel.SetActive(true);
            EMDR.SetActive(false);
        }
    }

    private void CheckandPlay()
    {
        if (!Music.isPlaying)
        {
            Music.Play();
        }
    }

    private IEnumerator MomentWait()
    {
        yield return new WaitForSeconds(loadSeconds);
        Moments.SetActive(true);
        Logo.SetActive(false);
    }



}
