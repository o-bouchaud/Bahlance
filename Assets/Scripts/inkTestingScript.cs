using Ink.Runtime;
//To be able to access Ink's class in the script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inkTestingScript : MonoBehaviour
{

    public TextAsset inkJSON;
    private Story story;
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
        //we create a new story object using Ink's Story class;
        //we give it inkJSON's text as a parameter;

        Debug.Log(story.Continue());
        //Continue loads the story each line at a time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
