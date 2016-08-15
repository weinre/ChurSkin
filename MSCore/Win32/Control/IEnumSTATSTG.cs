using System.Runtime.InteropServices;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace Win32
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000d-0000-0000-C000-000000000046")]
    public interface IEnumSTATSTG
    {
        [PreserveSig]
        uint Next(uint celt, [Out, MarshalAs(UnmanagedType.LPArray)] STATSTG[] rgelt, out uint pceltFetched);

        void Skip(uint celt);
        void Reset();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG Clone();
    }
}