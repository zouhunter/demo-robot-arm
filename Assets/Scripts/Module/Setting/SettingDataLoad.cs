using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.IO;
using PureMVC;

public class SettingDataLoadSave : Command<bool>
{
    private const string playPer = "SettingData";
    static ExpSetting expSetting;

    static SettingDataLoadSave(){
        InitExpSetting();
    }

    private static void InitExpSetting()
    {
        if (PlayerPrefs.HasKey(playPer))
        {
            var value = PlayerPrefs.GetString(playPer);
            if (!string.IsNullOrEmpty(value))
            {
                expSetting = JsonUtility.FromJson<ExpSetting>(value);
            }
        }
        if (expSetting == null)
        {
            expSetting = new global::ExpSetting();
            expSetting.ResetDefult();
        }
    }

    public override void Execute(bool isLoad)
    {
        //加载
        if (isLoad)
        {
            if (Facade.HaveProxy(GameManager.SettngData))
            {
                Facade.RetrieveProxy<ExpSetting>(GameManager.SettngData, (proxy)=> {
                    proxy.Data = expSetting;
                });

            }
            else
            {
                Facade.RegisterProxy(new Proxy<ExpSetting>(GameManager.SettngData, expSetting));
            }
        }
        //保存
        else
        {
            PlayerPrefs.SetString(playPer, JsonUtility.ToJson(expSetting));
        }

    }
}
