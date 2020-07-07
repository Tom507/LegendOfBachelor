using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class LobUtilities
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// shows ALL Objects, disabled objects included. (only works in editor)
    /// </summary>
    //public static List<GameObject> getAllGameobjectsInScene()
    //{
    //    {
    //        List<GameObject> objectsInScene = new List<GameObject>();

    //        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
    //        {
    //            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
    //                objectsInScene.Add(go);
    //        }

    //        return objectsInScene;
    //    }
    //}

    public static IEnumerable<GameObject> GetAllRootGameObjects()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            GameObject[] rootObjs = SceneManager.GetSceneAt(i).GetRootGameObjects();
            foreach (GameObject obj in rootObjs)
                yield return obj;
        }
    }

    public static IEnumerable<T> FindAllMonoBehaviourOfTypeExpensive<T>()
        where T : MonoBehaviour
    {
        foreach (GameObject obj in GetAllRootGameObjects())
        {
            foreach (T child in obj.GetComponentsInChildren<T>(true))
                yield return child;
        }
    }

    public static IEnumerable<T> FindAllBehaviourOfTypeExpensive<T>()
    where T : Behaviour
    {
        foreach (GameObject obj in GetAllRootGameObjects())
        {
            foreach (T child in obj.GetComponentsInChildren<T>(true))
                yield return child;
        }
    }
}