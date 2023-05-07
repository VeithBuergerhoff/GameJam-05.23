using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class HotkeyHelper
{
    public static char findUniqueHotkey(string itemName, IEnumerable<char> usedHotkeys)
    {
        char hotkey;
        var uniqueCharsInName = itemName.ToUpper().Except(usedHotkeys);
        if (uniqueCharsInName.Any())
        {
            hotkey = uniqueCharsInName.First();
        }
        else
        {
            var uniqueCharsInAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Except(usedHotkeys);
            if (uniqueCharsInAlphabet.Any())
            {
                hotkey = uniqueCharsInAlphabet.ElementAt(
                    UnityEngine.Random.Range(0, uniqueCharsInAlphabet.Count() - 1)
                );
            }
            else
            {
                throw new IndexOutOfRangeException("There are no hotkeys left :(");
            }
        }
        
        return hotkey;
    }
}
