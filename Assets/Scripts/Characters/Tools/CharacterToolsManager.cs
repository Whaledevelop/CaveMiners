using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;


public enum ToolCode
{
    None, Default, Bag, Pick, Shovel
}

/// <summary>
/// Обработчик смены инструментов/предметов в руках персонажа
/// </summary>

public class CharacterToolsManager : CharacterManager
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private List<ToolEquipData> toolsEquipData = new List<ToolEquipData>();

    private ToolCode defaultTool;

    public ToolCode DefaultTool => defaultTool;

    public void SetDefaultTool(ToolCode toolCode)
    {
        defaultTool = toolCode;
    }

    public void ChangeToolMode(ToolCode toolCode, bool toolMode)
    {
        if (toolCode == ToolCode.Default)
            toolCode = defaultTool;
        ToolEquipData toolData = toolsEquipData.Find(toolEquipData => toolEquipData.toolCode == toolCode);
        if (toolData != null)
        {
            // Смену каждого инструмента можно реализовать двумя способами. Через смену всего спрайта или через инициализацию 
            // дополнительного gameObject с инструментом. Изначально я делал отдельные спрайты персонажа для каждого инструмента. Для
            // моих спрайтов автоматических риггинг работает некорректно, а вручную это занимает время. Т.е. введение новых инструментов
            // становится довольно муторно. Но с таким способом можно лучше регулировать "глубину" инструмента - например, чтобы он был за головой, но перед телом.
            // Метод с геймобъектом быстрее, но там не получится сделать разное отношение глубин для разных частей тела - только для всего тела целиком.
            // Поэтому я оставил оба способа, потому что где-то без замены спрайта не обойтись (как, например, с лопатой), а где-то хватит и геймобъекта (мешок)
            if (toolData.isToUseSpriteSwitch)
            {
                spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), toolMode ? toolData.spriteResolveLabel : "Main");
            }
            else
            {
                toolData.ChangeMode(toolMode);
            }
        }
    }
    
    public void HideTool(ToolCode toolCode)
    {
        if (toolCode != ToolCode.None)
            ChangeToolMode(toolCode, false);
    }

    public void ApplyTool(ToolCode toolCode)
    {
        // При применении какого-то инструмента убираем остальные предметы. Несколько предметов одновременно пока не отрабатывал
        foreach (ToolEquipData toolEquipData in toolsEquipData)
        {
            if (toolCode != toolEquipData.toolCode && !toolEquipData.isToUseSpriteSwitch)
                toolEquipData.Destroy();
        }
        if (toolCode != ToolCode.None)
            ChangeToolMode(toolCode, true);
    }
}
