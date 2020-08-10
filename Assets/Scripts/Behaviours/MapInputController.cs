using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MapInputController : MonoBehaviour
{
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private CharacterManagersSet charactersSet;

    public void OnClickOnMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D characterHit = Physics2D.Raycast(clickPosition, Vector2.zero, 1, characterLayer);
            if (characterHit)
            {
                CharacterManager chosenCharacter = characterHit.collider.gameObject.GetComponent<CharacterManager>();
                if (chosenCharacter != null)
                    charactersSet.SetActiveCharacter(chosenCharacter);
                else
                    Debug.Log("Character has no task manager");
            }
            else
            {
                RaycastHit2D anyColliderHit = Physics2D.Raycast(clickPosition, Vector2.zero);
                if (anyColliderHit)
                {
                    charactersSet.ExecuteTask(anyColliderHit.collider.gameObject.layer, clickPosition);                        
                }
            }
        }
    }
}
