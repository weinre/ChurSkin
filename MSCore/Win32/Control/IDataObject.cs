using System.Runtime.InteropServices;

namespace Win32
{
    [ComImport, Guid("0000010E-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     ComVisible(true)]
    public interface IDataObject
    {
        [PreserveSig]
        uint GetData(ref FORMATETC a, ref STGMEDIUM b);

        [PreserveSig]
        uint GetDataHere(ref FORMATETC pFormatetc, out STGMEDIUM pMedium);

        [PreserveSig]
        uint QueryGetData(ref FORMATETC pFormatetc);

        [PreserveSig]
        uint GetCanonicalFormatEtc(ref FORMATETC pformatectIn, out FORMATETC pformatetcOut);

        [PreserveSig]
        uint SetData(ref FORMATETC pFormatectIn, ref STGMEDIUM pmedium,
            [In, MarshalAs(UnmanagedType.Bool)] bool fRelease);

        [PreserveSig]
        uint EnumFormatEtc(uint dwDirection, IEnumFORMATETC penum);

        [PreserveSig]
        uint DAdvise(ref FORMATETC pFormatetc, int advf, [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink,
            out uint pdwConnection);

        [PreserveSig]
        uint imethod_0(uint dwConnection);

        [PreserveSig]
        uint imethod_1([MarshalAs(UnmanagedType.Interface)] out IEnumSTATDATA ppenumAdvise);
    }
}