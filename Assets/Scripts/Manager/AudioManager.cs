using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;

public class AudioManager : PhoenixLikeSingleton<AudioManager> {

    private Dictionary<string, AudioSource> audios = new Dictionary<string, AudioSource>();

    private bool _mute;
    public bool mute {
        get { return _mute; }
        set {
            _mute = value;
            if (_mute) {
                stopAll();
            }
        }
    }

    public static string Dink = "Dink";
    public static string Zap = "Zap";
    public void Awake() {
        foreach(AudioSource au in GetComponentsInChildren<AudioSource>()) {
            audios.Add(au.gameObject.name, au);
        }
    }

    public void playDink() { play(Dink); }

    internal void playZap() { play(Zap); }

    /// <summary>
    /// play an audio clip whose path is <paramref name="resourceAudioRelativePath"/> 
    /// </summary>
    /// <param name="resourceAudioRelativePath"></param>
    public void play(string resourceAudioRelativePath) {
        AudioSource aud = getSource(resourceAudioRelativePath);
        play(aud);
    }

    public void play(AudioSource aus) {
        if(mute) { return; }
        if(aus) { aus.Play(); }
    }

    private void stopAll() {
        foreach(AudioSource aud in getAll()) {
            if(aud.isPlaying) {
                aud.Stop();
            }
        }
    }

    private IEnumerable<AudioSource> getAll() {
        foreach(string key in audios.Keys) {
            yield return audios[key];
        }
    }

    //lazy loading
    private AudioSource getSource(string resourcesAudioRelativePath) {
        if(audios.ContainsKey(resourcesAudioRelativePath)) { return audios[resourcesAudioRelativePath]; }
        AudioClip clip = Resources.Load<AudioClip>(string.Format("Audio/{0}", resourcesAudioRelativePath));
        Assert.IsTrue(clip, "null audio clip? " + resourcesAudioRelativePath);
        GameObject go = new GameObject(resourcesAudioRelativePath);
        AudioSource aud = go.AddComponent<AudioSource>();
        aud.clip = clip;
        audios.Add(go.name, aud);
        return aud;
    }


}
