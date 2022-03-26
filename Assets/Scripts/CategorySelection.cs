using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelection : MonoBehaviour
{
    public Canvas categorySelection;
    public GameObject peopleGroup, emotionGroup;
    public GameObject finishedPeople, finishedEmotion;
    public bool deepDive = false;
    public bool finishedPeopleClicked = false;
    public bool finishedEmotionClicked = false;
    void Start()
    {
        deepDive = false;
        finishedPeopleClicked = false;
        finishedEmotionClicked = false;

        /*  peopleGroup.gameObject.SetActive(false);
          emotionGroup.gameObject.SetActive(false);*/

    }

    public void DeepDiveOnClick()
    {
        /* deepDive = true;
         peopleGroup.SetActive(true);
         emotionGroup.SetActive(false);*/
        Debug.Log("deep dive is " + deepDive);

        if (!deepDive)
        {
            deepDive = true;
            peopleGroup.SetActive(true);
            emotionGroup.SetActive(false);
        }
        else
        {
            peopleGroup.SetActive(false);
            emotionGroup.SetActive(true);
            deepDive = false;

        }


    }

    public void FinishedPeopleOnClick()
    {
        /*   peopleGroup.SetActive(false);
           emotionGroup.SetActive(true);*/

        if (!finishedPeopleClicked)
        {
            peopleGroup.SetActive(false);
            emotionGroup.SetActive(true);
            finishedPeopleClicked = true;

        }
        else
        {
            peopleGroup.SetActive(true);
            emotionGroup.SetActive(false);
            finishedPeopleClicked = false;

        }

        Debug.Log("finished people is cilcked ");
    }

    public void FinishedEmotionOnClick()
    {
        if (!finishedPeopleClicked)
        {
            finishedPeopleClicked = true;
            peopleGroup.SetActive(false);
            emotionGroup.SetActive(false);
        }
        else
        {
            peopleGroup.SetActive(true);
            emotionGroup.SetActive(true);
            finishedPeopleClicked = false;

        }


        Debug.Log("emotion people is cilcked ");

    }



    private void OnDestroy()
    {
        // problem with this one 
        deepDive = false;
        finishedPeopleClicked = false;
        finishedEmotionClicked = false;

    }

}