using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeAndUpdater : MonoBehaviour
{
    [SerializeField]
    private string _code;
    public string code { get { return _code; } }

    [SerializeField, Header("If none, searches this game object")]
    private MLUpdaterSet _updaterSet;
    public MLUpdaterSet updaterSet { get { return _updaterSet; } }

    private void Awake() {
        _updaterSet = GetComponent<MLUpdaterSet>();
    }
}