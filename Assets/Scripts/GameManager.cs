using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    protected GameManager() { }

    private Player _player;
    public Player player {
        get {
            if(!_player) {
                _player = FindObjectOfType<Player>();
            }
            return _player;
        }
    }

    public Inventory inventory {
        get {
            return player.inventory;
        }
    }

}
