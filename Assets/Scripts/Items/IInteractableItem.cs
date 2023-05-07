using System.Collections.Generic;

public interface IInteractableItem
{
    public string GetItemName();
    public char GetCurrentHotkey();
    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys);

    public ItemController GetItemController();
}
