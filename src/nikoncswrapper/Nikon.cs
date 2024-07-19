//
// This work is licensed under a Creative Commons Attribution 3.0 Unported License.
//
// Thomas Dideriksen (thomas@dideriksen.com)
//

namespace Nikon;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;



public abstract class NikonBase {
    private readonly Dictionary<eNkMAIDCapability, NkMAIDCapInfo> _caps;

    internal NikonBase(NikonMd3 md3, NikonScheduler scheduler) {
        this.Md3 = md3;
        this.Scheduler = scheduler;
        this.ModuleType = NikonModuleType.Unknown;
        _caps = [];
    }

    internal NikonMd3 Md3 { get; }

    internal NikonScheduler Scheduler { get; }

    internal NikonObject Object { get; private set; }

    internal NikonModuleType ModuleType { get; set; }

    internal void InitializeObject(NikonObject obj) {
        this.Object = obj;
        this.Object.Open();
        this.Object.Event += this._object_Event;

        this.RefreshCaps();
    }

    private void _object_Event(NikonObject sender, IntPtr refClient, eNkMAIDEvent ulEvent, IntPtr data) {
        if (ulEvent == eNkMAIDEvent.kNkMAIDEvent_CapChange) {
            this.RefreshCaps();
        }

        this.HandleEvent(sender, ulEvent, data);
    }

    internal virtual void HandleEvent(NikonObject obj, eNkMAIDEvent currentEvent, IntPtr data) {
        // Note: Overridden in inheritors
    }

    private void RefreshCaps() {
        Debug.Assert(this.Scheduler.WorkerThreadId == Thread.CurrentThread.ManagedThreadId);

        var caps = this.Object.GetCapInfo();

        lock (_caps) {
            _caps.Clear();

            foreach (var cap in caps) {
                _caps[cap.ulID] = cap;
            }
        }
    }

    public uint Id => this.Object.Id;

    public string Name => this.GetString(eNkMAIDCapability.kNkMAIDCapability_Name);

    public bool SupportsCapability(eNkMAIDCapability capability) {
        lock (_caps) {
            return _caps.ContainsKey(capability);
        }
    }

    public NkMAIDCapInfo GetCapabilityInfo(eNkMAIDCapability cap) {
        lock (_caps) {
            return _caps[cap];
        }
    }

    public NkMAIDCapInfo[] GetCapabilityInfo() {
        NkMAIDCapInfo[] result;

        lock (_caps) {
            result = _caps.Values.ToArray();
        }

        return result;
    }

    internal eNkMAIDCapType GetCapabilityType(eNkMAIDCapability cap) {
        NkMAIDCapInfo info;
        var found = false;

        lock (_caps) {
            found = _caps.TryGetValue(cap, out info);
        }

        return !found ? throw new NikonException("Capability (" + cap.ToString() + ") is not supported") : info.ulType;
    }

    #region Get (Type Wrappers)
    public string GetString(eNkMAIDCapability cap) => this.Get(cap) as string;

    public uint GetUnsigned(eNkMAIDCapability cap) => (uint)this.Get(cap);

    public int GetInteger(eNkMAIDCapability cap) => (int)this.Get(cap);

    public bool GetBoolean(eNkMAIDCapability cap) => (bool)this.Get(cap);

    public double GetFloat(eNkMAIDCapability cap) => (double)this.Get(cap);

    public NikonEnum GetEnum(eNkMAIDCapability cap) => this.Get(cap) as NikonEnum;

    public NikonArray GetArray(eNkMAIDCapability cap) => this.Get(cap) as NikonArray;

    public NikonRange GetRange(eNkMAIDCapability cap) => this.Get(cap) as NikonRange;

    public DateTime GetDateTime(eNkMAIDCapability cap) => (DateTime)this.Get(cap);

    public NkMAIDPoint GetPoint(eNkMAIDCapability cap) => (NkMAIDPoint)this.Get(cap);

    public NkMAIDRect GetRect(eNkMAIDCapability cap) => (NkMAIDRect)this.Get(cap);

    public NkMAIDSize GetSize(eNkMAIDCapability cap) => (NkMAIDSize)this.Get(cap);

    public void GetGeneric(eNkMAIDCapability cap, IntPtr destination) => this.Scheduler.Invoke(() => { this.Object.GetGeneric(cap, destination); });

    public void GetArrayGeneric(eNkMAIDCapability cap, IntPtr destination) => this.Scheduler.Invoke(() => { this.Object.GetArrayGeneric(cap, destination); });
    #endregion

    #region Set (Type Wrappers)
    public void SetString(eNkMAIDCapability cap, string value) => this.Set(cap, value);

