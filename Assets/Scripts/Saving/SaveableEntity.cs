using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();
        
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>()) 
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state; 
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if(stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            } 
        }
        
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }
       
       
#if UNITY_EDITOR
        private void Update() 
        {   
            if(Application.IsPlaying(gameObject)) return;
            if(string.IsNullOrEmpty(gameObject.scene.path)) return;
            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }

        private bool IsUnique(string candidate)
        {
           if(!globalLookup.ContainsKey(candidate)) return true; // if it doesn't have the same key
           
           if(globalLookup[candidate] == this) return true; //if its the one we have

           if(globalLookup[candidate] == null) // if its a destroyed object's candidate
           {
             globalLookup.Remove(candidate);
             return true;
           }

           if(globalLookup[candidate].GetUniqueIdentifier() != candidate) // if it's been long time since generation and this bug came up
           {
            globalLookup.Remove(candidate);
            return true;
           }

           return false;
        }
#endif        
    }
}
