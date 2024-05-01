using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramesTracker : MonoBehaviour
{
    private Text text;
    private float deltaTime = 0.0f;
    private void Start() {
        
        text = GetComponent<Text>();
    }
    void Update()
    {
        // update text by number of frames per second
         deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
         text.text = Mathf.RoundToInt(1.0f / deltaTime).ToString();
    }
}
