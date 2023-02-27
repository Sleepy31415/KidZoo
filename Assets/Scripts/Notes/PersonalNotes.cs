using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalNotes : MonoBehaviour
{

    public static PersonalNotes instance;

    /*
    /// <summary>
    /// Notes field
    /// </summary>
    private string notes;
    */

    public string Notes;

    /*
    /// <summary>
    /// Constructor of this class
    /// </summary>
    /// <param name="notes"></param>
    public PersonalNotes(string notes)
    {
        this.notes = notes;
        Notes = this.notes;
    }

    public PersonalNotes()
    {
        notes = Notes;
    }
    */


    /*
    /// <summary>
    /// Notes variable
    /// </summary>
    public string Notes {
        get
        {
            if (String.IsNullOrEmpty(notes))
            {
                if (Caching.instance != null)
                    notes = Caching.instance.Notes;
                else
                    notes = "";
            }
                
            return notes;
        }

        set
        {
            notes = value;
        }
    }
    */

    public void AddToCache()
    {
        Caching.instance.Notes = Notes;
        Caching.instance.DownloadAndSetCacheNotes();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
