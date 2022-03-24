using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    // read from json 
    // bubble class 

    [Serializable]
    public class BubbleData
    {
        public string Moment;
        [Range (-10,10)]
        public int impactValue;
        public int age;
        [Range(0, 10)]
        public int CategoryOne;
        [Range(0, 10)]
        public int CategoryTwo;
        [Range(0, 10)]
        public int CategorThree;
        [Range(0, 10)]
        public int CategoryFour;
        [Range(0, 10)]
        public int CategoryFive;
    }
    [Serializable]
    public class AllBubbleData {
        public List<BubbleData> bubbleDatas = new List<BubbleData>();
    
    }

   // public List<BubbleData> inspectorBubbleData = new List<BubbleData>();
    public AllBubbleData abData = new AllBubbleData();

    // Start is called before the first frame update
    void Start()
    {
       //ReadFromFile();
      SaveToJson();
    }

    // create an editor that saves the json - low priority 
   // save to json from the inspector 
    void SaveToJson() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BubbleData.json");
        
    string json = JsonUtility.ToJson(abData);
        File.WriteAllText(filePath, json);
        Debug.Log($"inspectorBubbleData is {abData}, {abData.bubbleDatas.Count}");
        Debug.Log("save to json" + filePath + json);
    
    }


    // read from json file 
    void ReadFromFile() {
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
        if (Input.GetKeyDown(KeyCode.Space)) {
            SaveToJson();
        
        
        }
    }
}
