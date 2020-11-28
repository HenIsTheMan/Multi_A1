//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using UnityEngine;
using UnityEngine.Assertions;

public abstract class Singleton<T>: ScriptableObject where T: ScriptableObject {
    #region Fields

    private static T globalInstance;

    #endregion

    #region Properties

    public static T GlobalInstance {
        get {
            if(globalInstance == null) {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                Assert.AreEqual(results.Length, 1);
                globalInstance = results[0];
            }
            return globalInstance;
        }
    }

    #endregion

    #region Ctors and Dtor
    
    static Singleton() {
        globalInstance = null;
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion
}