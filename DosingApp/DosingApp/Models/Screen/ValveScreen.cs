using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class ValveScreen : BaseViewModel
    {
        public int Number { get; set; }
        public string Name { get; set; }

        private bool command;
        public bool Command
        {
            get { return command; }
            set 
            {
                SetProperty(ref command, value);
                State = value ? "Открыт" : "Закрыт";
            }
        }

        private string state;
        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }
    }
}
