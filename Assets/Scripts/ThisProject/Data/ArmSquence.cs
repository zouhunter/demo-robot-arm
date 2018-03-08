#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 08:48:24
    * 说    明：       
* ************************************************************************************/
#endregion
using System;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 手臂运动数据
/// <summary>

[System.Serializable]
public class ArmSquence {
    public List<float> values = new List<float>();
    public ArmSquence(float[] values)
    {
        this.values.AddRange(values);
    }
}
