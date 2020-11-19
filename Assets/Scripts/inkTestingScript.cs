using Ink.Runtime;
//To be able to access Ink's class in the script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//to be able to use UI's classes;
using TMPro;


public class inkTestingScript : MonoBehaviour
{

    public TextAsset inkJSON;
    private Story story;
    // Start is called before the first frame update

    public TextMeshProUGUI textPrefab;
    public Button buttonPrefab;
    void Start()
    {
        story = new Story(inkJSON.text);
        //we create a new story object using Ink's Story class;
        //we give it inkJSON's text as a parameter;

        var storyTextInstance = Instantiate(textPrefab);
        //we instantiate a clone of our prefab as text;
        var storyText = storyTextInstance.GetComponent<TextMeshProUGUI>();
        storyText.text = loadStoryChunk();
        //we're loading its text with the loadStoryChunk() function;
        storyText.transform.SetParent(this.transform, false);
        //we do not change anything relative to its position;

        Debug.Log(loadStoryChunk());
        //we're calling the loadStoryChunk function;


        foreach (Choice choice in story.currentChoices)
        //the for loop is going to move through the total number of currentChoices;
        {
            var choiceButton = Instantiate(buttonPrefab);
            var choiceText =  buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
            //We create the choiceText variable and give it the Button's text value;
            choiceText.text = choice.text;
            choiceButton.transform.SetParent(this.transform, false);

        }

        //we're now going to check how to make a choice;
        story.ChooseChoiceIndex(0);
        //choices starts at 0;

        Debug.Log(loadStoryChunk());
        //we're calling the loadStoryChunk function;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string loadStoryChunk()
    //the function will return some text so it's a string function and not void;
    {
        string text = "";
        //we create a string variable and leave it empty;

        if(story.canContinue)
        //we create an if statement that checks if the canContinue boolean is true;
        //canContinue is an Ink boolean, we can use it because we called "using Ink.Runtime;"
        {
            text = story.ContinueMaximally();
            //Continue loads the story each line at a time;
            //ContinueMaximally loads the story to the next choice
        }

        return text;
        
    }
}
