#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-09 04:05:22
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
public class DataSaveCommand : Command
{
    private const string pickupState = "pickup.json";
    private const string pickdownState = "pickdown.json";
    private IProxy<ArmSquenceList> pickupSequenceProxy;
    private IProxy<ArmSquenceList> pickdownSequenceProxy;

    private ArmSquenceList pickupSequence { get { return pickupSequenceProxy.Data; } }
    private ArmSquenceList pickdownSequence { get { return pickdownSequenceProxy.Data; } }
    public override void Execute()
    {
        Facade.RetrieveProxy<ArmSquenceList>(GameManager.pickupSequenceProxy, (x) => { pickupSequenceProxy = x; });
        Facade.RetrieveProxy<ArmSquenceList>(GameManager.pickdownSequenceProxy, (x) => { pickdownSequenceProxy = x; });

        SaveToJson(pickupState, pickupSequence);
        SaveToJson(pickdownState, pickdownSequence);
    }
    private void SaveToJson(string fileName, ArmSquenceList list)
    {
        var jsonPath = Application.persistentDataPath + "/" + fileName;
        var str = JsonUtility.ToJson(list);
        System.IO.File.WriteAllText(jsonPath, str);
    }

}
