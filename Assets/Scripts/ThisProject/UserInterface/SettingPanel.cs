#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 08:36:44
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BridgeUI;
using PureMVC;

/// <summary>
/// MonoBehaiver
/// <summary>
public class SettingPanel : SingleCloseAblePanel {
    [SerializeField] private Slider ligthStrigthSld;      //4.灯光亮度调节
    [SerializeField] private Button resetBtn;
    private static ExpSetting setting { get; set; }

    public override void Close()
    {
        SaveSettingData();
        base.Close();
    }

    protected override void Start()
    {
        base.Start();
        if (setting == null)
        {
            Facade.RetrieveData<ExpSetting>(GameManager.SettngData, (x) => { OnLoadSettingData(x); });
        }
        else
        {
            OnLoadSettingData(setting);
        }
     
        resetBtn.onClick.AddListener(ResetDefult);
        ligthStrigthSld.onValueChanged.AddListener(OnLigthStrigthSldValueChanged);
    }

    private void OnLigthStrigthSldValueChanged(float arg)
    {
        if (setting != null) setting.LightStrength = arg;
    }

    private void ResetDefult()
    {
        setting.ResetDefult();
        OnLoadSettingData(setting);
    }
    void OnLoadSettingData(ExpSetting proxy)
    {
        setting = proxy;
        ligthStrigthSld.value = setting.lightStrength;
    }

    void SaveSettingData()
    {
        setting.LightStrength = ligthStrigthSld.value;
        Facade.SendNotification(GameManager.SettingDataLoadSave, false);
    }
}
