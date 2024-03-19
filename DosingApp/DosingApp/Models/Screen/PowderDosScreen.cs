using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using DosingApp.Models.Modbus;

namespace DosingApp.Models.Screen
{
    public class PowderDosScreen : BaseViewModel
    {
        public int Number { get; set; }
        public ValveScreen Valve { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        
        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }

        public PowderDosScreen(int number)
        {
            this.Number = number;
            Valve = new ValveScreen() { Name = this.Number.ToString() + DispenserSuffix.Powder };
            Flowmeter = new FlowmeterScreen();

            RequiredVolume = 0;
            DosedVolume = 0;
        }

        public string Name { get { return "Порошковый дозатор " + Number.ToString() + " (" + DispenserSuffix.Powder + Number.ToString() + ")"; } }

        public void Update(VolumeDos volumeDos, bool showSettings)
        {
            Valve.Update(volumeDos.Valve);
            Flowmeter.Update(volumeDos.Flowmeter, showSettings);

            DosedVolume = volumeDos.Loop.DosedVolume;
            RequiredVolume = volumeDos.Loop.RequiredVolume;
        }

        public void Update(ushort[] registers)
        {
            union_bit_field_s status = new union_bit_field_s();
            status.w = registers[(int)VolumeDosModbus.Register.SW];
            Valve.Command = status.s[(int)VolumeDosModbus.StatusWord.VLV_COM];

            float flow = VolumeDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)VolumeDosModbus.Register32.FLOW));
            float volume = VolumeDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)VolumeDosModbus.Register32.VOL));
            float pulsesPerLiter = (float)Flowmeter.PulsesPerLiter;
            if (!Flowmeter.IsPulsesPerLiterFocused)
                pulsesPerLiter = VolumeDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)VolumeDosModbus.Register32.VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            DosedVolume = VolumeDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)VolumeDosModbus.Register32.DOSE_VOL));
            RequiredVolume = VolumeDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)VolumeDosModbus.Register32.REQ_VOL));
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}
    }
}
