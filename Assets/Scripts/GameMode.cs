using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static GameMode instance;

    public bool new_game = false;

    private void Start()
    {
        instance = this;
        new_game = false;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
