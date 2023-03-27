using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2
{
    public class MessangerViewModel : Base_ViewModel
    {
        private ObservableCollection<Message> chatMessages;
        private Chat currentChat;
        private string input_text,
                       ipAddress;
        private ClientObject client;
        private bool sendEnabled;



        public Command Send { get; private set; }
        public Command Clear { get; private set; }
        public Chat CurrentChat
        {
            get { return currentChat; }
            set { currentChat = value; OnPropertyChanged("ChatMessages"); }
        }
        public ObservableCollection<Message> ChatMessages
        {
            get { return currentChat.Messages; } 
        }
        public string Input_text
        {
            get { return input_text; }
            set 
            { 
                input_text = value; 
                OnPropertyChanged("Input_text"); 
                if (input_text.Length > 0)
                {
                    sendEnabled = true;
                }
                else { sendEnabled = false; }
            }
        }



        public async Task CheckForMessagesAsync()
        {
            await Task.Run(() => CheckForMessages());
        }



        public MessangerViewModel()
        {
            Send = new Command(o => _Send(), o => sendEnabled);
            Clear = new Command(o => _Clear());
            
            currentChat = new Chat();
            chatMessages = CurrentChat.Messages;

            // Чтение адреса сервера чата из текстового файла
            ipAddress = File.ReadAllText(Directory.GetCurrentDirectory() + "\\ServerIP.txt");

            client = new ClientObject(ipAddress);
            // Отправляем сервисное сообщение, чтобы сервер запомнил клиента
            client.SendMessageAsync(" ");
            // Начинаем принимать входящие сообщения
            Task.Run(CheckForMessagesAsync);
        }



        private void _Send()
        {
            Message msg = new Message();
            msg.Sender = "Вы";
            msg.Text = input_text;
            currentChat.Messages.Add(msg);
            client.SendMessageAsync(msg.Text);
            Input_text = "";
        }
        private void CheckForMessages ()
        {
            while (true)
            {
                client.ReceiveMessage();
                if (client.ReceivedMessage != "")
                {
                    Message msg = new Message();
                    msg.Sender = "Собеседник";
                    msg.Text = client.ReceivedMessage;
                    // Прием собщений происходит в асинхронном потоке, а обновление визуального интерфейса должно происходить в основном
                    // потому ииспользуем данную конструкцию
                    App.Current.Dispatcher.Invoke(() => currentChat.Messages.Add(msg));
                    client.ReceivedMessage = "";
                }
            }
        }
        private void _Clear()
        {
            currentChat.Messages.Clear();
        }
    }
}
