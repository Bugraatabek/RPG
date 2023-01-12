using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        
#if UNITY_EDITOR
        private void Awake() 
        {
            
            if(nodes.Count == 0)
            {
                var defaultNode = new DialogueNode();
                nodes.Add(defaultNode);
                defaultNode.uniqueID = System.Guid.NewGuid().ToString();
            }
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
            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            var childeren = parent.children;
            foreach (String child in childeren)
            {
                if(nodeLookup.ContainsKey(child))
                {
                    yield return nodeLookup[child];
                }
            }
        }
        
        public void AddNode(DialogueNode parent)
        {
            string newUniqueID = System.Guid.NewGuid().ToString();
            var newNode = new DialogueNode();
            newNode.uniqueID = newUniqueID;
            parent.children.Add(newUniqueID);
            nodes.Add(newNode);
            nodeLookup.Add(newUniqueID, newNode); // if any problems just use OnValidate();
        }

        public void RemoveNode(DialogueNode deletingNode)
        {
            nodes.Remove(deletingNode);
            nodeLookup.Remove(deletingNode.uniqueID); // if any problems just use OnValidate();
            CleanDanglingChildren(deletingNode);
        }

        private void CleanDanglingChildren(DialogueNode deletingNode)
        {
            foreach (var node in nodes)
            {
                if (node.children.Contains(deletingNode.uniqueID))
                {
                    node.children.Remove(deletingNode.uniqueID);
                }
            }
        }

        public void Link(DialogueNode linkingParentNode, DialogueNode linkingChildNode)
        {
            if(linkingParentNode.uniqueID == linkingChildNode.uniqueID) 
            {
                return;
            }    
            linkingParentNode.children.Add(linkingChildNode.uniqueID);
        }

        public void Unlink(DialogueNode linkingParentNode, DialogueNode unlinkingChildNode)
        {
            linkingParentNode.children.Remove(unlinkingChildNode.uniqueID);
        }
    }
}
