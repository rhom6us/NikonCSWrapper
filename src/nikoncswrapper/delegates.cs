namespace Nikon;
using System;

#region Public Delegates
public delegate void DeviceAddedDelegate(NikonManager sender, NikonDevice device);
public delegate void DeviceRemovedDelegate(NikonManager sender, NikonDevice device);
public delegate void PreviewReadyDelegate(NikonDevice sender, NikonPreview preview);
public delegate void ThumbnailReadyDelegate(NikonDevice sender, NikonThumbnail thumbnail);
public delegate void ImageReadyDelegate(NikonDevice sender, NikonImage image);
public delegate void CaptureCompleteDelegate(NikonDevice sender, int data);
public delegate void CapabilityChangedDelegate(NikonDevice sender, eNkMAIDCapability capability);
public delegate void VideoFragmentReadyDelegate(NikonDevice sender, NikonVideoFragment fragment);
public delegate void VideoRecordingInterruptedDelegate(NikonDevice sender, int error);
public delegate void ProgressDelegate(NikonDevice sender, eNkMAIDDataObjType type, int done, int total);
#endregion

#region Internal Delegates
internal delegate bool GetBooleanDelegate(eNkMAIDCapability cap);
internal delegate int GetIntegerDelegate(eNkMAIDCapability cap);
internal delegate uint GetUnsignedDelegate(eNkMAIDCapability cap);
internal delegate double GetFloatDelegate(eNkMAIDCapability cap);
internal delegate string GetStringDelegate(eNkMAIDCapability cap);
internal delegate NkMAIDRange GetRangeDelegate(eNkMAIDCapability cap);
internal delegate DateTime GetDateTimeDelegate(eNkMAIDCapability cap);
internal delegate NkMAIDPoint GetPointDelegate(eNkMAIDCapability cap);
internal delegate NkMAIDRect GetRectDelegate(eNkMAIDCapability cap);
internal delegate NkMAIDSize GetSizeDelegate(eNkMAIDCapability cap);
internal delegate NikonEnumWithData GetEnumWithDataDelegate(eNkMAIDCapability cap);
internal delegate NikonArrayWithData GetArrayWithDataDelegate(eNkMAIDCapability capability);

internal delegate void SetBooleanDelegate(eNkMAIDCapability cap, bool value);
internal delegate void SetIntegerDelegate(eNkMAIDCapability cap, int value);
internal delegate void SetUnsignedDelegate(eNkMAIDCapability cap, uint value);
internal delegate void SetFloatDelegate(eNkMAIDCapability cap, double value);
internal delegate void SetStringDelegate(eNkMAIDCapability cap, string value);
internal delegate void SetEnumDelegate(eNkMAIDCapability cap, NkMAIDEnum value);
internal delegate void SetArrayDelegate(eNkMAIDCapability cap, NkMAIDArray value);
internal delegate void SetRangeDelegate(eNkMAIDCapability cap, NkMAIDRange value);
internal delegate void SetDateTimeDelegate(eNkMAIDCapability cap, DateTime value);
internal delegate void SetPointDelegate(eNkMAIDCapability cap, NkMAIDPoint value);
internal delegate void SetRectDelegate(eNkMAIDCapability cap, NkMAIDRect value);
internal delegate void SetSizeDelegate(eNkMAIDCapability cap, NkMAIDSize value);

internal delegate void StartCapabilityDelegate(eNkMAIDCapability cap, eNkMAIDDataType type, IntPtr data);
#endregion