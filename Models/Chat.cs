using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2
{
    public class Chat :Base_ViewModel
    {
        private string name;
        private ObservableCollection<Message> messages;



        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged("Messages"); }
        }



        public Chat()
        {
            messages = new ObservableCollection<Message>();
        }
    }
}
