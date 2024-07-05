using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefubData : MonoBehaviour
{
    static public LoadPrefubData instance;

    public GameObject _brokenGrass;
    public GameObject _sound;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
