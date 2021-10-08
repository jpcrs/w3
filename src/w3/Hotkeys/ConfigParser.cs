using System;
using System.Collections.Generic;
using System.IO;
using w3.Desktop;

namespace w3.Hotkeys
{
    public class ConfigParser
    {
        public readonly Dictionary<string, string> Config = new(StringComparer.OrdinalIgnoreCase);
        private readonly DesktopManager _desktopManager;

        public ConfigParser(DesktopManager desktopManager)
        {
            ParseConfig();
            _desktopManager = desktopManager;
        }

        public static string ReadConfig()
        {
            return File.ReadAllText(@"config");
        }
        public void ParseConfig()
        {
            foreach (var s in ReadConfig().SplitLines())
            {
                if (!s.StartsWith("bindsym"))
                    continue;
                
                var firstSpace = s.IndexOf(' ')+1;
                var secondSpace = s[8..].IndexOf(' ');
                var hotkey = s[firstSpace..(firstSpace+secondSpace)];
                var command = s[(firstSpace+secondSpace+1)..];
                Config.Add(hotkey.ToString().ToLower(), command.ToString().ToLower());
            }
        }

        public bool ExecuteCommand(string shortcut)
        {
            Console.WriteLine(shortcut);
            if (!Config.TryGetValue(shortcut, out var command))
            {
                return false;
            }

            switch (command)
            {
                case "workspacenumber1": _desktopManager.GoToWorkspace(0); break;
                case "workspacenumber2": _desktopManager.GoToWorkspace(1); break;
                case "workspacenumber3": _desktopManager.GoToWorkspace(2); break;
                case "workspacenumber4": _desktopManager.GoToWorkspace(3); break;
                case "workspacenumber5": _desktopManager.GoToWorkspace(4); break;
                case "workspacenumber6": _desktopManager.GoToWorkspace(5); break;
                case "workspacenumber7": _desktopManager.GoToWorkspace(6); break;
                case "workspacenumber8": _desktopManager.GoToWorkspace(7); break;
                case "workspacenumber9": _desktopManager.GoToWorkspace(8); break;
                case "workspacenumber10": _desktopManager.GoToWorkspace(9); break;
                case "movecontainertoworkspacenumber1": _desktopManager.MoveWindowToWorkspace(0); break;
                case "movecontainertoworkspacenumber2": _desktopManager.MoveWindowToWorkspace(1); break;
                case "movecontainertoworkspacenumber3": _desktopManager.MoveWindowToWorkspace(2); break;
                case "movecontainertoworkspacenumber4": _desktopManager.MoveWindowToWorkspace(3); break;
                case "movecontainertoworkspacenumber5": _desktopManager.MoveWindowToWorkspace(4); break;
                case "movecontainertoworkspacenumber6": _desktopManager.MoveWindowToWorkspace(5); break;
                case "movecontainertoworkspacenumber7": _desktopManager.MoveWindowToWorkspace(6); break;
                case "movecontainertoworkspacenumber8": _desktopManager.MoveWindowToWorkspace(7); break;
                case "movecontainertoworkspacenumber9": _desktopManager.MoveWindowToWorkspace(8); break;
                case "movecontainertoworkspacenumber10": _desktopManager.MoveWindowToWorkspace(9); break;
                case "focusleft": _desktopManager.FocusOnLeftWindow(); break;
                case "focusright": _desktopManager.FocusOnRightWindow(); break;
                case "togglewindowslock": DesktopManager.SwapWindowsLockScreen(); break;
                default: return false;
            };
            return true;
        }
    }
}
