using System;
using System.Collections.Generic;

public class Watchable<T>
{
    private T _storage;

    public Watchable(T t) {
        this._storage = t;
    }

    private List<Action<T>> _watchers;
    private List<Action<T>> watchers {
        get {
            if (_watchers == null) {
                _watchers = new List<Action<T>>();
            }
            return _watchers;
        }
    }

    public void addListender(Action<T> a) { watchers.Add(a); }

    public T val {
        get {
            return _storage;
        }
        set {
            _storage = value;
            foreach(Action<T> a in watchers) {
                a.Invoke(_storage);
            }
        }
    }
}