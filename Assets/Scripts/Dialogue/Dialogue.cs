using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
       
        
#if UNITY_EDITOR
        private void Awake() 
        {
            OnValidate();
        }
#endif
        private void OnValidate() 
        {
            BuildLookupDict();    
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        private void BuildLookupDict()
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            var childeren = parent.GetChildren();
            foreach (String child in childeren)
            {
                if(nodeLookup.ContainsKey(child))
                {
                    yield return nodeLookup[child];
                }
            }
        }
        
        public void CreateNode(DialogueNode parent)
        {
            string newUniqueID = System.Guid.NewGuid().ToString();
            var newNode = CreateInstance<DialogueNode>();
            newNode.name = newUniqueID;
            Undo.RegisterCreatedObjectUndo(newNode, "Creating a new node");
            if (parent != null)
            {
                parent.AddChild(newUniqueID);
            }
            Undo.RecordObject(this, "Created Dialogue Node");
            nodes.Add(newNode);
            OnValidate(); // if any problems just use OnValidate();

        }


        public void RemoveNode(DialogueNode deletingNode)
        {
            Undo.RecordObject(this , "Deleted Dialogue Node");
            nodes.Remove(deletingNode);
            OnValidate();// if any problems just use OnValidate();
            CleanDanglingChildren(deletingNode);
            Undo.DestroyObjectImmediate(deletingNode);
        }

        private void CleanDanglingChildren(DialogueNode deletingNode)
        {
            foreach (var node in GetAllNodes())
            {
                if (node.ContainsChild(deletingNode.name))
                {
                    node.RemoveChild(deletingNode.name);
                }
            }
        }

        public void Link(DialogueNode linkingParentNode, DialogueNode linkingChildNode)
        {
            if(linkingParentNode.name == linkingChildNode.name) 
            {
                return;
            } 
            linkingParentNode.AddChild(linkingChildNode.name);
        }

        public void Unlink(DialogueNode linkingParentNode, DialogueNode unlinkingChildNode)
        {
            linkingParentNode.RemoveChild(unlinkingChildNode.name);
        }

        public void OnBeforeSerialize()
        {
            if(AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node ,this);
                    }
                }
            }
            if(nodes.Count == 0)
            {
                CreateNode(null);
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
