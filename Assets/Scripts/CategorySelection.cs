using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelection : MonoBehaviour
{
    public Canvas categorySelection;
    public GameObject peopleGroup, emotionGroup;
    public Button finishedPeople, finishedEmotion;
    public bool deepDive = false;
    public bool finishedPeopleClicked = false;
    public bool finishedEmotionClicked = false;
    void Start()
    {
        deepDive = false;
        finishedPeopleClicked = false;
        finishedEmotionClicked = false;
        /*        peopleGroup.gameObject.SetActive(false);
                emotionGroup.gameObject.SetActive(false);*/

    }

    // dummy key to drag the bubbble 
    // just focus on one bubble for now 

    // show the category selections 
    // show people 
    // once clicked finished people 
    // show emotions 
    // once clicked the finished emotions 
    // turn category off, show the bubble again 
    // if deepdive is clicked, do the category thing 

    public void DeepDiveOnClick()
    {
        /*  peopleGroup.SetActive(true);
          emotionGroup.SetActive(false);*/
        Debug.Log("deep dive is " + deepDive);
        deepDive = true;
/*
        peopleGroup.SetActive(true);
        emotionGroup.SetActive(false);*/
        /* finishedPeopleClicked = true;

         finishedEmotionClicked = false;*/
        /*Debug.Log($"people group is {peopleGroup.name}");
        Debug.Log($"people parent is {peopleGroup.gameObject.transform.parent.name}");
        Debug.Log($"emotionGroup is {emotionGroup.name}");
        Debug.Log("people group enabled is " + peopleGroup.activeSelf);
        Debug.Log("deep dive is cilcked ");
*/
        // CategorySelectionProcess(categorySelection, peopleGroup, emotionGroup, finishedPeople, finishedEmotion);
    }

    public void FinishedPeopleOnClick()
    {
        finishedPeopleClicked = true;
        finishedEmotionClicked = false;
        Debug.Log("finished people is cilcked ");


    }

    public void FinishedEmotionOnClick()
    {
        finishedEmotionClicked = true;


        Debug.Log("emotion people is cilcked ");

    }



    private void OnDestroy()
    {
        // problem with this one 
        deepDive = false;
        finishedPeopleClicked = false;
        finishedEmotionClicked = false;

    }
    void Update()
    {

        if (deepDive)
        {
            peopleGroup.SetActive(true);
            emotionGroup.SetActive(false);

           /* if (finishedPeopleClicked)
            {
                peopleGroup.SetActive(false);
                emotionGroup.SetActive(true);

            }
            else
            {

                peopleGroup.gameObject.SetActive(true);
                emotionGroup.gameObject.SetActive(false);
            }

            if (finishedEmotionClicked)
            {
                peopleGroup.gameObject.SetActive(false);
                emotionGroup.gameObject.SetActive(false);

            }*/
        }
        else
        {

            peopleGroup.SetActive(false);
            emotionGroup.SetActive(false);


        }




    }
}
