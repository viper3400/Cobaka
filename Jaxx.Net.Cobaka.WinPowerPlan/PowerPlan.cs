using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Jaxx.Net.Cobaka.WinPowerPlan
{
    public class PowerPlan
    {
        public static void SetActive(Guid powerSchemeId)
        {
            var schemeGuid = powerSchemeId;
            PowrProf.PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid);
        }

        public static IEnumerable<Guid> FindAll()
        {
            var schemeGuid = Guid.Empty;
            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;
            while (PowrProf.PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)PowrProf.AccessFlags.ACCESS_SCHEME, schemeIndex, ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

        public static string ReadFriendlyName(Guid schemeGuid)
        {
            uint sizeName = 1024;
            IntPtr pSizeName = Marshal.AllocHGlobal((int)sizeName);
            string friendlyName;
            try
            {
                PowrProf.PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, pSizeName, ref sizeName);
                friendlyName = Marshal.PtrToStringUni(pSizeName);
            }
            finally
            {
                Marshal.FreeHGlobal(pSizeName);
            }
            return friendlyName;
        }

        public static Guid GetActive()
        {
            IntPtr pCurrentSchemeGuid = IntPtr.Zero;
            PowrProf.PowerGetActiveScheme(IntPtr.Zero, ref pCurrentSchemeGuid);
            var currentSchemeGuid = (Guid)Marshal.PtrToStructure(pCurrentSchemeGuid, typeof(Guid));
            return currentSchemeGuid;
        }
    }
}
