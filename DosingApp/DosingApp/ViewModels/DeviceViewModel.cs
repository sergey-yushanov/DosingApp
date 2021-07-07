using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.ViewModels
{
    public class DeviceViewModel : BaseViewModel
    {
        #region Attributes
        IBluetoothLE bluetoothLE;
        IAdapter adapter;
        ObservableCollection<IDevice> devices;
        #endregion Attributes

        #region Constructor
        public DeviceViewModel()
        {
            bluetoothLE = CrossBluetoothLE.Current;
            adapter = bluetoothLE.Adapter;
            devices = new ObservableCollection<IDevice>();
        }
        #endregion Constructor

    }
}
