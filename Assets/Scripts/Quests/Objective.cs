using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using UnityEngine;

namespace RPG.Quests
{
        [System.Serializable]
        public class Objective
        {
            public ETrigger triggerType;
            public string text;
            public string reference;
        }
}