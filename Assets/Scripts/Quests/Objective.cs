using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using UnityEngine;

namespace RPG.Quests
{
        [System.Serializable]
        public class Objective
        {
            public string description;
            public string reference;
            public ETrigger triggerType;
        }
}