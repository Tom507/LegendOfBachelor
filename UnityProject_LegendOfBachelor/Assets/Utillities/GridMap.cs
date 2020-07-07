using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour, ISerializationCallbackReceiver
{
    public int[,] mapLayout = new int[3,4];

    [HideInInspector]
    [SerializeField]
    private int[] m_FlattendMapLayout;

    [HideInInspector]
    [SerializeField]
    private int m_FlattendMapLayoutRows;

    public void OnBeforeSerialize()
    {
        int c1 = mapLayout.GetLength(0);
        int c2 = mapLayout.GetLength(1);
        int count = c1 * c2;
        m_FlattendMapLayout = new int[count];
        m_FlattendMapLayoutRows = c1;
        for (int i = 0; i < count; i++)
        {
            m_FlattendMapLayout[i] = mapLayout[i % c1, i / c1];
        }
    }
    public void OnAfterDeserialize()
    {
        int count = m_FlattendMapLayout.Length;
        int c1 = m_FlattendMapLayoutRows;
        int c2 = count / c1;
        mapLayout = new int[c1, c2];
        for (int i = 0; i < count; i++)
        {
            mapLayout[i % c1, i / c1] = m_FlattendMapLayout[i];
        }
    }
}
