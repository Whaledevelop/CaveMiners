using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStateManagersSet", menuName = "Sets/CharacterStateManagersSet")]
public class CharacterStatesManagersSet : RuntimeSet<CharacterStatesManager> 
{ 
    public void OnStateActionEvent()
    {
        // Полная фигня
        Items[0].EndState();
    }
}