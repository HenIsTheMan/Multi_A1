//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using UnityEngine;

[CreateAssetMenu(menuName = "Misc/MasterManager")]
public class MasterManager: Singleton<MasterManager> {
    #region Fields

    [SerializeField] private GameSettings myGameSettings;

    #endregion

    #region Properties

    public static GameSettings MyGameSettings {
        get {
            return GlobalInstance.myGameSettings;
        }
    }

    #endregion

    #region Ctors and Dtor

    public MasterManager() {
        myGameSettings = null;
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion
}