using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaNumericInput : MonoBehaviour {

    class CharacterQueue
    {
        private Queue<char> chars = new Queue<char>();
        public string str {
            get {
                string result = "";
                foreach(char c in chars) {
                    result = string.Format("{0}{1}", result, c);
                }
                return result;
            }
        }

        public void Add(string s) {
            foreach(char c in s) {
                chars.Enqueue(c);
            }
        }

        public string appendNext(string s) {
            if(chars.Count == 0) {
                return s;
            }
            return string.Format("{0}{1}", s, chars.Dequeue());
        }

        public void Add(char c) { chars.Enqueue(c); }

        public static implicit operator string(CharacterQueue cq) {
            return cq.str;
        }

        internal void Clear() {
            chars.Clear();
        }
    }

    private Button[] buttons;
    [SerializeField]
    private Button closeButton;

    private CharacterQueue input = new CharacterQueue();
    [SerializeField]
    private Text display;
    public Action<string, Action<bool, MLUpdaterSet>> handleInput;
    private AudioSource aus;
    [SerializeField]
    private AudioSource validCodeFeedback;
    [SerializeField]
    private AudioSource invalidCodeFeedback;

    private bool processingInput;
    [SerializeField]
    private string emptyDisplayStr = "---";

    private void Awake() {
        buttons = GetComponentsInChildren<Button>();
        foreach(Button b in buttons) {
            if (b == closeButton) {
                b.onClick.AddListener(() => { hide(); });
            } else {
                b.onClick.AddListener(() => { pressed(b.GetComponentInChildren<Text>().text); });
            }
        }
        aus = GetComponent<AudioSource>();
    }

    private void hide() {
        input.Clear();
        gameObject.SetActive(false);
    }

    private void Start() {
        display.text = emptyDisplayStr;
    }

    private void pressed(string s) {
        input.Add(s);// string.Format("{0}{1}", input, s);
        if(aus) {
            aus.Play();
        }
        StartCoroutine(processInput());
    }

    private IEnumerator processInput() {
        while(processingInput) {
            yield return new WaitForSeconds(.25f);
        }
        display.text = input.appendNext(getDisplayString());
        if(handleInput != null) { 
            handleInput.Invoke(display.text, (bool clear, MLUpdaterSet upSet) => {
                StartCoroutine(invokeAndClear(clear, upSet, ()=> {
                    processingInput = false;
                }
                ));
            });
        }
    }

    private IEnumerator invokeAndClear(bool clear, MLUpdaterSet upSet, Action callback) {
        if (clear) {
            processingInput = true;
            if(!upSet) {
                invalidCodeFeedback.Play();
            } else {
                validCodeFeedback.Play();
            }
            yield return new WaitForSeconds(.4f);
            display.text = emptyDisplayStr;
        }
        if (upSet) {
            upSet.Invoke();
        }
        callback.Invoke();
    }

    private string getDisplayString() {
        if(display.text.Equals(emptyDisplayStr)) { return ""; }
        return display.text;
    }

}
