using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        DialogueNode draggingNode = null;
        GUIStyle nodeStyle;
        Vector2 dragginOffset;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable() 
        {
            Selection.selectionChanged += OnSelectionChanged;
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12,12,12,12);
        }

        private void OnSelectionChanged()
        {
            ;
            var newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI() 
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtMousePosition(Event.current.mousePosition);
                if(draggingNode != null)
                {
                    dragginOffset = draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Node");
                draggingNode.rect.position = Event.current.mousePosition + dragginOffset;
                    
                GUI.changed = true;
            
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                    draggingNode = null;
            }
            
        }

        private DialogueNode GetNodeAtMousePosition(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (var node in selectedDialogue.GetAllNodes())
            {
                
                if(node.rect.Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

        private void OnGUINode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLargeLabel);
            var newUniqueID = EditorGUILayout.TextField(node.uniqueID);
            var defaultNodeText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");

                node.uniqueID = newUniqueID;
                node.text = defaultNodeText;
            }
            GUILayout.EndArea();
        }
    }
}
