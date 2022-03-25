using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelection : MonoBehaviour
{
    public Canvas categorySelection;
    public GameObject peopleGroup, emotionGroup;
    public Button finishedPeople, finishedEmotion;

    void Start()
    {
        categorySelection.gameObject.SetActive(true);
        peopleGroup.gameObject.SetActive(true);
        emotionGroup.gameObject.SetActive(false);

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
        CategorySelectionProcess(categorySelection, peopleGroup, emotionGroup, finishedPeople, finishedEmotion);
    }

    public void CategorySelectionProcess(Canvas categorySelection, GameObject peopleGroup, GameObject emotionGroup, Button finishedPeople, Button finishedEmotion)
    {
        // not showing the category system 
        categorySelection.gameObject.SetActive(true);
        peopleGroup.gameObject.SetActive(true);
        emotionGroup.gameObject.SetActive(false);

        Debug.Log("category selection "+categorySelection.enabled);

        finishedPeople.onClick.AddListener(() =>
        {
            Debug.Log("finished people is cilcked ");
          //  categorySelection.gameObject.SetActive(true);
            peopleGroup.gameObject.SetActive(false);
            emotionGroup.gameObject.SetActive(true);
        });


        finishedEmotion.onClick.AddListener(() =>
        {
            Debug.Log("emotion people is cilcked ");
          /*  peopleGroup.gameObject.SetActive(false);
            emotionGroup.gameObject.SetActive(false);*/
            categorySelection.gameObject.SetActive(false); //bubble would show up
           
        });

    }

    private void OnDestroy()
    {
        categorySelection.gameObject.SetActive(false);
        peopleGroup.gameObject.SetActive(false);
        emotionGroup.gameObject.SetActive(false);
    }
    void Update()
    {

    }
}
