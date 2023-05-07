using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class HotkeyHelper
{
    public static char[] hotkeyBlacklist = {'W', 'A', 'S', 'D'};
    
    public static char findUniqueHotkey(string itemName, IEnumerable<char> usedHotkeys)
    {
        char hotkey;
        IEnumerable<char> unavailableHotkeys = hotkeyBlacklist.Concat(usedHotkeys);
        var uniqueCharsInName = itemName.ToUpper().Except(unavailableHotkeys);
        if (uniqueCharsInName.Any())
        {
            hotkey = uniqueCharsInName.First();
        }
        else
        {
            var uniqueCharsInAlphabet = "BCEFGHIJKLMNOPQRTUVXYZ".Except(unavailableHotkeys);
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
