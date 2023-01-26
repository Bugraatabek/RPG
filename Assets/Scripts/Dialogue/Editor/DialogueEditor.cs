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
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] GUIStyle playerNodeStyle;
        [NonSerialized] GUIStyle rootNodeStyle;
        [NonSerialized] Vector2 dragginOffset;
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        [NonSerialized] DialogueNode linkingChildNode = null;
        [NonSerialized] DialogueNode unlinkingChildNode = null;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;

        const float canvasSize = 10000;
        const float backgroundSize = 50;



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

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            playerNodeStyle.border = new RectOffset(12,12,12,12);
            
            rootNodeStyle = new GUIStyle();
            rootNodeStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
            rootNodeStyle.padding = new RectOffset(20, 20, 20, 20);
            rootNodeStyle.border = new RectOffset(12,12,12,12);

        }

        private void OnSelectionChanged()
        {
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

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                
                Rect canvas = GUILayoutUtility.GetRect(canvasSize,canvasSize);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoordinates = new Rect(0,0,200,200); 

                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoordinates);


                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes()) // this is 2 different foreach loop because the first one Drawed will position underneath the second.
                {
                    DrawNode(node);
                }

                EditorGUILayout.EndScrollView();
                

                if(creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if(deletingNode != null)
                {
                    selectedDialogue.RemoveNode(deletingNode);
                    deletingNode = null;
                }
                if(linkingParentNode != null)
                {
                    if(linkingChildNode != null)
                    {
                        selectedDialogue.Link(linkingParentNode, linkingChildNode);
                        linkingParentNode = null;
                        linkingChildNode = null;
                    }
                }
                if(unlinkingChildNode != null)
                {
                    selectedDialogue.Unlink(linkingParentNode, unlinkingChildNode);
                    linkingParentNode = null;
                    unlinkingChildNode = null;
                }
            }
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtMousePosition(Event.current.mousePosition  + scrollPosition);
                if(draggingNode != null)
                {
                    dragginOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                if(draggingNode == null)
                {
                    Selection.activeObject = selectedDialogue;
                    draggingCanvas = true;
                    draggingCanvasOffset = scrollPosition + Event.current.mousePosition;
                }
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetRectPosition(Event.current.mousePosition + dragginOffset);
                    
                GUI.changed = true;
                draggingCanvas = false;
            }
            else if(Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                    draggingNode = null;
            }
            else if(Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }

            
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector2 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector2 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector2 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.6f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = nodeStyle;
            if(node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }

            if(node.GetIsARootNode() && !node.IsPlayerSpeaking())
            {
                style = rootNodeStyle;
            }

            GUILayout.BeginArea(node.GetRect(), style);
            var defaultNodeText = EditorGUILayout.TextField(node.GetText());

            node.SetText(defaultNodeText);
            
            GUILayout.BeginHorizontal();
            DrawAddRemoveButtons(node);
            GUILayout.EndHorizontal();

            DrawLinkButtons(node);

            
            GUILayout.EndArea();
        }

        private void DrawAddRemoveButtons(DialogueNode node)
        {
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }
        }
        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else
            {
                if (linkingParentNode == node)
                {
                    if (GUILayout.Button("cancel"))
                    {
                        linkingChildNode = null;
                        linkingParentNode = null;
                        return;
                    }
                }

                if (!linkingParentNode.ContainsChild(node.name) && linkingParentNode != node)
                {
                    if (GUILayout.Button("child"))
                    {
                        linkingChildNode = node;
                    }
                }

                if (linkingParentNode.ContainsChild(node.name))
                {
                    if (GUILayout.Button("unlink"))
                    {
                        unlinkingChildNode = node;
                    }
                }
            }
        }

         private DialogueNode GetNodeAtMousePosition(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (var node in selectedDialogue.GetAllNodes())
            {
                if(node.GetRect().Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }

    }
}
