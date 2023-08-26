using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using DosingApp.Models.Modbus;

namespace DosingApp.Models.Screen
{
    public class SingleDosScreen : BaseViewModel
    {
        public int Number { get; set; }
        public ValveAdjustableScreen ValveAdjustable { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        
        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }

        public SingleDosScreen(int number)
        {
            this.Number = number;
            ValveAdjustable = new ValveAdjustableScreen() { Name = this.Number.ToString() + "ОД" };
            Flowmeter = new FlowmeterScreen();

            RequiredVolume = 0;
            DosedVolume = 0;
        }

        public string Name { get { return "Одиночный дозатор " + Number.ToString() + " (ОД " + Number.ToString() + ")"; } }

        public void Update(SingleDos singleDos, bool showSettings)
        {
            ValveAdjustable.Update(singleDos.ValveAdjustable, showSettings);
            Flowmeter.Update(singleDos.Flowmeter, showSettings);

            DosedVolume = singleDos.Loop.DosedVolume;
            RequiredVolume = singleDos.Loop.RequiredVolume;
        }

        public void Update(ushort[] registers)
        {
            union_bit_field_s status = new union_bit_field_s();
            status.w = registers[(int)SingleDosModbus.Register.SW];
            ValveAdjustable.Open = status.s[(int)SingleDosModbus.StatusWord.VADJ_OPN];

            float setPoint = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.VADJ_SP));
            float position = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.VADJ_POS));
            ValveAdjustable.Update(setPoint, position);

            float flow = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.FLOW));
            float volume = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.VOL));
            float pulsesPerLiter = (float)Flowmeter.PulsesPerLiter;
            if (!Flowmeter.IsPulsesPerLiterFocused)
                pulsesPerLiter = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            DosedVolume = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.DOSE_VOL));
            RequiredVolume = SingleDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)SingleDosModbus.Register32.REQ_VOL));
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}
    }
}
