#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-09 03:56:05
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
/// 信息读取
/// <summary>
public class DataLoadCommand :  Command {

    private const string pickupState = "pickup.json";
    private const string pickdownState = "pickdown.json";

    public override void Execute()
    {
        var pickupSequence = GetSequenceFromPath(pickupState);
        var pickdownSequence = GetSequenceFromPath(pickdownState);

        Facade.RegisterProxy(new Proxy<ArmSquenceList>(GameManager.pickupSequenceProxy, pickupSequence));
        Facade.RegisterProxy(new Proxy<ArmSquenceList>(GameManager.pickdownSequenceProxy, pickdownSequence));
    }

    private ArmSquenceList GetSequenceFromPath(string fileName)
    {
        var jsonPath = Application.persistentDataPath + "/" + fileName;
        ArmSquenceList list = null;
        if (System.IO.File.Exists(jsonPath))
        {
            var str = System.IO.File.ReadAllText(jsonPath);
            list = JsonUtility.FromJson<ArmSquenceList>(str);
        }

        if (list == null)
        {
            list = new ArmSquenceList();
        }
        return list;

    }

}
