using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Microsoft.MixedReality.Toolkit.UI;

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
    public GameObject momentVisuals;
    public const int CATEGORY_SIZE = 6;
    public const int RADAR_MAX_VALUE = 10;
    public GameObject bubblePrefab;
    public GameObject buttonPrefab;
    public List<GameObject> buttonPrefabs;
    public List<GameObject> bubblePrefabs;

    public AllBubbleData abData = new AllBubbleData();


    // Start is called before the first frame update
    void Start()
    {
       // SaveToJson();
        ReadFromFile();
        CreateBubbles();
  
        for (int i = 0; i < buttonPrefabs.Count; i++)
        {
            PressableButton btn = buttonPrefabs[i].GetComponent<PressableButton>();
            btn.ButtonPressed.AddListener(() => SaveButtonToList(btn));
        }


        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            abData.bubbleDatas[i].people = AvoidDuplicatesInList(abData.bubbleDatas[i].people);
            abData.bubbleDatas[i].emotions = AvoidDuplicatesInList(abData.bubbleDatas[i].emotions);
            bubblePrefabs[i].GetComponentInChildren<TextMesh>().text = abData.bubbleDatas[i].Moment;
       }




    }

    //once on clicked, save to people list in the json 
    public void OnClickedMsg()
    {

        Debug.Log("clicked");

    }
    public List<string> AvoidDuplicatesInList(List<string> list)
    {
        return list.Distinct().ToList();

    }

    public void SaveButtonToList(PressableButton button)
    {

        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            string bubbleString = abData.bubbleDatas[i].Moment;
            string parentName = button.gameObject.transform.parent.name;
            TextMeshProUGUI buttonText = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            button.name = buttonText.text;
            button.gameObject.name = parentName + " " + buttonText.text;
            List<string> thePeople = abData.bubbleDatas[i].people;
            List<string> theEmotions = abData.bubbleDatas[i].emotions;

            if (parentName.Contains(bubbleString) || parentName.Contains("People"))
            {
                if (!thePeople.Contains(button.name))
                {
                    thePeople.Add(button.name);
                }
                thePeople = thePeople.Distinct().ToList();

                Debug.Log("people list is " + thePeople.Count);
            }

            if (parentName.Contains(bubbleString) || parentName.Contains("Emotions"))
            {
                if (!theEmotions.Contains(button.name))
                {
                    theEmotions.Add(button.name);

                }
                theEmotions = theEmotions.Distinct().ToList();

                Debug.Log("emotions list is " + theEmotions.Count);

            }
        }


        // show button list in bubble - figure out to show the text 

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
    public float momentMoveThreshold = .04f;
    void CreateBubbles()
    {
        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / abData.bubbleDatas.Count;

            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, floorPosition, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(bubblePrefab, newPos, Quaternion.identity, momentVisuals.transform);


            //  GameObject go = Instantiate(bubblePrefab, this.transform);
            go.name = abData.bubbleDatas[i].Moment;
            bubblePrefabs.Add(go);
            bubblePrefabs[i].transform.Find("CategorySelections/People").gameObject.name += " " + go.name;
            bubblePrefabs[i].transform.Find("CategorySelections/Emotions").gameObject.name += " " + go.name;

            Moment moment = go.GetComponent<Moment>();

            moment.SetMoving(false);
            moment.SetSpeed(bubbleTransitionSpeed);
            moment.moveThreshold = momentMoveThreshold;
        }


    }

    public float bubbleTransitionSpeed = 1f;
    // should map the age to the timeline and set the transform for the bubble individually     
    // assign that to the position of the bubble in the world 
    public void SendBubblesToGraphPosition()
    {
        Debug.Log("Sending to graph");

        //Start highest age at 0
        int highestAge = 0;                

        //loop through all bubble data
        foreach (BubbleData oneBubble in abData.bubbleDatas)
        {
            //if age is higher than highest age, set highest age to new value
            if (oneBubble.age > highestAge)
            {
                highestAge = oneBubble.age;
            }

        }


        //get the length of the line, using workaround of start and end position objects
        float xAxisLength = GraphManager.Instance.graphEnd.position.x - GraphManager.Instance.graphStart.position.x;

        //calc distance between years on the graph
        float xIncrement = xAxisLength / highestAge;

        //get the length of y axis based on capsule collider
        float yAxisLength = GraphManager.Instance.graphTop.position.y - GraphManager.Instance.graphBottom.position.y;

        //calc distance between intensity amounts
        float yIncrement = yAxisLength / GraphManager.Instance.intensityScale;


        //loop through all bubble data
        for (int i = 0; i < abData.bubbleDatas.Count; i++)
        {
            Moment currentMoment = bubblePrefabs[i].gameObject.GetComponent<Moment>();

            // Positions are calculated by age/impact multiplied by increments
            float xPosition = (abData.bubbleDatas[i].age-5) * xIncrement;
            float yPosition = abData.bubbleDatas[i].impactValue * yIncrement;

            //create position, z is 0
            Vector3 newPosition = new Vector3(xPosition, yPosition, 0);

            newPosition += GraphManager.Instance.yAxis.transform.position;

            //set bubble position
            currentMoment.SetNextPosition(newPosition);
            currentMoment.SetMoving(true);

        }
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




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveToJson();
        }

     
    }
}
