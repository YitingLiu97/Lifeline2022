using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


public class Bubble : MonoBehaviour
{

    [Serializable]
    public class BubbleData
    {
        public string Moment;
        [Range(-10, 10)]
        public int impactValue;
        public int age;
        public List<string> people = new List<string>();
        public List<string> emotions = new List<string>();
    }

    public List<string> peopleList = new List<string>();
    public List<string> emotionList = new List<string>();



    [Serializable]
    public class AllBubbleData
    {
        public List<BubbleData> bubbleDatas = new List<BubbleData>();

    }
    public const int CATEGORY_SIZE = 6;
    public const int RADAR_MAX_VALUE = 10;
    public GameObject bubblePrefab;
    public GameObject buttonPrefab;
    public List<GameObject> buttonPrefabs;
    public List<GameObject> bubblePrefabs;
    /*    public List<GameObject> radarChartPrefabs;
        public List<RadarChart> currentRadarPrefabs;*/
    public AllBubbleData abData = new AllBubbleData();


    // Start is called before the first frame update
    void Start()
    {
        ReadFromFile();
        CreateBubbles();
        //  PopulateListForPeopleAndEmotions();

        for (int i = 0; i < buttonPrefabs.Count; i++)
        {
            Button btn = buttonPrefabs[i].GetComponent<Button>();
            btn.onClick.AddListener(() => OnClicked(btn));
        }

        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            abData.bubbleDatas[i].people = AvoidDuplicatesInList(abData.bubbleDatas[i].people);
            abData.bubbleDatas[i].emotions = AvoidDuplicatesInList(abData.bubbleDatas[i].emotions);
            bubblePrefabs[i].transform.Find("VertexExtrusion/Placard/Label").gameObject.name = abData.bubbleDatas[i].Moment;
            // should update the text mesh text 
            //   bubblePrefabs[i].transform.Find("VertexExtrusion/Placard/Label").gameObject.GetComponent<TextMesh>().text = abData.bubbleDatas[i].Moment;
            //  bubblePrefabs[i].transform.Find("VertexExtrusion/Placard/Label").gameObject.GetComponent<TextMesh>().text = abData.bubbleDatas[i].Moment;
            // bubblePrefabs[i].transform.Find("VertexExtrusion/Placard/Label").gameObject.GetComponent<TextMesh>().text = abData.bubbleDatas[i].Moment;
            // bubblePrefabs[i].transform.Find("Label Canvas/Label").GetComponent<TextMeshProUGUI>().text = abData.bubbleDatas[i].Moment;
        }



        //PopulateListForPeopleAndEmotions();

