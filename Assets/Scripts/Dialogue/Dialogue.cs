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
#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Creating a new node");
            Undo.RecordObject(this, "Created Dialogue Node");
            AddNode(newNode);

        }

        public void RemoveNode(DialogueNode deletingNode)
        {
            Undo.RecordObject(this , "Deleted Dialogue Node");
            nodes.Remove(deletingNode);
            OnValidate();// if any problems just use OnValidate();
            CleanDanglingChildren(deletingNode);
            Undo.DestroyObjectImmediate(deletingNode);
        }

        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }

        private static DialogueNode MakeNode(DialogueNode parent)
        {
            string newUniqueID = System.Guid.NewGuid().ToString();
            var newNode = CreateInstance<DialogueNode>();
            newNode.name = newUniqueID;
            if (parent != null)
            {
                parent.AddChild(newUniqueID);
            }

            return newNode;
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
#endif        

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR            
            if(nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }
#endif    
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
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
