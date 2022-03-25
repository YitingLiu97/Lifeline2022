using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssignData : MonoBehaviour
{
    // instantiate the prefab and assign the value 
    public GameObject chartPrefab;
    public GameObject peopleContentBox;
    // for people 



    // find out which point is clicked 
    void PopulateData()
    {
        peopleContentBox.GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
