using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2
{
    public class Message : Base_ViewModel
    {
        private string sender,
                       text;



        public string Sender
        {
            get { return sender; }
            set { sender = value; OnPropertyChanged("Sender"); }
        }
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged("Text"); }
        }



        public Message()
        {

        }
    }
}
