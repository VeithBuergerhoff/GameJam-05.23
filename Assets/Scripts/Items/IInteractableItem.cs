using System.Collections.Generic;

public interface IInteractableItem {
    

public char GetHotkey(int n = 0);
public char GetUniqueHotkey(IEnumerable<char> usedHotkeys);
}