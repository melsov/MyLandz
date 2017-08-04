using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefs : MonoBehaviour {

    [MenuItem("MyLandz/Delete All Player Prefs")]
    static void Delete() {
        PlayerPrefs.DeleteAll();
    }
    
}
