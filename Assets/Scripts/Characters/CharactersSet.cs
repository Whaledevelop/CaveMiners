using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CharactersSet", menuName = "Sets/CharactersSet")]
public class CharactersSet : RuntimeSet<Character>
{
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private LayerMask noActionLayer;

    [NonSerialized] private Character activeCharacter;

    public Character ActiveCharacter => activeCharacter;

    public void OnClickOnMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D characterHit = Physics2D.Raycast(clickPosition, Vector2.zero, 1, characterLayer);
            if (characterHit)
            {
                SetActiveCharacter(characterHit.collider.gameObject.GetComponent<Character>());
            }
            else
            {
                // Для объектов, скрывающие под собой другие объекты. Пока эти объекты есть, нельзя взаимодействовать с теми,
                // что под ними. Так, например, с туманом войны. В будущем планируются сундуки или объекты за решеткой.
                // Если не проверять по этому внешнему слою, то он будет улавливать внутренний. Может я чего-то не понимаю,
                // но эксперименты с уровнем вложенности в иерархии, z, порядком слоев не дали результатов, поэтому так
                if (!Physics2D.Raycast(clickPosition, Vector2.zero, 1, noActionLayer))
                {
                    RaycastHit2D anyColliderHit = Physics2D.Raycast(clickPosition, Vector2.zero);
                    if (anyColliderHit)
                    {
                        if (activeCharacter != null)
                        {
                            activeCharacter.ExecuteTask(anyColliderHit.collider.gameObject.layer, clickPosition);
                        }
                        else
                        {
                            Debug.Log("No active character for ground task");
                        }
                    }
                    else
                    {
                        Debug.Log("No collider for click");
                    }
                }
                else
                {
                    Debug.Log("No action object");
                }
            }
        }
    }

    public void SetActiveCharacter(Character newActiveCharacter)
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
