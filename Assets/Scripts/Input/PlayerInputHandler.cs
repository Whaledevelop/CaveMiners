using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Слой персонажа")]
        [SerializeField] 
        private LayerMask characterLayer;

        [Header("Слой, на котором не выполняются никакие действия")]
        [SerializeField] 
        private LayerMask noActionLayer;

        [SerializeField]
        CharactersSet charactersSet;

        [SerializeField]
        CameraController cameraController;

        public void OnLookInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                cameraController.OnLookInput(context.ReadValue<Vector2>());
            }
        }

        public void OnExecuteInput(InputAction.CallbackContext context)
        {
            cameraController.OnExecuteInput(context.phase);
            if (!context.performed)
            {
                return;
            }
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D characterHit = Physics2D.Raycast(clickPosition, Vector2.zero, 1, characterLayer);
            if (characterHit)
            {
                charactersSet.ActiveCharacter = characterHit.collider.gameObject.GetComponent<Character>();
                return;
            }
            if (Physics2D.Raycast(clickPosition, Vector2.zero, 1, noActionLayer))
            {
                return;
            }
            if (charactersSet.ActiveCharacter == null)
            {
                return;
            }
            RaycastHit2D anyColliderHit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (!anyColliderHit)
            {
                return;
            }
            charactersSet.ActiveCharacter.GetManager<CharacterTaskManager>().ExecuteTask(anyColliderHit.collider.gameObject.layer, clickPosition);
        }

        public void OnUseInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                cameraController.SetNextCameraMode();
            }
        }

        public void OnScrollInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                cameraController.OnScrollInput(context.ReadValue<Vector2>().normalized);
            }
        }
    }
}