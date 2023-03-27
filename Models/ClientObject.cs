using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace _2
{
    public class ClientObject
    {
        private Socket socketSend,
                       socketReceive;
        private IPEndPoint ipPointSend,
                           ipPointReceive;
        private string receivedMessage;
        private int receivingPort,
                    sendingPort,
                    numberOfBytes;

        private byte[] dataSend,
                       dataReceive;
        


        public string ReceivedMessage
        {
            get { return receivedMessage; }
            set { receivedMessage = value; }
        }



        public async Task SendMessageAsync(string Message)
        {
            await Task.Run(() => SendMessage(Message));
        }



        public ClientObject(string IPadress)
        {
            sendingPort = 14000;
            receivingPort = 14001;

            ipPointSend = new IPEndPoint(IPAddress.Parse(IPadress), sendingPort); 
            ipPointReceive = new IPEndPoint(IPAddress.Any, receivingPort);
            receivedMessage = "";
        }



        private void SendMessage(in string Message)
        {
            using (socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socketSend.Connect(ipPointSend);
                    dataSend = Encoding.UTF8.GetBytes(Message);
                    socketSend.Send(dataSend);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    socketSend.Close();
                }
            }
        }
        public void ReceiveMessage()
        {
            try
            {
                using (socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socketReceive.Bind(ipPointReceive);
                    socketReceive.Listen(10);
                    dataReceive = new byte[256];
                    using (var handler = socketReceive.Accept())
                    {
                        numberOfBytes = handler.Receive(dataReceive);
                        if (numberOfBytes > 0)
                        {
                            // отсекаем нулеваые байты, не несущие информации
                            List<byte> finalArray = new List<byte>();
                            foreach (byte element in dataReceive)
                            {
                                if (element != 0x00)
                                {
                                    finalArray.Add(element);
                                }
                            }
                            receivedMessage = Encoding.UTF8.GetString(finalArray.ToArray());
                        }
                        else
                        {
                            receivedMessage = "";
                        }

                        handler.Shutdown(SocketShutdown.Receive);
                        handler.Close();
                    }
                    dataReceive = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socketReceive.Close();
            }
        }
    }
}
