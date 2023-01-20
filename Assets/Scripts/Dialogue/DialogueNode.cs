using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    { 
        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(10,10,200,100);
        
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;

        public string GetText()
        {
            return text;
        }

        public IEnumerable<string> GetChildren()
        {
            foreach (string child in children)
            {
                yield return child;
            }
        }

        public bool ContainsChild(string name)
        {
            if(children.Contains(name)) return true;
            else return false;
        }

        public Rect GetRect()
        {
            return rect;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public string GetOnEnterAction()
        {
            return onEnterAction;
        }

        public string GetOnExitAction()
        {
            return onExitAction;
        }


#if UNITY_EDITOR
        public void SetRectPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Update Dialogue Rect"); // This doesn't mark objects as Dirty if they are sub-assets like this DialogueNode which is created as a sub-asset by Dialogue.
            rect.position = newPosition;
            EditorUtility.SetDirty(this); // # Using this to set object dirty so it is saved to the Disk from the Memory.
        }

        public void SetText(string newText)
        {
            Undo.RecordObject(this, "Update Dialogue Text");
            text = newText;
            EditorUtility.SetDirty(this); // # Uncomment these and change stuff on dialogue node in the editor. 
        }

        public void AddChild(string name)
        {
            Undo.RecordObject(this, "Added Dialogue Child");
            children.Add(name);
            EditorUtility.SetDirty(this); // # Check on SourceControl to see what assets are being saved differentely. 
        }

        public void RemoveChild(string name)
        {
            Undo.RecordObject(this, "Removed Dialogue Child");
            children.Remove(name);
            EditorUtility.SetDirty(this); // #
        }

        public void SetPlayerSpeaking(bool trueOrFalse)
        {
            Undo.RecordObject(this, "Changed Dialogue Bool");
            isPlayerSpeaking = trueOrFalse;
            EditorUtility.SetDirty(this); // #
        }
#endif
    }
}
