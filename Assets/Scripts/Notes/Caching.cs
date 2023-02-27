using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Caching : MonoBehaviour
{
    // Instance
    public static Caching instance;

    /// <summary>
    /// Text to add to PlayerPrefs codes
    /// </summary>
    public string Notes;

    private void Awake()
    {
        if (instance == null)
            instance = this;
       /* else
            Destroy(gameObject);*/
    }

    /// <summary>
    /// If notes arent null or empty, caches notes to PlayerPrefs with key "notes"
    /// </summary>
    public void DownloadAndSetCacheNotes()
    {
        if (!String.IsNullOrEmpty(Notes))
        {
            PlayerPrefs.SetString("notes", Notes);
        }
    }

    public string DownloadAndGetCacheNotes()
    {
        return PlayerPrefs.GetString("notes");
    }

}
