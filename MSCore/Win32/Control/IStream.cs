using System.Runtime.InteropServices;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace Win32
{
    [ComImport, Guid("0000000c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStream : ISequentialStream
    {
        int Seek(ulong dlibMove, uint dwOrigin, out ulong plibNewPosition);
        int SetSize(ulong libNewSize);
        int CopyTo([In] IStream pstm, ulong cb, out ulong pcbRead, out ulong pcbWritten);
        int Commit(uint grfCommitFlags);
        int Revert();
        int LockRegion(ulong libOffset, ulong cb, uint dwLockType);
        int UnlockRegion(ulong libOffset, ulong cb, uint dwLockType);
        int Stat(out STATSTG pstatstg, uint grfStatFlag);
        int Clone(out IStream ppstm);
    }
}