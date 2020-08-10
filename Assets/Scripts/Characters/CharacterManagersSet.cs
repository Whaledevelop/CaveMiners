using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterManagersSet", menuName = "Sets/CharacterManagersSet")]
public class CharacterManagersSet : RuntimeSet<CharacterManager>
{
    [NonSerialized] private CharacterManager activeCharacter;

    public CharacterManager ActiveCharacter => activeCharacter;

    public void ExecuteTask(int layer, Vector2 position)
    {
        if (activeCharacter != null)
        {
            activeCharacter.ExecuteTask(layer, position);
        }
        else
        {
            Debug.Log("No active character for ground task");
        }        
    }

    public void SetActiveCharacter(CharacterManager newActiveCharacter)
    {
        if (activeCharacter != newActiveCharacter)
        {
            if (activeCharacter != null)
                activeCharacter.OnBecomeNotActive();

            activeCharacter = newActiveCharacter;
            activeCharacter.OnBecomeActive();
        }
    }
}
