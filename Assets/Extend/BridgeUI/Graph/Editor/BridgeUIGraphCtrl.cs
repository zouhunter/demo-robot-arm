using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography;

using NodeGraph;
using NodeGraph.DataModel;
using BridgeUI.Model;

namespace BridgeUI
{
    public class BridgeUIGraphCtrl : NodeGraphController
    {
        private const string prefer_scriptPath = "BridgeUIPanelNames_path";
        public override string Group
        {
            get
            {
                return "BridgeUI";
            }
        }

        /// <summary>
        /// ����Ϣ�����浽PanelGroup
        /// </summary>
        /// <param name="group"></param>
        private void StoreInfoOfPanel(PanelGroup group)
        {
            InsertBridges(group.bridges, GetBridges());
            if (group.loadType == LoadType.Prefab)
            {
                InsertPrefabinfo(group.p_nodes, GetPrefabUIInfos(GetNodeInfos()));
            }
            else if (group.loadType == LoadType.Bundle)
            {
                InsertBundleinfo(group.b_nodes, GetBundleUIInfos(GetNodeInfos()));
            }
            TryRecoredGraphGUID(group);
            EditorUtility.SetDirty(group);
        }

        private void TryRecoredGraphGUID(PanelGroup group)
        {
            var path = AssetDatabase.GetAssetPath(TargetGraph);
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (group is PanelGroup)
            {
                var panelGroup = group as PanelGroup;
                var record = panelGroup.graphList.Find(x => x.guid == guid);
                if (record == null)
                {
                    var item = new GraphWorp(TargetGraph.name, guid);
                    panelGroup.graphList.Add(item);
                }
                else
                {
                    record.graphName = TargetGraph.name;
                }
            }
        }

        private void InsertBridges(List<BridgeInfo> source, List<BridgeInfo> newBridges)
        {
            if (newBridges == null) return;
            foreach (var item in newBridges)
            {
                if (string.IsNullOrEmpty(item.outNode)) continue;
                source.RemoveAll(x => (x.inNode == item.inNode || (string.IsNullOrEmpty(x.inNode) && string.IsNullOrEmpty(item.inNode))) && x.outNode == item.outNode);
                source.Add(item);
            }
        }
        private void InsertPrefabinfo(List<PrefabUIInfo> source, List<PrefabUIInfo> newInfo)
        {
            if (newInfo == null) return;
            foreach (var item in newInfo)
            {
                var old = source.Find(x => x.panelName == item.panelName);
                if (old != null)
                {
                    old.prefab = item.prefab;
                    old.type = item.type;
                }
                else
                {
                    source.Add(item);
                }
            }
        }
        private void InsertBundleinfo(List<BundleUIInfo> source, List<BundleUIInfo> newInfo)
        {
            if (newInfo == null) return;
            foreach (var item in newInfo)
            {
                CompleteBundleUIInfo(item);

                var old = source.Find(x => x.panelName == item.panelName);

                if (old != null)
                {
                    old.guid = item.guid;
                    old.type = item.type;
                }
                else
                {
                    source.Add(item);
                }
            }
        }
        private List<BridgeInfo> GetBridges()
        {
            var nodes = TargetGraph.Nodes;
            var connectons = TargetGraph.Connections;
            var bridges = new List<BridgeInfo>();
            foreach (var item in connectons)
            {
                if (!(item.Object is BridgeConnection)) continue;
                var connection = item.Object as BridgeConnection;

                var bridge = new BridgeInfo();
                var innode = nodes.Find(x => x.OutputPoints != null && x.OutputPoints.Find(y => y.Id == item.FromNodeConnectionPointId) != null);
                var outnode = nodes.Find(x => x.InputPoints != null && x.InputPoints.Find(y => y.Id == item.ToNodeConnectionPointId) != null);
                if (innode != null)
                {
                    if (innode.Object is IPanelInfoHolder)
                    {
                        bridge.inNode = innode.Name;
                    }
                }

                if (outnode != null && outnode.Object is IPanelInfoHolder)
                {
                    bridge.outNode = outnode.Name;
                }

                bridge.showModel = connection.show;
                bridge.index = connection.index;
                bridges.Add(bridge);
            }
            return bridges;
        }

        private List<NodeInfo> GetNodeInfos()
        {
            var nodeInfos = new List<NodeInfo>();
            var nodes = TargetGraph.Nodes;
            foreach (var item in nodes)
            {
                var nodeItem = item.Object as IPanelInfoHolder;
                if (nodeItem != null)
                {
                    nodeInfos.Add(nodeItem.Info);
                }
            }
            return nodeInfos;
        }

        private List<PrefabUIInfo> GetPrefabUIInfos(List<NodeInfo> infos)
        {
            var pinfos = new List<PrefabUIInfo>();
            foreach (var item in infos)
            {
                var p = new PrefabUIInfo();
                p.type = item.uiType;
                p.prefab = LoadPrefabFromGUID(item.prefabGuid);
                p.panelName = p.prefab.name;
                pinfos.Add(p);
            }
            return pinfos;
        }

