#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-09 03:56:42
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
public class Main : SceneMain<Main> {
    protected override void Awake()
    {
        base.Awake();
        GameManager.StartGame();
    }
}