#region statement
/*************************************************************************************   
    * 作    者：       zouhunter
    * 时    间：       2018-03-08 09:04:20
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
public class ObjCharge : UnityEngine.MonoBehaviour
{
    public static ObjCharge Instence;

    public Transform spanPos;
    public Transform hidePos;
    public Transform waitPos;
    public bool active { get; set; }
    public Road road;
    public float roadSpeed = 30;
    public float spanTime = 1;
    private List<ObjItem> created = new List<ObjItem>();
    private List<ObjItem> objPool = new List<ObjItem>();
    public const int objItemLayer = 10;
    private void Awake()
    {
        Instence = this;
    }
    private IEnumerator Start()
    {
        float timer = 0;

        while (true)
        {
            yield return null;
            if (active)
            {
                timer += Time.deltaTime;
                if (timer > spanTime)
                {
                    timer = 0;
                    GetOneObj();
                }

                UpdateCreatedPositon();
            }
        }
    }

    private void UpdateCreatedPositon()
    {
        var moved = roadSpeed * Time.deltaTime;

        foreach (var item in created)
        {
            if (item.onRoad && item.gameObject.activeSelf)
            {
                item.transform.position += moved * road.forward;
                if (!item.handed)
                {
                    if (Vector3.Dot(waitPos.position - item.transform.position, road.forward) < 0)
                    {
                        active = false;
                        item.SetOkEvent(() => { active = true; });
                    }
                }
                else if (Vector3.Dot(hidePos.position - item.transform.position, road.forward) < 0)
                {
                    RemoveItem(item);
                }
            }
        }
        road.MoveRoad(moved);
    }
    private void RemoveItem(ObjItem item)
    {
        objPool.Add(item);
        item.gameObject.SetActive(false);
    }

    private void GetOneObj()
    {
        ObjItem item = null;
        if (objPool.Count > objItemLayer)
        {
            item = CreateFromList();
        }
        else
        {
            item = CreateRundomObject();
        }

        InitObj(item);
    }

    private ObjItem CreateFromList()
    {
        var id = UnityEngine.Random.Range(0, objPool.Count);
        var item = objPool[id];
        objPool.Remove(item);
        return item;
    }


    private ObjItem CreateRundomObject()
    {
        var types = new PrimitiveType[] { PrimitiveType.Cube, PrimitiveType.Sphere };
        var type = types[UnityEngine.Random.Range(0, 2)];
        var go = GameObject.CreatePrimitive(type);
        var item = go.AddComponent<ObjItem>();
        go.layer = 10;
        go.transform.SetParent(transform);
        created.Add(item);
        return item;
    }

    private void InitObj(ObjItem item)
    {
        item.transform.position = spanPos.position;
        item.onRoad = true;
        item.Reset();
        item.gameObject.SetActive(true);
    }
}
