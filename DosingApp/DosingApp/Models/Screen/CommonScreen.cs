﻿using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class CommonScreen : BaseViewModel
    {
        public virtual ValveAdjustableScreen ValveAdjustable { get; set; }
        public virtual FlowmeterScreen Flowmeter { get; set; }

        public CommonScreen()
        {
            ValveAdjustable = new ValveAdjustableScreen() { Name = "РегКл" };
            Flowmeter = new FlowmeterScreen();
        }

        public void Update(Common common, bool showSettings)
        {
            ValveAdjustable.Update(common.ValveAdjustable, showSettings);
            Flowmeter.Update(common.Flowmeter, showSettings);
        }

        //public void InitNew(Common common, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(common.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(common.Flowmeter, showSettings);
        //}
    }
}