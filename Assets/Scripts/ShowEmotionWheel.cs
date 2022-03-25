using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEmotionWheel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // insert bool from hand tracking 
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
