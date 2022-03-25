using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bubble : MonoBehaviour
{

    // read from json 
    // bubble class 

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
    public List<GameObject> radarChartPrefabs;
    public List<RadarChart> currentRadarPrefabs;
    public AllBubbleData abData = new AllBubbleData();


    // Start is called before the first frame update
    void Start()
    {
        ReadFromFile();
        CreateBubbles();
        PopulateListForPeopleAndEmotions();

    }

    // create an editor that saves the json - low priority 
    // save to json from the inspector 
    void SaveToJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BubbleData.json");

        string json = JsonUtility.ToJson(abData);
        File.WriteAllText(filePath, json);
        Debug.Log($"inspectorBubbleData is {abData}, {abData.bubbleDatas.Count}");
        Debug.Log("save to json" + filePath + json);

    }

    void PopulateListForPeopleAndEmotions() {
        GameObject people = GameObject.FindGameObjectWithTag("People");
        GameObject emotion = GameObject.FindGameObjectWithTag("Emotions");

        for (int i = 0; i < peopleList.Count; i++)
        {
            buttonPrefab.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = peopleList[i];
            buttonPrefab.gameObject.name = peopleList[i];
            Instantiate(buttonPrefab, people.transform);
            buttonPrefabs.Add(buttonPrefab);

        }

        for (int i = 0; i < emotionList.Count; i++)
        {
            buttonPrefab.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = emotionList[i];
            buttonPrefab.gameObject.name = emotionList[i];
            Instantiate(buttonPrefab, emotion.transform);
            buttonPrefabs.Add(buttonPrefab);
        }



    }
 /*   /// <summary>
    /// button on click to add data to specific string 
    /// </summary>
    /// <param name="bubbleData"></param>
    /// <param name="strDataList"></param>
    /// <param name="strData"></param>
    public void AddContentToJson(BubbleData bubbleData, string strData) {

        bubbleData = abData.bubbleDatas[0];

        // setactive the canvas when the bubble data is picked 

        if (GetComponentInParent<GameObject>().tag == "Emotions")
        {
            if (bubbleData.emotions.Contains(strData))
            {
                bubbleData.emotions.Add(strData);
            }
        }
        if (GetComponentInParent<GameObject>().tag == "People")
        {
            if (bubbleData.people.Contains(strData))
            {
                bubbleData.people.Add(strData);
            }
        }

    }*/


    public void SaveNameToList() {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        GameObject currentParent = current.GetComponentInParent<GameObject>();
        string name = current.name;


        List<string> dataEmotionList = abData.bubbleDatas[0].emotions;
        List<string> dataPeopleList = abData.bubbleDatas[0].people;
        if (currentParent.tag == "Emotions") {
            if (!dataEmotionList.Contains(name))
            {

                dataEmotionList.Add(name);
            }
        }


        if (currentParent.tag == "People")
        {
            if (!dataPeopleList.Contains(name)) {
                dataPeopleList.Add(name);
            }
        }



    }

    /// <summary>
    /// Create the bubbles and assign the bubble prefabs 
    /// can use the prefabs index to change the name of the gameobject 
    /// </summary>
    void CreateBubbles()
    {
        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            GameObject go = Instantiate(bubblePrefab, this.transform);
            go.name = abData.bubbleDatas[i].Moment;
            bubblePrefabs.Add(go);
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
    /*    void AssignDataToRadar(BubbleData bubbleData, RadarChart radarChart)
        {
            Debug.Log($"bubble data is {bubbleData.Moment}, radarChart {radarChart.GetParameters()}");
            radarChart.SetParameter(0,MapToZeroToOne(bubbleData.CategoryTwo,RADAR_MAX_VALUE));
            radarChart.SetParameter(1, MapToZeroToOne(bubbleData.CategoryThree, RADAR_MAX_VALUE));
            radarChart.SetParameter(2, MapToZeroToOne(bubbleData.CategoryFour, RADAR_MAX_VALUE));
            radarChart.SetParameter(3, MapToZeroToOne(bubbleData.CategoryFive, RADAR_MAX_VALUE));
            radarChart.SetParameter(4, MapToZeroToOne(bubbleData.CategorySix, RADAR_MAX_VALUE));
        }
    */   
    
    // read from json file 
    void ReadFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BubbleData.json");

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveToJson();
        }

    }
}
