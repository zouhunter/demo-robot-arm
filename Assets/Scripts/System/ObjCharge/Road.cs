#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 09:20:45
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MonoBehaiver
/// <summary>
public class Road : UnityEngine.MonoBehaviour {
    private Material material;
    public Vector2 offset;

    public Vector3 forward { get { return transform.forward; } }

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    public void MoveRoad(float value)
    {
        material.mainTextureOffset += offset * value;
    }
}
