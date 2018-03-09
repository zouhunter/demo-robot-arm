#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 07:28:09
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
/// 手臂机器人
/// <summary>
[ExecuteInEditMode]
public class Robot : UnityEngine.MonoBehaviour {
    [SerializeField]
    private Arm[] arms;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private Transform hand;
    [SerializeField]
    private float radio = 2;
    public static Robot Instence;
    public const int objLayers = 10;
    private bool pickUped;
    private bool delyPickDown;
    private bool delyContinue;


    private IProxy<ArmSquenceList> pickupSequenceProxy;
    private IProxy<ArmSquenceList> pickdownSequenceProxy;

    public ArmSquenceList pickupSequence { get { return pickupSequenceProxy.Data; } }
    public ArmSquenceList pickdownSequence { get { return pickdownSequenceProxy.Data; } }

    private const float delyTime = 3;
    private float timer;
    private ObjItem pickupedItem;
    private bool stop;
    private void Awake()
    {
        InitArms();
        Instence = this;
        InitProxy();
    }
    private void Update()
    {
        if (stop) return;

        if (!pickUped)
        {
            FindAndTryPickupObj();
        }
        else if(delyPickDown)
        {
            timer += Time.deltaTime;
            if(timer > delyTime)
            {
                timer = 0;
                delyPickDown = false;
                TryMoveObject();
            }
        }
        else if(delyContinue)
        {
            timer += Time.deltaTime;
            if (timer > delyTime)
            {
                timer = 0;
                delyContinue = false;
                TryReleaseObject();
            }
            else
            {
                pickupedItem.transform.position = hand.position;
            }
        }
    }
    private void InitProxy()
    {
        Facade.RetrieveProxy<ArmSquenceList>(GameManager.pickupSequenceProxy, (x) => { pickupSequenceProxy = x;  });
        Facade.RetrieveProxy<ArmSquenceList>(GameManager.pickdownSequenceProxy, (x) => { pickdownSequenceProxy = x;  });
    }

    private void FindAndTryPickupObj()
    {
        var colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (var item in colliders)
        {
            if(item.gameObject.layer == 10)
            {
                pickupedItem  = item.GetComponent<ObjItem>();
                if(pickupedItem != null && pickupedItem.waitPick && !pickupedItem.handed)
                {
                    pickUped = true;
                    
                    if (pickupSequence.armList.Count >0)
                    {
                        SetValuesDely(pickupSequence.armList[0].values.ToArray());
                        delyPickDown = true;
                    }
                   
                }
            }
        }
    }

    private void TryMoveObject()
    {
        if (pickdownSequence.armList.Count > 0)
        {
            SetValuesDely(pickdownSequence.armList[0].values.ToArray());
            pickupedItem.SetChanged();
            //pickupedItem.transform.localPosition = Vector3.zero;
            delyContinue = true;
            AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
        }
    }

    private void TryReleaseObject()
    {
        if(pickupedItem != null)
        {
            SetValuesDely(new float[] { 0,0,0,0,0});

            pickupedItem.OnPickDown();
            pickUped = false;
        }
    }

    private void InitArms()
    {
        arms = GetComponentsInChildren<Arm>();
        foreach (var item in arms){
            item.SetValue(0);
        }
    }

    public void Reset()
    {
        InitArms();
        stop = false;
    }

    public void SetValuesDely(float[] values)
    {
        if(values != null)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if(arms.Length > 0)
                {
                    arms[i].SetValueTarget(values[i]);
                }
            }
        }
    }
    public void Stop()
    {
        stop = true;
    }
    public void SetArmValue(int id,float value)
    {
        if(arms.Length > id)
        {
            arms[id].SetValue(value);
        }
    }
}
