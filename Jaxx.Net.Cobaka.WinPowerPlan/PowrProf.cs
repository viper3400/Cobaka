using System;
using System.Runtime.InteropServices;

namespace Jaxx.Net.Cobaka.WinPowerPlan
{
    public static class PowrProf
    {
        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref UInt32 BufferSize);

        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerEnumerate(IntPtr RootPowerKey, IntPtr SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, UInt32 AcessFlags, UInt32 Index, ref Guid Buffer, ref UInt32 BufferSize);

        [DllImport("PowrProf.dll")]
        public static extern uint PowerGetActiveScheme(IntPtr UserRootPowerKey, ref IntPtr ActivePolicyGuid);

        [DllImport("PowrProf.dll")]
        public static extern uint PowerSetActiveScheme(IntPtr UserRootPowerKey, ref Guid SchemeGuid);

        public enum AccessFlags : uint
        {
            ACCESS_SCHEME = 16,
            ACCESS_SUBGROUP = 17,
            ACCESS_INDIVIDUAL_SETTING = 18
        }
    }
}