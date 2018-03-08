#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 07:27:37
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 任意手臂
/// <summary>
public class Arm : UnityEngine.MonoBehaviour
{

    [SerializeField]
    private Vector3 axis;
    [SerializeField]
    private Transform container;

    public float rotateSpeed = 30;

    public Transform childArm
    {
        get
        {
            if (container.childCount == 0)
            {
                return null;
            }
            else
            {
                return container.GetChild(0);
            }
        }
    }
    private Quaternion target;
    private Quaternion startRot;
    private bool lerpRotation;
    private float timer;
    private IEnumerator Start()
    {
        while (true)
        {
            yield return null;
            if (childArm)
            {
                if (lerpRotation && childArm.localRotation != target)
                {
                   float time = 1f / (rotateSpeed * Time.deltaTime);

                    for (timer = 0; timer < time; timer += Time.deltaTime)
                    {
                        var ret = timer / time;
                        childArm.localRotation = Quaternion.Lerp(startRot, target, ret);
                        yield return null;
                    }
                    lerpRotation = false;
                }
            }
        }

    }

    public void SetValue(float value)
    {
        if (childArm)
        {
            childArm.localRotation = Quaternion.Euler(axis * value);
        }
    }

    internal void SetValueTarget(float value)
    {
        //lerp
        startRot = childArm.localRotation;
        target = Quaternion.Euler(axis * value);
        timer = 0;
        Debug.Log("value:" + value);
        lerpRotation = true;
    }

}
