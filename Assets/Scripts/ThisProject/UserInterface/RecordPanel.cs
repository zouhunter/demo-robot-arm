#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 08:45:12
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BridgeUI;

/// <summary>
/// MonoBehaiver
/// <summary>
public class RecordPanel : SingleCloseAblePanel {
    [SerializeField]
    private Toggle m_pickup;
    [SerializeField]
    private Button m_save;
    [SerializeField]
    private List<Slider> sliders;
    private const string pickupState = "pickup.json";
    private const string pickdownState = "pickdown.json";

    private ArmSquenceList pickupSequence;
    private ArmSquenceList pickdownSequence;
    private Robot robot { get { return Robot.Instence; } }

    protected override void Awake()
    {
        base.Awake();
        InitEvent();
        InitData();
        RegistSliderEvents();
        LoadSliderStates(m_pickup.isOn);
    }
    private void InitEvent()
    {
        m_save.onClick.AddListener(SaveToJson);
        m_pickup.onValueChanged.AddListener(LoadSliderStates);
    }

    private void SaveToJson()
    {
        if (m_pickup.isOn)
        {
            pickupSequence.armList.Clear();
            pickupSequence.armList.Add(new ArmSquence(GetSliderValues()));
            SaveToJson(pickupState, pickupSequence);
        }
        else
        {
            pickdownSequence.armList.Clear();
            pickdownSequence.armList.Add(new ArmSquence(GetSliderValues()));
            SaveToJson(pickdownState, pickdownSequence);
        }
    }

    private void InitData()
    {
        pickupSequence = GetSequenceFromPath(pickupState);
        pickdownSequence = GetSequenceFromPath(pickdownState);
    }

    private void LoadSliderStates(bool pickup)
    {
        ArmSquence sequestion = null;

        if (pickup && pickupSequence.armList.Count > 0)
        {
            sequestion = pickupSequence.armList[0];
        }
        else if(pickdownSequence.armList.Count > 0)
        {
            sequestion = pickdownSequence.armList[0];
        }

        if(sequestion != null)
        {
            for (int i = 0; i < sequestion.values.Count; i++)
            {
                if (sliders.Count > i)
                {
                    sliders[i].value = sequestion.values[i];
                }
            }
        }
       
    }
    private ArmSquenceList GetSequenceFromPath(string fileName)
    {
        var jsonPath = Application.persistentDataPath + "/" + fileName;
        ArmSquenceList list = null;
        if(System.IO.File.Exists(jsonPath))
        {
            var str = System.IO.File.ReadAllText(jsonPath);
            list = JsonUtility.FromJson<ArmSquenceList>(str);
        }

        if(list == null)
        {
            list = new ArmSquenceList();
        }
        return list;

    }

    private void SaveToJson(string fileName, ArmSquenceList list)
    {
        var jsonPath = Application.persistentDataPath + "/" + fileName;
        var str = JsonUtility.ToJson(list);
        System.IO.File.WriteAllText(jsonPath, str);
    }

    private float[] GetSliderValues()
    {
        var values = new float[sliders.Count];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = sliders[i].value;
        }
        return values;
    }
    private void RegistSliderEvents()

    {
        for (int i = 0; i < sliders.Count; i++)
        {
            var id = i;
            sliders[id].onValueChanged.AddListener((value) => { OnSliderValueChanged(id, value); });
        }
    }

    private void OnSliderValueChanged(int index,float value)
    {
        if(robot)
        {
            robot.SetArmValue(index, value);
        }
    }
}
