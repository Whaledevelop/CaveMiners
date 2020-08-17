using UnityEngine;

/// <summary>
/// Скрипт, выделяющий каким-то образом спрайт. Сейчас он меняет материал спрайта
/// </summary>
public class Highlighter  : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Material highlightMaterial;

    private bool highlightMode;

    private Material defaultMaterial;

    void Awake()
    {
        defaultMaterial = sprite.material;
    }

    public void SwapHighlightMode()
    {
        highlightMode = !highlightMode;
        sprite.material = highlightMode ? highlightMaterial : defaultMaterial;
    }
}