    public void SetUnsigned(eNkMAIDCapability cap, uint value) => this.Set(cap, value);

    public void SetInteger(eNkMAIDCapability cap, int value) => this.Set(cap, value);

    public void SetBoolean(eNkMAIDCapability cap, bool value) => this.Set(cap, value);

    public void SetFloat(eNkMAIDCapability cap, double value) => this.Set(cap, value);

    public void SetEnum(eNkMAIDCapability cap, NikonEnum value) => this.Set(cap, value);

    public void SetArray(eNkMAIDCapability cap, NikonArray value) => this.Set(cap, value);

    public void SetRange(eNkMAIDCapability cap, NikonRange value) => this.Set(cap, value);

    public void SetDateTime(eNkMAIDCapability cap, DateTime value) => this.Set(cap, value);

    public void SetPoint(eNkMAIDCapability cap, NkMAIDPoint value) => this.Set(cap, value);

    public void SetRect(eNkMAIDCapability cap, NkMAIDRect value) => this.Set(cap, value);

    public void SetSize(eNkMAIDCapability cap, NkMAIDSize value) => this.Set(cap, value);

    public void SetGeneric(eNkMAIDCapability cap, IntPtr source) => this.Scheduler.Invoke(() => { this.Object.SetGeneric(cap, source); });
    #endregion

    #region Get Default (Type Wrappers)
    public string GetDefaultString(eNkMAIDCapability cap) => this.GetDefault(cap) as string;

    public uint GetDefaultUnsigned(eNkMAIDCapability cap) => (uint)this.GetDefault(cap);

    public int GetDefaultInteger(eNkMAIDCapability cap) => (int)this.GetDefault(cap);

    public bool GetDefaultBoolean(eNkMAIDCapability cap) => (bool)this.GetDefault(cap);

    public double GetDefaultFloat(eNkMAIDCapability cap) => (double)this.GetDefault(cap);

    public NikonRange GetDefaultRange(eNkMAIDCapability cap) => this.GetDefault(cap) as NikonRange;

    public DateTime GetDefaultDateTime(eNkMAIDCapability cap) => (DateTime)this.GetDefault(cap);

    public NkMAIDPoint GetDefaultPoint(eNkMAIDCapability cap) => (NkMAIDPoint)this.GetDefault(cap);

    public NkMAIDRect GetDefaultRect(eNkMAIDCapability cap) => (NkMAIDRect)this.GetDefault(cap);

    public NkMAIDSize GetDefaultSize(eNkMAIDCapability cap) => (NkMAIDSize)this.GetDefault(cap);

    public void GetDefaultGeneric(eNkMAIDCapability cap, IntPtr destination) => this.Scheduler.Invoke(() => { this.Object.GetDefaultGeneric(cap, destination); });
    #endregion

