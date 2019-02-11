using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Gekko.Libraries
{
    public static class PlaySounds
    {
        private enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_MEMORY = 0x0004,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_ALIAS = 0x00010000,
            SND_ALIAS_ID = 0x00110000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004,
            SND_PURGE = 0x0040,
            SND_APPLICATION = 0x0080
        }
        [DllImport("winmm.dll",CharSet=CharSet.Auto)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, PlaySoundFlags fdwSound);


        /// <summary>
        /// サウンドを再生する。ファイルパスが入った場合は自動で切り替わる。
        /// </summary>
        /// <param name="command"></param>
        public static void Play(string command)
        {
            if (Regex.IsMatch(command, "[^+a-z]:"))
            {
                PlaySound(command, IntPtr.Zero, PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_ASYNC);
            }
            else
            {
                PlaySound(command, IntPtr.Zero, PlaySoundFlags.SND_ALIAS | PlaySoundFlags.SND_NODEFAULT | PlaySoundFlags.SND_ASYNC);
            }
        }
    
    }
}
