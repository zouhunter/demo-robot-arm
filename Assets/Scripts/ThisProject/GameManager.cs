#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-09 03:58:04
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PureMVC;

/// <summary>
/// MonoBehaiver
/// <summary>
public class GameManager : GameManager<GameManager> {

    public const string pickupSequenceProxy = "pickupSequence";
    public const string pickdownSequenceProxy = "pickdownSequence";
    public const string saveObserverName = "saveSequenceDatas";
    public const string SettngData = "SettngData";
    public const string SettingDataLoadSave = "SettingDataLoadSave";

    protected override void LunchFrameWork()
    {
        new DataLoadCommand().Execute();
        new SettingDataLoadSave().Execute(true);
        Facade.RegisterCommand<DataSaveCommand>(saveObserverName);
        Facade.RegisterCommand<SettingDataLoadSave,bool>(SettingDataLoadSave);
    }
}
