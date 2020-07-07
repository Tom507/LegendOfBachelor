using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class LobToggle : MonoBehaviour
{
    public Toggle m_Toggle;
    
    void Awake()
    {
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });
    }

    private void OnEnable()
    {
        m_Toggle.isOn = false;
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        if (m_Toggle.isOn)
        {
            LOBEvents.current.QuizToggle(gameObject);
        }
    }
}
