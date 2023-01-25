using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(EPredicate predicate, string[] parameters);
    }
}
