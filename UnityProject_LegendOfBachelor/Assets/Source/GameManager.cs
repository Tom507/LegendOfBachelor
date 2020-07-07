using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        UIMachine.StateChanged(UIMachine.StateUi.Game);
    }

    public enum StateType
    {
        Default,
        OnMap,
        Fighting,
        InCity
    }
}