    #region Get
    public object Get(eNkMAIDCapability cap) {
        switch (this.GetCapabilityType(cap)) {
            case eNkMAIDCapType.kNkMAIDCapType_String:
                return this.Scheduler.Invoke(new GetStringDelegate(this.Object.GetString), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Unsigned:
                return this.Scheduler.Invoke(new GetUnsignedDelegate(this.Object.GetUnsigned), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Integer:
                return this.Scheduler.Invoke(new GetIntegerDelegate(this.Object.GetInteger), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Boolean:
                return this.Scheduler.Invoke(new GetBooleanDelegate(this.Object.GetBoolean), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Float:
                return this.Scheduler.Invoke(new GetFloatDelegate(this.Object.GetFloat), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Enum: {
                var result = (NikonEnumWithData)this.Scheduler.Invoke(new GetEnumWithDataDelegate(this.Object.GetEnumWithData), cap);
                return new NikonEnum(result.nativeEnum, result.buffer);
            }

            case eNkMAIDCapType.kNkMAIDCapType_Array: {
                var result = (NikonArrayWithData)this.Scheduler.Invoke(new GetArrayWithDataDelegate(this.Object.GetArrayWithData), cap);
                return new NikonArray(result.nativeArray, result.buffer);
            }

            case eNkMAIDCapType.kNkMAIDCapType_Range: {
                var result = (NkMAIDRange)this.Scheduler.Invoke(new GetRangeDelegate(this.Object.GetRange), cap);
                return new NikonRange(result);
            }

            case eNkMAIDCapType.kNkMAIDCapType_DateTime:
                return this.Scheduler.Invoke(new GetDateTimeDelegate(this.Object.GetDateTime), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Point:
                return this.Scheduler.Invoke(new GetPointDelegate(this.Object.GetPoint), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Rect:
                return this.Scheduler.Invoke(new GetRectDelegate(this.Object.GetRect), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Size:
                return this.Scheduler.Invoke(new GetSizeDelegate(this.Object.GetSize), cap);

            default:
                return null;
        }
    }
    #endregion

    #region Set
    public void Set(eNkMAIDCapability cap, object value) {
        switch (this.GetCapabilityType(cap)) {
            case eNkMAIDCapType.kNkMAIDCapType_String:
                _ = this.Scheduler.Invoke(new SetStringDelegate(this.Object.SetString), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Unsigned:
                _ = this.Scheduler.Invoke(new SetUnsignedDelegate(this.Object.SetUnsigned), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Integer:
                _ = this.Scheduler.Invoke(new SetIntegerDelegate(this.Object.SetInteger), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Boolean:
                _ = this.Scheduler.Invoke(new SetBooleanDelegate(this.Object.SetBoolean), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Float:
                _ = this.Scheduler.Invoke(new SetFloatDelegate(this.Object.SetFloat), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Enum:
                _ = this.Scheduler.Invoke(new SetEnumDelegate(this.Object.SetEnum), cap, (value as NikonEnum).Enum);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Array:
                _ = this.Scheduler.Invoke(new SetArrayDelegate(this.Object.SetArray), cap, (value as NikonArray).Array);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Range:
                _ = this.Scheduler.Invoke(new SetRangeDelegate(this.Object.SetRange), cap, (value as NikonRange).Range);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_DateTime:
                _ = this.Scheduler.Invoke(new SetDateTimeDelegate(this.Object.SetDateTime), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Point:
                _ = this.Scheduler.Invoke(new SetPointDelegate(this.Object.SetPoint), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Rect:
                _ = this.Scheduler.Invoke(new SetUnsignedDelegate(this.Object.SetUnsigned), cap, value);
                break;

            case eNkMAIDCapType.kNkMAIDCapType_Size:
                _ = this.Scheduler.Invoke(new SetUnsignedDelegate(this.Object.SetUnsigned), cap, value);
                break;
        }
    }
    #endregion

    #region Get Default
    public object GetDefault(eNkMAIDCapability cap) {
        switch (this.GetCapabilityType(cap)) {
            case eNkMAIDCapType.kNkMAIDCapType_String:
                return this.Scheduler.Invoke(new GetStringDelegate(this.Object.GetDefaultString), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Unsigned:
                return this.Scheduler.Invoke(new GetUnsignedDelegate(this.Object.GetDefaultUnsigned), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Integer:
                return this.Scheduler.Invoke(new GetIntegerDelegate(this.Object.GetDefaultInteger), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Boolean:
                return this.Scheduler.Invoke(new GetBooleanDelegate(this.Object.GetDefaultBoolean), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Float:
                return this.Scheduler.Invoke(new GetFloatDelegate(this.Object.GetDefaultFloat), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Range: {
                var result = (NkMAIDRange)this.Scheduler.Invoke(new GetRangeDelegate(this.Object.GetDefaultRange), cap);
                return new NikonRange(result);
            }

            case eNkMAIDCapType.kNkMAIDCapType_DateTime:
                return this.Scheduler.Invoke(new GetDateTimeDelegate(this.Object.GetDefaultDateTime), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Point:
                return this.Scheduler.Invoke(new GetPointDelegate(this.Object.GetDefaultPoint), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Rect:
                return this.Scheduler.Invoke(new GetRectDelegate(this.Object.GetDefaultRect), cap);

            case eNkMAIDCapType.kNkMAIDCapType_Size:
                return this.Scheduler.Invoke(new GetSizeDelegate(this.Object.GetDefaultSize), cap);

            // Note: 'Array' and 'Enum' are not here

            default:
                return null;
        }
    }
    #endregion

    public void Start(eNkMAIDCapability cap) => this.Start(cap, eNkMAIDDataType.kNkMAIDDataType_Null, IntPtr.Zero);

    public void Start(eNkMAIDCapability cap, eNkMAIDDataType dataType, IntPtr data) => this.Scheduler.Invoke(new StartCapabilityDelegate(this.Object.CapStart), cap, dataType, data);
}

public class NikonManager : NikonBase {
    private Dictionary<uint, NikonDevice> _devices;
    private const string _defaultMd3EntryPoint = "MAIDEntryPoint";

    private event DeviceAddedDelegate _deviceAdded;
    private event DeviceRemovedDelegate _deviceRemoved;

    // Note: Add and remove event handlers on the thread where they are fired

    public event DeviceAddedDelegate DeviceAdded
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _deviceAdded += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _deviceAdded -= value; });
    }

    public event DeviceRemovedDelegate DeviceRemoved
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _deviceRemoved += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _deviceRemoved -= value; });
    }

    public NikonManager(string md3File)
        : this(md3File, _defaultMd3EntryPoint, SynchronizationContext.Current) {
    }

    public NikonManager(string md3File, string md3EntryPoint)
        : this(md3File, md3EntryPoint, SynchronizationContext.Current) {
    }

    public NikonManager(string md3File, SynchronizationContext context)
        : this(md3File, _defaultMd3EntryPoint, context) {
    }

    public NikonManager(string md3File, string md3EntryPoint, SynchronizationContext context)
        : base(new NikonMd3(md3File, md3EntryPoint), new NikonScheduler(context)) {
        _devices = [];

        _ = this.Scheduler.Invoke(() => {
            this.InitializeObject(new NikonObject(this.Md3, null, 0));

            var moduleName = this.Object.GetString(eNkMAIDCapability.kNkMAIDCapability_Name).Split(' ');

            this.ModuleType = moduleName.Length > 0 && Enum.TryParse(moduleName[0], out
            NikonModuleType type) ? type : NikonModuleType.Unknown;

            var asyncRate = (double)this.Object.GetUnsigned(eNkMAIDCapability.kNkMAIDCapability_AsyncRate);

            this.Scheduler.SchedulePeriodicTask(() => {
                this.Async();
            },
            asyncRate);
        });
    }

    private void Async() {
        Debug.Assert(this.Scheduler.WorkerThreadId == Thread.CurrentThread.ManagedThreadId);

        try {
            this.Object.Async();
        }
        catch (NikonException ex) {
            // Note: Allow 'CameraNotFound' - this is thrown from Async when the camera is removed
            if (ex.ErrorCode != eNkMAIDResult.kNkMAIDResult_CameraNotFound) {
                throw;
            }
        }
    }

    internal override void HandleEvent(NikonObject obj, eNkMAIDEvent currentEvent, IntPtr data) {
        switch (currentEvent) {
            case eNkMAIDEvent.kNkMAIDEvent_AddChild:
                this.HandleAddChild(data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_RemoveChild:
                this.HandleRemoveChild(data);
                break;
        }
    }

    private void HandleAddChild(IntPtr data) {
        var id = (uint)data.ToInt32();

        var device = new NikonDevice(this.Md3, this.Scheduler, this.Object, this.ModuleType, id);

        lock (_devices) {
            Debug.Assert(!_devices.ContainsKey(id));
            _devices[id] = device;
        }

        this.Scheduler.Callback(new DeviceAddedDelegate(this.OnDeviceAdded), this, device);
    }

    private void HandleRemoveChild(IntPtr data) {
        var id = (uint)data.ToInt32();

        NikonDevice device = null;

        lock (_devices) {
            Debug.Assert(_devices.ContainsKey(id));
            device = _devices[id];
            _ = _devices.Remove(id);
        }

        device.Object.Close();

        this.Scheduler.Callback(new DeviceRemovedDelegate(this.OnDeviceRemoved), this, device);
    }

    private void OnDeviceAdded(NikonManager sender, NikonDevice device) => _deviceAdded?.Invoke(sender, device);

    private void OnDeviceRemoved(NikonManager sender, NikonDevice device) => _deviceRemoved?.Invoke(sender, device);

    public int DeviceCount {
        get {
            var count = 0;
            lock (_devices) {
                count = _devices.Count;
            }
            return count;
        }
    }

    public NikonDevice GetDeviceByIndex(uint index) {
        NikonDevice device = null;

        lock (_devices) {
            var i = 0;
            foreach (var d in _devices.Values) {
                if (i == index) {
                    device = d;
                    break;
                }

                i++;
            }
        }

        return device;
    }

    public NikonDevice GetDeviceById(uint id) {
        NikonDevice device = null;

        lock (_devices) {
            if (!_devices.TryGetValue(id, out device)) {
                device = null;
            }
        }

        return device;
    }

    public void Shutdown() {
        this.Scheduler.Shutdown();

        lock (_devices) {
            foreach (var device in _devices.Values) {
                try {
                    device.Object.Close();
                }
                catch (AccessViolationException ex) {
                    Console.WriteLine("Shutdown_Error: {0}", ex.Message);
                }
            }

            _devices = null;
        }

        this.Object.Close();

        this.Md3.Close();
    }
}

public class NikonDevice : NikonBase {
    private NikonImage _currentImage;
    private uint _currentItemId;
    private int _bulbCaptureShutterSpeedBackup;

    private event PreviewReadyDelegate _previewReady;
    private event PreviewReadyDelegate _lowResolutionPreviewReady;
    private event ThumbnailReadyDelegate _thumbnailReady;
    private event ImageReadyDelegate _imageReady;
    private event CaptureCompleteDelegate _captureComplete;
    private event CapabilityChangedDelegate _capabilityChanged;
    private event CapabilityChangedDelegate _capabilityValueChanged;
    private event VideoFragmentReadyDelegate _videoFragmentReady;
    private event VideoRecordingInterruptedDelegate _videoRecordingInterrupted;
    private event ProgressDelegate _progress;

    public int DeviceID { set; get; } = 0;

    // Note: Add and remove event handlers on the thread where they are fired

    public event PreviewReadyDelegate PreviewReady
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _previewReady += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _previewReady -= value; });
    }

    public event PreviewReadyDelegate LowResolutionPreviewReady
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _lowResolutionPreviewReady += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _lowResolutionPreviewReady -= value; });
    }

    public event ThumbnailReadyDelegate ThumbnailReady
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _thumbnailReady += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _thumbnailReady -= value; });
    }

    public event ImageReadyDelegate ImageReady
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _imageReady += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _imageReady -= value; });
    }

    public event CaptureCompleteDelegate CaptureComplete
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _captureComplete += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _captureComplete -= value; });
    }

    public event CapabilityChangedDelegate CapabilityChanged
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _capabilityChanged += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _capabilityChanged -= value; });
    }

    public event CapabilityChangedDelegate CapabilityValueChanged
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _capabilityValueChanged += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _capabilityValueChanged -= value; });
    }

    public event VideoFragmentReadyDelegate VideoFragmentReady
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _videoFragmentReady += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _videoFragmentReady -= value; });
    }

    public event VideoRecordingInterruptedDelegate VideoRecordingInterrupted
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _videoRecordingInterrupted += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _videoRecordingInterrupted -= value; });
    }

    public event ProgressDelegate Progress
    {
        add => this.Scheduler.AddOrRemoveEvent(() => { _progress += value; });
        remove => this.Scheduler.AddOrRemoveEvent(() => { _progress -= value; });
    }

    internal NikonDevice(NikonMd3 md3, NikonScheduler scheduler, NikonObject parent, NikonModuleType moduleType, uint deviceId)
        : base(md3, scheduler) {
        Debug.Assert(this.Scheduler.WorkerThreadId == Thread.CurrentThread.ManagedThreadId);

        this.ModuleType = moduleType;

        var source = new NikonObject(md3, parent, deviceId);
        this.InitializeObject(source);
    }

    internal override void HandleEvent(NikonObject obj, eNkMAIDEvent currentEvent, IntPtr data) {
        switch (currentEvent) {
            case eNkMAIDEvent.kNkMAIDEvent_AddChild:
            case eNkMAIDEvent.kNkMAIDEvent_AddChildInCard:
                this.HandleAddChild(data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_AddPreviewImage:
                this.HandleAddPreviewImage(data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_CaptureComplete:
                this.Scheduler.Callback(new CaptureCompleteDelegate(this.OnCaptureComplete), this, (int)data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_CapChange:
                this.Scheduler.Callback(new CapabilityChangedDelegate(this.OnCapabilityChanged), this, (eNkMAIDCapability)data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_CapChangeValueOnly:
                this.Scheduler.Callback(new CapabilityChangedDelegate(this.OnCapabilityValueChanged), this, (eNkMAIDCapability)data);
                break;

            case eNkMAIDEvent.kNkMAIDEvent_RecordingInterrupted:
                this.Scheduler.Callback(new VideoRecordingInterruptedDelegate(this.OnVideoRecordingInterrupted), this, (int)data);
                break;

            default:
                Debug.Print("Unhandled event: " + currentEvent + " (" + data.ToString() + ")");
                break;
        }
    }

    private void HandleAddPreviewImage(IntPtr data) {
        try {
            // Note:
            // The two event checks below are not thread safe, since
            // events are hooked up on the callback thread (and we're
            // currently on the worker thread). So there is a minor
            // race condition here. We choose to live with it for efficency
            // purposes.

            var doPreview = this.SupportsCapability(eNkMAIDCapability.kNkMAIDCapability_GetPreviewImageNormal) &&
                _previewReady != null;

            var doLowResPreview = this.SupportsCapability(eNkMAIDCapability.kNkMAIDCapability_GetPreviewImageLow) &&
                _lowResolutionPreviewReady != null;

            if (doPreview || doLowResPreview) {
                this.Object.SetUnsigned(eNkMAIDCapability.kNkMAIDCapability_CurrentPreviewID, (uint)data.ToInt32());
            }

            if (doPreview) {
                this.GetPreviewAndFireEvent(
                    eNkMAIDCapability.kNkMAIDCapability_GetPreviewImageNormal,
                    new PreviewReadyDelegate(this.OnPreviewReady));
            }

            if (doLowResPreview) {
                this.GetPreviewAndFireEvent(
                    eNkMAIDCapability.kNkMAIDCapability_GetPreviewImageLow,
                    new PreviewReadyDelegate(this.OnLowResolutionPreviewReady));
            }
        }
        catch (NikonException ex) {
            Debug.Print("Failed to retrieve preview image (" + ex.ToString() + ")");

            // TODO: BUG(?): Why do we sometimes get 'ValueOutOfBounds' when retrieving the preview images?
            if (ex.ErrorCode != eNkMAIDResult.kNkMAIDResult_ValueOutOfBounds) {
                throw;
            }
        }
    }

    private void DataItemAcquire(NikonObject data) {
        // Listen for progress
        data.Progress += this.data_Progress;

        // Listen for data
        data.DataFile += this.data_DataFile;
        data.DataImage += this.data_DataImage;
        data.DataSound += this.data_DataSound;

        // Try to acquire the data object
        try {
            data.CapStart(
                eNkMAIDCapability.kNkMAIDCapability_Acquire,
                eNkMAIDDataType.kNkMAIDDataType_Null,
                IntPtr.Zero);
        }
        catch (NikonException ex) {
            // Is this a 'NotSupported' exception?
            var isNotSupported = ex.ErrorCode == eNkMAIDResult.kNkMAIDResult_NotSupported;

            // Is this a 'Thumbnail' data object?
            var isThumbnail = data.Id == (uint)eNkMAIDDataObjType.kNkMAIDDataObjType_Thumbnail;

            // According to the documentation, acquiring a thumbnail data object
            // sometimes produces a NotSupported error. Apparently this is expected,
            // so we ignore this specific case here.
            var isAllowed = isNotSupported && isThumbnail;

            if (isAllowed) {
                Debug.Print("Failed to retrieve thumbnail image");
            }
            else {
                // If this is some other case, rethrow the exception
                throw;
            }
        }
        finally {
            // Unhook data object events
            data.Progress -= this.data_Progress;
            data.DataFile -= this.data_DataFile;
            data.DataImage -= this.data_DataImage;
            data.DataSound -= this.data_DataSound;
        }
    }

    private unsafe void DataItemGetVideoImage(NikonObject data) {
        var name = data.GetString(eNkMAIDCapability.kNkMAIDCapability_Name);
        var videoDimensions = data.GetSize(eNkMAIDCapability.kNkMAIDCapability_Pixels);

        var videoImage = new NkMAIDGetVideoImage();
        data.GetGeneric(eNkMAIDCapability.kNkMAIDCapability_GetVideoImage, new IntPtr(&videoImage));

        // Note: Download 4MB at the time
        const int chunkSize = 4 * 1024 * 1024;

        var totalSize = videoImage.ulDataSize;

        for (uint offset = 0; offset < totalSize; offset += chunkSize) {
            var fragmentSize = Math.Min(chunkSize, totalSize - offset);

            var buffer = new byte[fragmentSize];

            fixed (byte* pBuffer = buffer) {
                videoImage.ulOffset = offset;
                videoImage.ulReadSize = (uint)buffer.Length;
                videoImage.ulDataSize = (uint)buffer.Length;
                videoImage.pData = new IntPtr(pBuffer);

                data.GetArrayGeneric(eNkMAIDCapability.kNkMAIDCapability_GetVideoImage, new IntPtr(&videoImage));
            }

            var fragment = new NikonVideoFragment(
                name,
                totalSize,
                offset,
                buffer,
                videoDimensions.w,
                videoDimensions.h);

            this.Scheduler.Callback(new VideoFragmentReadyDelegate(this.OnVideoFragmentReady), this, fragment);
        }
    }

    private void HandleAddChild(IntPtr id) {
        var item = new NikonObject(this.Md3, this.Object, (uint)id.ToInt32());

        List<uint> dataIds = [];

        item.Open();
        item.Event += (NikonObject obj, IntPtr refClient, eNkMAIDEvent currentEvent, IntPtr data) => {
            if (currentEvent == eNkMAIDEvent.kNkMAIDEvent_AddChild) {
                dataIds.Add((uint)data.ToInt32());
            }
        };

        item.EnumChildren();

        _currentItemId = item.Id;

        foreach (var dataId in dataIds) {
            var dataObjectType = (eNkMAIDDataObjType)dataId;

            var data = new NikonObject(this.Md3, item, dataId);

            data.Open();

            switch (dataObjectType) {
                case eNkMAIDDataObjType.kNkMAIDDataObjType_Thumbnail:
                case eNkMAIDDataObjType.kNkMAIDDataObjType_File | eNkMAIDDataObjType.kNkMAIDDataObjType_Thumbnail:
                    // Note:
                    // We do a 'thread-unsafe' check of the thumbnail-ready event here. No
                    // need to acquire if the user hasn't hooked up the thumbnail event.
                    if (_thumbnailReady != null) {
                        this.DataItemAcquire(data);
                    }
                    break;

                case eNkMAIDDataObjType.kNkMAIDDataObjType_Image:
                case eNkMAIDDataObjType.kNkMAIDDataObjType_Sound:
                case eNkMAIDDataObjType.kNkMAIDDataObjType_File:
                case eNkMAIDDataObjType.kNkMAIDDataObjType_File | eNkMAIDDataObjType.kNkMAIDDataObjType_Image:
                case eNkMAIDDataObjType.kNkMAIDDataObjType_File | eNkMAIDDataObjType.kNkMAIDDataObjType_Sound:
                    this.DataItemAcquire(data);
                    break;

                case eNkMAIDDataObjType.kNkMAIDDataObjType_Video:
                    // Note:
                    // We do a 'thread-unsafe' check of the videofragment-ready event here. No
                    // need to download videos if the user hasn't hooked up the event.
                    if (_videoFragmentReady != null) {
                        this.DataItemGetVideoImage(data);
                    }
                    break;

                default:
                    Debug.Print("Unknown data object type: " + dataObjectType.ToString());
                    break;
            }

            data.Close();
        }

        item.Close();
    }

    private void GetPreviewAndFireEvent(eNkMAIDCapability previewCapabilty, PreviewReadyDelegate d) {
        var previewArray = this.Object.GetArrayWithData(previewCapabilty);

        var preview = new NikonPreview(previewArray.buffer);

        this.Scheduler.Callback(d, this, preview);
    }

    private void data_Progress(NikonObject sender,
        eNkMAIDCommand ulCommand,
        uint ulParam,
        IntPtr refComplete,
        uint ulDone,
        uint ulTotal) => this.Scheduler.Callback(new ProgressDelegate(this.OnProgress), this, (eNkMAIDDataObjType)sender.Id, (int)ulDone, (int)ulTotal);

    private void data_DataImage(NikonObject sender, NkMAIDImageInfo imageInfo, IntPtr data) {
        var thumbnail = new NikonThumbnail(imageInfo, data);
        this.Scheduler.Callback(new ThumbnailReadyDelegate(this.OnThumbnailReady), this, thumbnail);
    }

    private void data_DataSound(NikonObject sender, NkMAIDSoundInfo soundInfo, IntPtr data) => Debug.Print("DataProcSoundInfo event fired");

    private void data_DataFile(NikonObject sender, NkMAIDFileInfo fileInfo, IntPtr data) {
        if (fileInfo.ulStart == 0) {
            Debug.Assert(_currentImage == null);

            var size = (int)fileInfo.ulTotalLength;
            var type = (NikonImageType)(_currentItemId >> 27);
            var number = (int)((_currentItemId << 8) >> 8);
            var isFragmentOfRawPlusJpeg = (_currentItemId & (1 << 26)) != 0;

            _currentImage = new NikonImage(size, type, number, isFragmentOfRawPlusJpeg);
        }

        Debug.Assert(_currentImage != null);

        var offset = (int)fileInfo.ulStart;
        var length = (int)fileInfo.ulLength;

        _currentImage.CopyFrom(data, offset, length);

        var complete = fileInfo.ulTotalLength == fileInfo.ulStart + fileInfo.ulLength;

        if (complete) {
            var image = _currentImage;
            _currentImage = null;

            this.Scheduler.Callback(new ImageReadyDelegate(this.OnImageReady), this, image);
        }
    }

    private void OnPreviewReady(NikonDevice sender, NikonPreview preview) => _previewReady?.Invoke(sender, preview);

    private void OnLowResolutionPreviewReady(NikonDevice sender, NikonPreview preview) => _lowResolutionPreviewReady?.Invoke(sender, preview);

    private void OnThumbnailReady(NikonDevice sender, NikonThumbnail thumbnail) => _thumbnailReady?.Invoke(sender, thumbnail);

    private void OnImageReady(NikonDevice sender, NikonImage image) => _imageReady?.Invoke(sender, image);

    private void OnCaptureComplete(NikonDevice sender, int data) => _captureComplete?.Invoke(sender, data);

    private void OnCapabilityChanged(NikonDevice sender, eNkMAIDCapability capability) => _capabilityChanged?.Invoke(sender, capability);

    private void OnCapabilityValueChanged(NikonDevice sender, eNkMAIDCapability capability) => _capabilityValueChanged?.Invoke(sender, capability);

    private void OnVideoFragmentReady(NikonDevice sender, NikonVideoFragment fragment) => _videoFragmentReady?.Invoke(sender, fragment);

    private void OnVideoRecordingInterrupted(NikonDevice sender, int error) => _videoRecordingInterrupted?.Invoke(sender, error);

    private void OnProgress(NikonDevice sender, eNkMAIDDataObjType type, int done, int total) => _progress?.Invoke(sender, type, done, total);

    public void StartRecordVideo() => this.SetUnsigned(eNkMAIDCapability.kNkMAIDCapability_MovRecInCardStatus, 1);

    public void StopRecordVideo() => this.SetUnsigned(eNkMAIDCapability.kNkMAIDCapability_MovRecInCardStatus, 0);

    public void Capture() => this.Start(eNkMAIDCapability.kNkMAIDCapability_Capture);

    public bool LiveViewEnabled {
        set => this.SetUnsigned(eNkMAIDCapability.kNkMAIDCapability_LiveViewStatus, value ? 1U : 0U);
        get => this.GetUnsigned(eNkMAIDCapability.kNkMAIDCapability_LiveViewStatus) != 0;
    }

    public NikonLiveViewImage GetLiveViewImage() {
        var a = this.GetArray(eNkMAIDCapability.kNkMAIDCapability_GetLiveViewImage);

        var headerSize = this.ModuleType switch {
            NikonModuleType.Type0001 or NikonModuleType.Type0002 => 64,
            NikonModuleType.Type0003 => 128,
            _ => 384,
        };
        return new NikonLiveViewImage(a.Buffer, headerSize);
    }

    public void StartBulbCapture() {
        // Lock camera
        this.SetBoolean(
            eNkMAIDCapability.kNkMAIDCapability_LockCamera,
            true);

        // Change the exposure mode to 'Manual'
        var exposureMode = this.GetEnum(eNkMAIDCapability.kNkMAIDCapability_ExposureMode);
        var foundManual = false;
        for (var i = 0; i < exposureMode.Length; i++) {
            if ((uint)exposureMode[i] == (uint)eNkMAIDExposureMode.kNkMAIDExposureMode_Manual) {
                exposureMode.Index = i;
                foundManual = true;
                this.SetEnum(eNkMAIDCapability.kNkMAIDCapability_ExposureMode, exposureMode);
                break;
            }
        }

        // Throw exception if the 'Manual' exposure mode wasn't found
        if (!foundManual) {
            throw new NikonException("Failed to find the 'Manual' exposure mode");
        }

        // Change the shutterspeed to 'Bulb'
        var shutterSpeed = this.GetEnum(eNkMAIDCapability.kNkMAIDCapability_ShutterSpeed);
        _bulbCaptureShutterSpeedBackup = shutterSpeed.Index;
        var foundBulb = false;
        for (var i = 0; i < shutterSpeed.Length; i++) {
            if (shutterSpeed[i].ToString().ToLower().Contains("bulb")) {
                shutterSpeed.Index = i;
                foundBulb = true;
                this.SetEnum(eNkMAIDCapability.kNkMAIDCapability_ShutterSpeed, shutterSpeed);
                break;
            }
        }

        // Throw exception if the 'Bulb' shutterspeed wasn't found
        if (!foundBulb) {
            throw new NikonException("Failed to find the 'Bulb' shutter speed");
        }

        // Capture
        try {
            this.Capture();
        }
        catch (NikonException ex) {
            // Ignore 'BulbReleaseBusy' exception - it's expected
            if (ex.ErrorCode != eNkMAIDResult.kNkMAIDResult_BulbReleaseBusy) {
                throw;
            }
        }
    }

    public void StopBulbCapture() {
        // Terminate capture
        var terminate = new NkMAIDTerminateCapture {
            ulParameter1 = 0,
            ulParameter2 = 0
        };

        unsafe {
            var terminatePointer = new IntPtr(&terminate);

            this.Start(
                eNkMAIDCapability.kNkMAIDCapability_TerminateCapture,
                eNkMAIDDataType.kNkMAIDDataType_GenericPtr,
                terminatePointer);
        }

        // Restore original shutter speed
        var shutterSpeed = this.GetEnum(eNkMAIDCapability.kNkMAIDCapability_ShutterSpeed);
        shutterSpeed.Index = _bulbCaptureShutterSpeedBackup;
        this.SetEnum(eNkMAIDCapability.kNkMAIDCapability_ShutterSpeed, shutterSpeed);

        // Unlock camera
        this.SetBoolean(
            eNkMAIDCapability.kNkMAIDCapability_LockCamera,
            false);
    }
}
