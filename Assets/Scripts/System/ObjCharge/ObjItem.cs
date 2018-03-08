#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 09:17:24
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MonoBehaiver
/// <summary>
public class ObjItem : UnityEngine.MonoBehaviour {
    public bool onRoad { get; set; }
    private UnityAction onOK { get; set; }
    private Material material;
    internal bool waitPick;
    public bool handed { get; private set; }
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    public void Reset()
    {
        material.color = Color.white;
    }
    public void SetChanged()
    {
        material.color = Color.green;
        waitPick = false;
    }

    public void OnPickDown()
    {
        handed = true;
        if (onOK != null)
        {
            onOK.Invoke();
        }
    }

    internal void SetOkEvent(UnityAction onOK)
    {
        this.onOK = onOK;
        waitPick = true;
    }

}
