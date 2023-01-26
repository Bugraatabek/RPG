    [System.Serializable]
    public enum EPredicate
    {
        Select,   //0 - no handlers will be handling Select, it's a placeholder to remind us to choose a predicate
        HasQuest, //1, QuestList needs the name of the quest
        CompletedObjective, //2, QuestList needs the name of the quest and the identifier of the objective
        CompletedQuest, //1, QuestList needs the name of the quest
        HasLevel, //1, BaseStats needs to know the level we're shooting for
        MinimumTrait, //2, TraitStore needs the trait and the minimum level of the trait
        HasItem, //1, Inventory needs the ID of the item
        HasItems, //2, Inventory needs the ID of the item and the required quantity
        HasItemEquipped //1, Equipment needs the ID of the item.

    }