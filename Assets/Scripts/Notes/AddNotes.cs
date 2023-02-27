using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AddNotes : MonoBehaviour
{

    public Text NotesInput;

    public Button SaveNotes;
/*
    // Start is called before the first frame update
    void Start()
    {
        SaveNotes.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }*/

    private void TaskOnClick()
    {
        //personalNotes = new PersonalNotes(NotesInput.text);
        PersonalNotes.instance.Notes = NotesInput.text;
        PersonalNotes.instance.AddToCache();

        Debug.Log(Caching.instance.DownloadAndGetCacheNotes());
    }
}
