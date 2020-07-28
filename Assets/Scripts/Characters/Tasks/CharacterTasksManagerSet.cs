using System;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CharacterTaskManagersSet", menuName = "Sets/CharacterTaskManagersSet")]
public class CharacterTasksManagersSet : RuntimeSet<CharacterTasksManager> 
{
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private CellCenterRequest cellCenterRequest;

    [NonSerialized] private CharacterTasksManager activeCharacter;

    public void OnClickOnMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            cellCenterRequest.MakeRequest(new ParamsObject(clickPosition), out clickPosition);
            RaycastHit2D characterHit = Physics2D.Raycast(clickPosition, Vector2.zero, 1, characterLayer);
            if (characterHit)
            {
                CharacterTasksManager chosenCharacter = characterHit.collider.gameObject.GetComponent<CharacterTasksManager>();
                if (chosenCharacter != null)
                    SetActiveCharacter(chosenCharacter);
                else
                    Debug.Log("Character has no task manager");
            }
            else
            {
                RaycastHit2D anyColliderHit = Physics2D.Raycast(clickPosition, Vector2.zero);
                if (anyColliderHit)
                {
                    if (activeCharacter != null)
                        activeCharacter.ExecuteTask(anyColliderHit.collider.gameObject, clickPosition);
                    else
                        Debug.Log("No active character for ground task");
                }
            }
        }
    }


    public void SetActiveCharacter(CharacterTasksManager newActiveCharacter)
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