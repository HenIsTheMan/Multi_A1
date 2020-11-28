//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using UnityEngine;

[CreateAssetMenu(menuName = "Misc/GameSettings")]
public class GameSettings: ScriptableObject {
    #region Fields

    [SerializeField] private string gameVer;
    [SerializeField] private string nickname;

    #endregion

    #region Properties

    public string GameVer {
        get {
            return gameVer;
        }
    }

    public string Nickname {
        get {
            return nickname + Random.Range(0, 99999);
        }
    }

    #endregion

    #region Ctors and Dtor

    public GameSettings() {
        gameVer = "4.0";
        nickname = "Pun";
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion
}