        private GameObject LoadPrefabFromGUID(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
            {
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
            else
            {
                return null;
            }
        }

        private List<BundleUIInfo> GetBundleUIInfos(List<NodeInfo> infos)
        {
            var binfo = new List<BundleUIInfo>();
            foreach (var item in infos)
            {
                var p = new BundleUIInfo();
                p.type = item.uiType;
                p.guid = item.prefabGuid;
                binfo.Add(p);
            }
            return binfo;
        }

        private void CompleteBundleUIInfo(BundleUIInfo binfo)
        {
            if (string.IsNullOrEmpty(binfo.guid))
            {
                return;
            }
            else
            {
                var path = AssetDatabase.GUIDToAssetPath(binfo.guid);
                var importer = AssetImporter.GetAtPath(path);
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (importer)
                {
                    binfo.bundleName = importer.assetBundleName = Setting.bundleNameBase + obj.name.ToLower();
                    binfo.panelName = obj.name;
                    binfo.good = true;
                    EditorUtility.SetDirty(importer);
                }
                else
                {
                    binfo.good = false;
                }
            }
        }

        internal override void Validate(NodeGUI node)
        {
            var changed = false;
            if (node.Data.Object is IPanelInfoHolder)
            {
                var nodeItem = node.Data.Object as IPanelInfoHolder;
                var guid = nodeItem.Info.prefabGuid;
                if (!string.IsNullOrEmpty(guid) && !string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid)))
                {
                    node.ResetErrorStatus();
                    changed = true;
                }
            }
            if (changed)
            {
                Perform();
            }
        }
        protected override void JudgeNodeExceptions(NodeGraphObj m_targetGraph, List<NodeException> m_nodeExceptions)
        {
            foreach (var item in TargetGraph.Nodes)
            {
                if (item.Object is IPanelInfoHolder)
                {
                    var nodeItem = item.Object as IPanelInfoHolder;
                    var guid = nodeItem.Info.prefabGuid;
                    if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid)))
                    {
                        m_nodeExceptions.Add(new NodeException("prefab is null", item.Id));
                    }
                }
            }
        }

        protected override void BuildFromGraph(NodeGraphObj m_targetGraph)
        {
            if (Selection.activeGameObject != null)
            {
                var panelGroup = Selection.activeGameObject.GetComponent<PanelGroup>();
                if (panelGroup != null)
                {
                    StoreInfoOfPanel(panelGroup);
                }
            }
            UpdateScriptOfPanelNames(m_targetGraph.Nodes.FindAll(x => x.Object is PanelNodeBase).ConvertAll<string>(x => x.Name));
        }
        private void UpdateScriptOfPanelNames(List<string> list)
        {
            var path = PlayerPrefs.GetString(prefer_scriptPath);
            bool needOpenSelect = false;
            string directory = null;
            if (!string.IsNullOrEmpty(path))
            {
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
                if (script == null)
                {
                    needOpenSelect = true;
                    directory = System.IO.Path.GetDirectoryName(path);
                }
            }
            if (string.IsNullOrEmpty(path))
            {
                needOpenSelect = true;
                directory = Application.dataPath;
            }
            if (needOpenSelect)
            {
                path = EditorUtility.SaveFilePanel("��ѡ��PanelNames.cs����·��", directory, "PanelNames", "cs");
                if (!string.IsNullOrEmpty(path)) PlayerPrefs.SetString(prefer_scriptPath, path);
            }

            if (!string.IsNullOrEmpty(path))
            {
                new PanelNameGenerater(path).GenerateParcialPanelName(list.ToArray());
            }
        }

        internal override void OnDragUpdated()
        {
            base.OnDragUpdated();
            foreach (UnityEngine.Object obj in DragAndDrop.objectReferences)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path) && obj is GameObject)
                {
                    path = GetInstenceObjectPath(obj as GameObject);
                }


                if (!string.IsNullOrEmpty(path))
                {
                    FileAttributes attr = File.GetAttributes(path);

                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory || obj is GameObject)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        break;
                    }
                    else
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        break;
                    }
                }
            }
        }

        protected static string GetInstenceObjectPath(GameObject instenceObj)
        {
            var pfbTrans = PrefabUtility.GetPrefabParent(instenceObj);
            if (pfbTrans != null)
            {
                var prefab = PrefabUtility.FindPrefabRoot(pfbTrans as GameObject);
                if (prefab != null)
                {
                    return AssetDatabase.GetAssetPath(prefab);
                }
            }
            return null;
        }
        internal override List<KeyValuePair<string, Node>> OnDragAccept(UnityEngine.Object[] objectReferences)
        {
            var nodeList = new List<KeyValuePair<string, Node>>();
            foreach (UnityEngine.Object obj in DragAndDrop.objectReferences)
            {
                var path = AssetDatabase.GetAssetPath(obj);

                if (string.IsNullOrEmpty(path) && obj is GameObject)
                {
                    path = GetInstenceObjectPath(obj as GameObject);
                }

                if (!string.IsNullOrEmpty(path))
                {
                    FileAttributes attr = File.GetAttributes(path);
                    PanelNode panelNode = null;
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        var files = System.IO.Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
                        foreach (var item in files)
                        {
                            panelNode = ScriptableObject.CreateInstance<PanelNode>();
                            panelNode.prefabPath = item;
                            panelNode.name = typeof(PanelNode).FullName;
                            nodeList.Add(new KeyValuePair<string, Node>(Path.GetFileNameWithoutExtension(item), panelNode));
                        }
                    }
                    else if (obj is GameObject)
                    {
                        panelNode = ScriptableObject.CreateInstance<PanelNode>();
                        panelNode.prefabPath = path;

                        panelNode.name = typeof(PanelNode).FullName;
                        nodeList.Add(new KeyValuePair<string, Node>(obj.name, panelNode));
                    }
                }
            }
            return nodeList;
        }

    }
}
