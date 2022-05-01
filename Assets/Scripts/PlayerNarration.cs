using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNarration : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject go_content;
    [SerializeField] private TextMeshProUGUI t_textToRead;
    [SerializeField] private TextMeshProUGUI t_readAloudHeader;
    [SerializeField] private TextMeshProUGUI t_pullTriggersToStart;
    // Properties
    private bool isPlayingText = false;
    private float timeWhenNextWord;
    //private int currMessageIndex;
    private string currentMessage; // this gets truncated!
    private string currentMessageUncut; // this doesn't get truncated.
    // TESTING
    private string message_setup0 = "Pull both triggers to start.";
    private string message_setup1 = "Good. Listen very carefully to what I am about to say. I am being fed information from the great beyond.";
    //private List<string> messages = new List<string>()
    //{
    //    "Glad you can make it.",
    //    "This is the final sentence.",
    //};



    // ----------------------------------------------------------------
    // Start
    // ----------------------------------------------------------------
    void Start() {
        //t_textToRead.enabled = false;
        //t_readAloudHeader.enabled = false;
    }

    public void OnSetGameState_Setup0() {
        SetCurrMessage(message_setup0);
    }
    public void OnSetGameState_Setup1() {
        SetCurrMessage(message_setup1);
    }
    public void OnSetGameState_Playing() {
        //t_textToRead.enabled = true;
        //t_readAloudHeader.enabled = true;
        go_content.SetActive(false);
        isPlayingText = false;
    }


    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (!isPlayingText) return;
        if (Time.time >= timeWhenNextWord) {
            ShowNextWord();
        }
    }


    // ----------------------------------------------------------------
    // Doers
    // ----------------------------------------------------------------
    private void ShowNextWord() {
        if (currentMessage.Length == 0) {
            // Repeat the message.
            currentMessage = currentMessageUncut;
            t_textToRead.text = "";
            timeWhenNextWord = Time.time + 0.8f;
            return;
        }
        int wordIndex = currentMessage.IndexOf(' ');
        string word = wordIndex>=0 ? currentMessage.Substring(0, wordIndex) : currentMessage; // either take the substring, or the whole thing if there's no space at the end.
        t_textToRead.text = word.ToUpper();

        timeWhenNextWord = Time.time;
        timeWhenNextWord += 0.35f + word.Length*0.08f; // word length.
        if (word.Contains(","))
            timeWhenNextWord += 1.1f;
        if (word.Contains(".") || word.Contains("!") || word.Contains("?"))
            timeWhenNextWord += 2;


        if (wordIndex!=-1 && wordIndex <= currentMessage.Length) {
            currentMessage = currentMessage.Substring(wordIndex+1).ToUpper();
        }
        else {
            currentMessage = "";
        }
        //Debug.Log(Time.time + "   " + word);
    }


    private void SetCurrMessage(string _message) {
        isPlayingText = true;
        currentMessage = currentMessageUncut = _message;
        ShowNextWord();
    }



}
