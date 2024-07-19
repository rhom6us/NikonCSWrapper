//
// This work is licensed under a Creative Commons Attribution 3.0 Unported License.
//
// Thomas Dideriksen (thomas@dideriksen.com)
//

namespace Nikon;

using System;
using System.Runtime.InteropServices;

// Note: This file is auto generated

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDActiveSelectionSelectedPictures {
    public uint ulPicIDNum;
    public IntPtr pulSelPicIDs;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDArray {
    public eNkMAIDArrayType ulType; // one of eNkMAIDArrayType
    public uint ulElements;       // total number of elements
    public uint ulDimSize1;       // size of first dimension
    public uint ulDimSize2;       // size of second dimension, zero for 1 dim
    public uint ulDimSize3;       // size of third dimension, zero for 1 or 2 dim
    public ushort wPhysicalBytes;   // bytes per element
    public ushort wLogicalBits;     // must be <= wPhysicalBytes * 8
    public IntPtr pData;            // allocated by the client
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDCallback {
    public IntPtr pProc;
    public IntPtr refProc;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDCapInfo {
    public eNkMAIDCapability ulID;            // one of eNkMAIDCapability or vendor specified
    public eNkMAIDCapType ulType;             // one of eNkMAIDCapabilityType
    public eNkMAIDCapVisibility ulVisibility; // eNkMAIDCapVisibility bits
    public eNkMAIDCapOperation ulOperations;  // eNkMAIDCapOperations bits
    public fixed byte szDescription[256];     // text describing the capability
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDCMLProfile {
    public eNkMAIDColorSpace ulColorSpace;             // one of eNkMAIDColorSpace
    public uint ulBits;                              // Bit depth of the supported image by this profile.
    public fixed uint ulReserved[5];
    public NkMAIDString file;                          // DOS path and filename
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDDataInfo {
    public eNkMAIDDataObjType ulType; // one of eNkMAIDDataObjType
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDDateCounterData {
    public fixed ushort wcDate1[9]; // First date
    public fixed ushort wcDate2[9]; // Second date
    public fixed ushort wcDate3[9]; // Third date
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDDateTime {
    public ushort nYear;      // ie 1997, 1998
    public ushort nMonth;     // 0-11 = Jan-Dec
    public ushort nDay;       // 1-31
    public ushort nHour;      // 0-23
    public ushort nMinute;    // 0-59
    public ushort nSecond;    // 0-59
    public uint nSubsecond; // Module dependent
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDEnum {
    public eNkMAIDArrayType ulType; // one of eNkMAIDArrayType
    public uint ulElements;       // total number of elements
    public uint ulValue;          // current index
    public uint ulDefault;        // default index
    public short wPhysicalBytes;    // bytes per element
    public IntPtr pData;            // allocated by the client
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDEventParam {
    public eNkMAIDEvent ulEvent;    // one of eNkMAIDEvent
    public uint ulElements;       // total number of valid params
    public fixed uint ulParam[2]; // event parameter
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDFileInfo {
    public NkMAIDDataInfo baseInfo;
    public eNkMAIDFileDataType ulFileDataType; // One of eNkMAIDFileDataTypes
    public uint ulTotalLength;               // total number of bytes to be transferred
    public uint ulStart;                     // index of starting byte (0-based)
    public uint ulLength;                    // number of bytes in this delivery
    public uint fDiskFile;                   // TRUE if the file is delivered on disk
    public uint fRemoveObject;               // TRUE if the object should be removed
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDGetPicCtrlInfo {
    public uint ulPicCtrlItem; // picture control item
    public uint ulSize;        // the data sizer of pData
    public IntPtr pData;         // The pointer to Quick Adjust Param
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDGetSBAttrDesc {
    public uint ulSBHandle;
    public uint ulSBAttrID;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDGetSBGroupAttrDesc {
    public uint ulSBGroupID;
    public uint ulSBGroupAttrID;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDGetSBHandles {
    public uint ulSBGroupID;
    public uint ulNumber;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDGetVideoImage {
    public eNkMAIDArrayType ulType; // one of eNkMAIDArrayType
    public uint ulOffset;         // Offset
    public uint ulReadSize;       // size of get data
    public uint ulDataSize;       // size of "pData"
    public IntPtr pData;            // allocated by the client
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDImageInfo {
    public NkMAIDDataInfo baseInfo;
    public NkMAIDSize szTotalPixels;       // total size of image to be transfered
    public eNkMAIDColorSpace ulColorSpace; // One of eNkMAIDColorSpace
    public NkMAIDRect rData;               // Coords of data, (0,0) = top,left
    public uint ulRowBytes;              // number of bytes per row of pixels
    public fixed ushort wBits[4];          // number of bits per plane per pixel
    public ushort wPlane;                  // Plane of the image being delivered
    public uint fRemoveObject;           // TRUE if the object should be removed
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDIPTCPresetInfo {
    public uint ulPresetNo;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDLensTypeNikon1 {
    public uint ulLensType1;
    public uint ulLensType2;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDObject {
    public eNkMAIDObjectType ulType; // One of eNkMAIDObjectType
    public uint ulID;
    public IntPtr refClient;
    public IntPtr refModule;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDPicCtrlData {
    public uint ulPicCtrlItem; // picture control item
    public uint ulSize;        // the data sizer of pData
    public uint bModifiedFlag; // Flag to set New or current
    public IntPtr pData;         // The pointer to picture control data
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDPoint {
    public int x;
    public int y;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDRange {
    public double lfValue;
    public double lfDefault;
    public uint ulValueIndex;
    public uint ulDefaultIndex;
    public double lfLower;
    public double lfUpper;
    public uint ulSteps;        // zero for infinite range, otherwise must be >= 2
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDRect {
    public int x;  // left coordinate
    public int y;  // top coordinate
    public uint w; // width
    public uint h; // height
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDSBAttrValue {
    public uint ulSBHandle;
    public uint ulSBAttrID;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDSBGroupAttrValue {
    public uint ulSBGroupID;
    public uint ulSBGroupAttrID;
    public uint ulSize;
    public IntPtr pData;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDSize {
    public uint w;
    public uint h;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDSoundInfo {
    public NkMAIDDataInfo baseInfo;
    public uint ulTotalSamples;   // number of full samples to be transferred
    public uint fStereo;          // TRUE if stereo, FALSE if mono
    public uint ulStart;          // index of starting sample of data
    public uint ulLength;         // number of samples of data
    public ushort wBits;            // number of bits per channel
    public ushort wChannel;         // 0 = mono or L+R; 1,2 = left, right
    public uint fRemoveObject;    // TRUE if the object should be removed
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDString {
    public fixed byte str[256]; // allows a 255 character null terminated string
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDTerminateCapture {
    public uint ulParameter1; // Recognize client
    public uint ulParameter2; // The shooting time specified 100msec unit
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDTestFlash {
    public uint ulType;
    public uint ulParam;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDUIRequestInfo {
    public eNkMAIDUIRequestType ulType;      // one of eNkMAIDUIReqestType
    public eNkMAIDUIRequestResult ulDefault; // default return value � one of eNkMAIDUIRequestResult
    public uint fSync;                     // TRUE if user must respond before returning
    public IntPtr lpPrompt;                  // NULL terminated text to show to user
    public IntPtr lpDetail;                  // NULL terminated text indicating more detail
    public IntPtr pObject;                   // module, source, item, or data object
    public IntPtr data;                      // Pointer to an NkMAIDArray structure
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public unsafe struct NkMAIDWBPresetData {
    public uint ulPresetNumber;    // Preset Number
    public uint ulPresetGain;      // Preset Gain
    public uint ulThumbnailSize;   // Thumbnail size of pThumbnailData
    public uint ulThumbnailRotate; // add for D70 One of eNkMAIDThumbnailRotate
    public IntPtr pThumbnailData;    // The pointer to Thumbnail Data
}
