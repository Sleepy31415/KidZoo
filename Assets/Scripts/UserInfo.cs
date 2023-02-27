using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Address { get; set; }

    [FirestoreProperty]
    public int PhoneNumber { get; set; }

    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public int YearOfBirth { get; set; }

    [FirestoreProperty]
    public string DateOfBirth { get; set; }
}
