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

        private bool pumpCommand;

        public PowderDosScreen(int number)
        {
            this.Number = number;
            Valve = new ValveScreen() { Name = this.Number.ToString() + DispenserSuffix.Powder };
            Flowmeter = new FlowmeterScreen();

            RequiredVolume = 0;
            DosedVolume = 0;
        }

        public string Name { get { return "Порошковый дозатор " + Number.ToString() + " (" + DispenserSuffix.Powder + Number.ToString() + ")"; } }

        public void Update(PowderDos powderDos, bool showSettings)
        {
            Valve.Update(powderDos.Valve);
            Flowmeter.Update(powderDos.Flowmeter, showSettings);

            DosedVolume = powderDos.Loop.DosedVolume;
            RequiredVolume = powderDos.Loop.RequiredVolume;
        }

        public void Update(ushort[] registers)
        {
            union_bit_field_s status = new union_bit_field_s();
            status.w = registers[(int)PowderDosModbus.Register.SW];
            PumpCommand = status.s[(int)PowderDosModbus.StatusWord.PUMP_COM];
            Valve.Command = status.s[(int)PowderDosModbus.StatusWord.VLV_COM];

            float flow = PowderDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)PowderDosModbus.Register32.FLOW));
            float volume = PowderDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)PowderDosModbus.Register32.VOL));
            float pulsesPerLiter = (float)Flowmeter.PulsesPerLiter;
            if (!Flowmeter.IsPulsesPerLiterFocused)
                pulsesPerLiter = PowderDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)PowderDosModbus.Register32.VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            DosedVolume = PowderDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)PowderDosModbus.Register32.DOSE_VOL));
            RequiredVolume = PowderDosModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)PowderDosModbus.Register32.REQ_VOL));
        }


        public bool PumpCommand
        {
            get { return pumpCommand; }
            set { SetProperty(ref pumpCommand, value); }
        }
    }
}
