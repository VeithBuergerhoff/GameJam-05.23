using System.Collections.Generic;

public interface IInteractableItem
{
    public string GetItemName();
    public char GetPreferredHotkey();
    public char GetUniqueHotkey(IEnumerable<char> usedHotkeys);

    public ItemController GetItemController();
}
