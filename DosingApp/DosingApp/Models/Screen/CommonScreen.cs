using DosingApp.Models.Modbus;
using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System;

namespace DosingApp.Models.Screen
{
    public class CommonScreen : BaseViewModel
    {
        public virtual ValveAdjustableScreen ValveAdjustable { get; set; }
        public virtual FlowmeterScreen Flowmeter { get; set; }

        public double? CarrierRequiredVolume { get; set; }
        public double? CarrierDosedVolume { get; set; }

        public double CollectorFineK1 { get; set; }
        public double CollectorFineK2 { get; set; }
        public double CollectorFineK3 { get; set; }
        public double CollectorFineSetPoint { get; set; }

        public double VolumeDosFineK4 { get; set; }

        public CommonScreen()
        {
            ValveAdjustable = new ValveAdjustableScreen() { Name = "РегКл" };
            Flowmeter = new FlowmeterScreen();
        }

        public void Update(Common common, bool showSettings)
        {
            ValveAdjustable.Update(common.ValveAdjustable, showSettings);
            Flowmeter.Update(common.Flowmeter, showSettings);

            CarrierDosedVolume = common.Loop.CarrierDosedVolume;
            CarrierRequiredVolume = common.Loop.CarrierRequiredVolume;

            //Console.Write("CarrierDosedVolume: ");
            //Console.WriteLine(CarrierDosedVolume);

            //Console.Write("CarrierRequiredVolume: ");
            //Console.WriteLine(CarrierRequiredVolume);
        }

        public void Update(ushort[] registers)
        {
            float flow = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_FLOW));
            float volume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_VOL));
            float pulsesPerLiter = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            CarrierDosedVolume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_DOSE_VOL));
            CarrierRequiredVolume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_REQ_VOL));

            CollectorFineK1 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K1));
            CollectorFineK2 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K2));
            CollectorFineK3 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K3));
            CollectorFineSetPoint = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_SP));

            VolumeDosFineK4 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_FINE_K4));
        }

        //public void InitNew(Common common, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(common.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(common.Flowmeter, showSettings);
        //}
    }
}
