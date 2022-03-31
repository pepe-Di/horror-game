using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class DropDown : MonoBehaviour
{
    [Serializable]
    public class DropdownEvent : UnityEvent<int> {}
    [SerializeField] private DropdownEvent m_OnValueChanged = new DropdownEvent();
    public DropdownEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }
    void Set(int value, bool sendCallback = true)
        {this.value=value;
            if (Application.isPlaying && (value == Value || options.Count == 0))
                return;

           // value = Mathf.Clamp(value, 0, options.Count - 1);
            //RefreshShownValue();

            if (sendCallback)
            {
                Debug.Log("set() "+value);
                // Notify all listeners
                //UISystemProfilerApi.AddMarker("Dropdown.value", this);
                m_OnValueChanged.Invoke(value);
            }
        }
       public void SetValueWithoutNotify(int input)
        {
           // Set(input, false);
        }
    public List<string> options;
    public int value;
    public Text txtValue;

    public int Value { get => value; set => this.value=value; }

    void Awake()
    {
        txtValue = GetComponentInChildren<Text>();
        
    }
    void Start(){

     // onValueChanged.AddListener(delegate {m_OnValueChanged.Invoke(value);});
    }
    public void ClearOptions()
    {
        options.Clear();
    }
    public void AddOptions(List<string> opt)
    {
        options = new List<string>();
        foreach(string o in opt)
        {
            options.Add(o);
        }
    }
   public void RefreshShownValue()
   {
       if(value<options.Count&&options!=null){txtValue.text = options[value];
       m_OnValueChanged.Invoke(value);
       }
      // Set(value);
   }
   public void onClick()
   {
        NextOption();
   }
   public void NextOption()
   {
       value++;
       if(value>=options.Count) Value = 0;
       RefreshShownValue();
   }
}
