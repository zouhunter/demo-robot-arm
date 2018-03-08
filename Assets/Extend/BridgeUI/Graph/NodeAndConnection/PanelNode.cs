﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Sprites;
using UnityEngine.Scripting;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Assertions.Must;
using UnityEngine.Assertions.Comparers;
using System.Collections;
using NodeGraph.DataModel;
using NodeGraph;
using System;
using BridgeUI.Model;
using BridgeUI;
using System.Collections.Generic;

[CustomNode("Panel", 1,"BridgeUI")]
public class PanelNode : PanelNodeBase
{
    public PanelNode() { }
    protected override IEnumerable<Point> inPoints
    {
        get
        {
            return new Point[] { new Point("","bridge",100) };
        }
    }
    protected override IEnumerable<Point> outPoints
    {
        get
        {
            return new Point[] { new Point("", "bridge", 100) };
        }
    }
    public string prefabPath
    {
        set
        {
#if UNITY_EDITOR
            Info.prefabGuid = UnityEditor.AssetDatabase.AssetPathToGUID(value);
#endif
        }
    }

}
