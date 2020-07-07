using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    public GameObject keyPrefab;
    private void OnDestroy()
    {
        GameObject key = Instantiate(keyPrefab);
        key.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
    }
}
