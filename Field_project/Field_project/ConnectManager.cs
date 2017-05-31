using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Field_project
{
    public class ConnectManager
    {
        private const int port = 3000;
        private const string server = "192.168.43.251";
        private const short count = 3;
        TcpClient client = new TcpClient();
        private string Id = "id0";

        //Конструктор, который выдает Id
        public ConnectManager()
        {


            client.Connect(server, port);
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(Id);

            stream.Write(data, 0, data.Length);//Отправляем сообщение 
            stream.Read(data, 0, data.Length);//Получаем сообщение 
            Id = Encoding.UTF8.GetString(data, 0, count);//Присваиваем полученный id новому пользователю

            // Закрываем потоки
            stream.Close();
            client.Close();
        }

        //Тупой метод отправки сообщений
        private void SetMessage(string message)
        {
            if (message.Length != 3)
                throw new Exception("Сообщение некорректной длины");
            //client = new TcpClient();
            //client.Connect(server, port);
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);

            stream.Close();
        }

        //Проверяет возможен ли ход или сейчас ход другого игрока
        public bool IsMyStep()
        {
            client = new TcpClient();
            client.Connect(server, port);
            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(Id);
            stream.Write(data, 0, data.Length);//Отправляем сообщение 
            stream.Read(data, 0, data.Length);//Получаем сообщение 
            string message = Encoding.UTF8.GetString(data, 0, count);
            if (message == "ok0")
            {
                return true;
            }
            else
            {
                client.Close();
                stream.Close();
                return false;
            }

        }

        //Устанавливает полезное сообщение
        public void SetUsefulMessage(string message)
        {
            if (message.Length != 3)
                throw new Exception("Сообщение некорректной длины");
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("set");
            stream.Write(data, 0, data.Length);
            System.Threading.Thread.Sleep(1000);

            data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        //Получает полезное сообщение
        public string GetUsefulMessage()
        {
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("get");
            stream.Write(data, 0, data.Length);

            int bytes = stream.Read(data, 0, data.Length); // получаем количество считанных байтов
            string message = Encoding.UTF8.GetString(data, 0, count);

            return message;
        }

        //Закрытие сессии и переключение на другого клиента
        public void EndSession()
        {
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes("ok1");
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
        }
    }
}