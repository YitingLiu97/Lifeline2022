using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
        [Range(0, 10)]
        public int CategoryTwo;
        [Range(0, 10)]
        public int CategoryThree;
        [Range(0, 10)]
        public int CategoryFour;
        [Range(0, 10)]
        public int CategoryFive;
        [Range(0, 10)]
        public int CategorySix;
    }
    [Serializable]
    public class AllBubbleData
    {
        public List<BubbleData> bubbleDatas = new List<BubbleData>();

    }
    public const int CATEGORY_SIZE = 6;
    public const int RADAR_MAX_VALUE = 10;
    public GameObject bubblePrefab;
    GameObject radarChartPrefab;
    RadarChart currentRadar;
    public List<GameObject> bubblePrefabs;
    public List<GameObject> radarChartPrefabs;
    public List<RadarChart> currentRadarPrefabs;
    public AllBubbleData abData = new AllBubbleData();



    // Start is called before the first frame update
    void Start()
    {


        ReadFromFile();
        CreateBubbles();

        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            BubbleData bubbleData = abData.bubbleDatas[i];
            AssignDataToRadar(bubbleData, currentRadarPrefabs[i]);
        }
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
            radarChartPrefab = bubblePrefab.gameObject.transform.Find("RadarChart").gameObject;
            currentRadar = radarChartPrefab.transform.Find("Current").gameObject.GetComponent<RadarChart>();
            bubblePrefabs.Add(go);
            radarChartPrefabs.Add(radarChartPrefab);
            currentRadarPrefabs.Add(currentRadar);
        }

    }
    // should map the age to the timeline and set the transform for the bubble individually     

    void AssignDataToBubble(BubbleData bubbleData, GameObject bubble)
    {



    }

    float MapToZeroToOne(float originalValue, float maxValue) {
        return originalValue / maxValue;
    }
    // chart is not really working - it is the same data 
    void AssignDataToRadar(BubbleData bubbleData, RadarChart radarChart)
    {
        Debug.Log($"bubble data is {bubbleData.Moment}, radarChart {radarChart.GetParameters()}");
        radarChart.SetParameter(0,MapToZeroToOne(bubbleData.CategoryTwo,RADAR_MAX_VALUE));
        radarChart.SetParameter(1, MapToZeroToOne(bubbleData.CategoryThree, RADAR_MAX_VALUE));
        radarChart.SetParameter(2, MapToZeroToOne(bubbleData.CategoryFour, RADAR_MAX_VALUE));
        radarChart.SetParameter(3, MapToZeroToOne(bubbleData.CategoryFive, RADAR_MAX_VALUE));
        radarChart.SetParameter(4, MapToZeroToOne(bubbleData.CategorySix, RADAR_MAX_VALUE));
    }
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




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveToJson();


        }
    }
}
