using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStateManagersSet", menuName = "Sets/CharacterStateManagersSet")]
public class CharacterStatesManagersSet : RuntimeSet<CharacterStatesManager> 
{ 
    public void OnStateActionEvent(CharacterActionData actionData)
    {
        CharacterStatesManager statesManager = Items.Find(item => item == actionData.stateManager);
        Debugger.Log(statesManager);
        if (statesManager != null)
        {
            statesManager.EndState();
        }
    }
}