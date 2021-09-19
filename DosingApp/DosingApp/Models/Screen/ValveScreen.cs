using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class ValveScreen : BaseViewModel
    {
        private bool command;
        private string state;

        public void Update(Valve valve)
        {
            Command = (bool)valve.Command;
        }

        public int Number { get; set; }
        public string Name { get; set; }
        
        public bool Command
        {
            get { return command; }
            set 
            {
                SetProperty(ref command, value);
                State = value ? "ОТКР" : "ЗАКР";
            }
        }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }
    }
}
