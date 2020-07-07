using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    public List<Mesh> TextMeshes = new List<Mesh>();
    void Start()
    {
        gameObject.GetComponent<MeshFilter>().mesh = TextMeshes[Random.Range(0, TextMeshes.Count)];
    }
}