        /*       for (int i = 0; i < buttonPrefabs.Count; i++)
               {
                   BubbleData bubbleData = abData.bubbleDatas[i];
                 //  SaveButtonValueToJson(bubbleData, currentRadarPrefabs[i]);
               }*/
    }

    //once on clicked, save to people list in the json 
    public void OnClickedMsg()
    {

        Debug.Log("clicked");

    }


    // randomize the bubble positions 
    // only show category when the ball is clicked/ collided 


    // category PEOPLE showing - button - build the ui the same 
    // category EMOTIONS showing - button - build the ui the same 

    // avoid duplicates 

    // set up a button 
    public void ClearTheList()
    {

        // a button to clear the list to edit 


    }

    public List<string> AvoidDuplicatesInList(List<string> list)
    {
        return list.Distinct().ToList();

    }
    public void OnClicked(Button button)
    {

        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            string bubbleString = abData.bubbleDatas[i].Moment;
            string parentName = button.GetComponentInParent<GridLayoutGroup>().gameObject.name;
            button.name = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            List<string> thePeople = abData.bubbleDatas[i].people;
            List<string> theEmotions = abData.bubbleDatas[i].emotions;

            //find out the parent name or the button name 
            //Debug.Log($"button name is {button.name}, bubble string is {bubbleString}, parent name is {parentName}");

            if (parentName.Contains(bubbleString) && parentName.Contains("People"))
            {
                if (!thePeople.Contains(button.name))
                {
                    thePeople.Add(button.name);
                }
                thePeople = thePeople.Distinct().ToList();

                Debug.Log("people list is " + thePeople.Count);
            }

            if (parentName.Contains(bubbleString) && parentName.Contains("Emotions"))
            {
                if (!theEmotions.Contains(button.name))
                {
                    theEmotions.Add(button.name);

                }
                theEmotions = theEmotions.Distinct().ToList();

                Debug.Log("emotions list is " + theEmotions.Count);

            }
        }

        SaveToJson();
    }


    // create an editor that saves the json - low priority 
    // save to json from the inspector 
    void SaveToJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BubbleData.json");
        filePath = filePath.Replace(@"\", @"/");
        string json = JsonUtility.ToJson(abData);
        File.WriteAllText(filePath, json);
        Debug.Log($"inspectorBubbleData is {abData}, {abData.bubbleDatas.Count}");
        Debug.Log("save to json" + filePath + json);

    }

    public void SaveNameToList()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        GameObject currentParent = current.GetComponentInParent<GameObject>();
        string name = current.name;

        List<string> dataEmotionList = abData.bubbleDatas[0].emotions;
        List<string> dataPeopleList = abData.bubbleDatas[0].people;
        if (currentParent.tag == "Emotions")
        {
            if (!dataEmotionList.Contains(name))
            {
                dataEmotionList.Add(name);
            }
        }


        if (currentParent.tag == "People")
        {
            if (!dataPeopleList.Contains(name))
            {
                dataPeopleList.Add(name);
            }
        }



    }

    /// <summary>
    /// Create the bubbles and assign the bubble prefabs 
    /// can use the prefabs index to change the name of the gameobject 
    /// </summary>
    /// 
    public float radius = 20f;
    public float floorPosition = -2f;
    void CreateBubbles()
    {
        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            float angle = i * Mathf.PI*2f / abData.bubbleDatas.Count;
           
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, floorPosition, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(bubblePrefab, newPos, Quaternion.identity, this.transform);


          //  GameObject go = Instantiate(bubblePrefab, this.transform);
            go.name = abData.bubbleDatas[i].Moment;
            bubblePrefabs.Add(go);
            bubblePrefabs[i].transform.Find("CategorySelections/People").gameObject.name += " " + go.name;
            bubblePrefabs[i].transform.Find("CategorySelections/Emotions").gameObject.name += " " + go.name;
        }

    }

    // should map the age to the timeline and set the transform for the bubble individually     
    // assign that to the position of the bubble in the world 
    void AssignDataToBubble(BubbleData bubbleData, GameObject bubble)
    {


    }

    float MapToZeroToOne(float originalValue, float maxValue)
    {
        return originalValue / maxValue;
    }

    // read from json file 
    void ReadFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BubbleData.json");
        filePath = filePath.Replace(@"\", @"/");

        if (!File.Exists(filePath))
        {
            File.Create(filePath);

        }
        else
        {
            string json = File.ReadAllText(filePath);
            abData = JsonUtility.FromJson<AllBubbleData>(json);

            Debug.Log($"read from file {json}");

        }


    }

    // dummy key to drag the bubbble 
    // just focus on one bubble for now 

    // show the category selections 
    // show people 
    // once clicked finished people 
    // show emotions 
    // once clicked the finished emotions 
    // turn category off, show the bubble again 


    // if category is selected 
    void CategorySelectionProcess(Canvas categorySelection, GameObject peopleGroup, GameObject emotionGroup)
    {

        Button finishedPeople, finishedEmotion;

        peopleGroup.SetActive(true);
        emotionGroup.SetActive(true);

        if (!categorySelection.gameObject.activeSelf)
        {
            categorySelection.gameObject.SetActive(!categorySelection.gameObject.activeSelf);
        }






    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveToJson();
        }

        // dummy key to drag the bubbble 
        // just focus on one bubble for now 
        if (Input.GetKeyDown(KeyCode.B))
        {
            // show the category selections 
            // show people 
            // once clicked finished people 
            // show emotions 
            // once clicked the finished emotions 
            // turn category off, show the bubble again 


        }
    }
}
