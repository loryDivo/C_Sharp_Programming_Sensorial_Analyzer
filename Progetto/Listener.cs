using Progetto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class Listener
    {
        private PacketBuffer packetBuffer;

        public Listener(PacketBuffer packetBuffer)
        {
            this.packetBuffer = packetBuffer;
        }

        public TcpClient Connection()
        {
            TcpListener Server;
            Int32 port = 45555;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            Server = new TcpListener(localAddr, port);
            Server.Start();
            Console.WriteLine("Waiting for a connection... ");
            TcpClient client = Server.AcceptTcpClient();
            Console.WriteLine("Connected!");
            return client;
        }

        public void StreamRead(TcpClient client)
        {
            NetworkStream stream = client.GetStream();      //incapsulamento in un network stream
            byte[] len = new byte[2];
            byte[] tem = new byte[3];
            BinaryReader bin = new BinaryReader(stream);   //incapsulamento in un BinaryReader
            ParsingData(len, tem, bin, stream);            //Parsing dei dati con aggiunta in buffer
        }

        public void ParsingData(Byte[] len, Byte[] tem, BinaryReader bin, NetworkStream stream)
        {
            int byteToRead;
            byte[] pacchetto;
            int numSensori;
            int maxSensori = 10;
            float valore;
            while (!(tem[0] == 0xFF && tem[1] == 0x32))//cerca la sequenza FF-32
            {
                tem[0] = tem[1];
                tem[1] = tem[2];
                byte[] read = bin.ReadBytes(1);
                tem[2] = read[0];
            }
            if (tem[2] != 0xFF) // modalità normale
            {
                byteToRead = tem[2]; // byte da leggere
            }
            else  // modalità extended-length
            {
                len = new byte[2];
                len = bin.ReadBytes(2);
                byteToRead = ((len[0] * 256) + len[1]); // byte da leggere
            }

            byte[] data = new byte[byteToRead + 1];
            data = bin.ReadBytes(byteToRead + 1); // lettura dei dati

            if (tem[2] != 0xFF)
            {
                pacchetto = new byte[byteToRead + 4]; // creazione pacchetto
            }
            else
            {
                pacchetto = new byte[byteToRead + 6];
            }

            numSensori = (byteToRead - 2) / 52; // calcolo del numero di sensori
            pacchetto[0] = 0xFF; // copia dei primi elementi
            pacchetto[1] = 0x32;
            pacchetto[2] = tem[2];

            if (tem[2] != 0xFF)
            {
                data.CopyTo(pacchetto, 3); // copia dei dati
            }
            else
            {
                pacchetto[3] = len[0];
                pacchetto[4] = len[1];
                data.CopyTo(pacchetto, 5); // copia dei dati
            }


            List<List<double>> array = new List<List<double>>(); // salvataggio dati
            int[] t = new int[maxSensori];

            for (int x = 0; x < numSensori; x++)
            {
                array.Add(new List<double>()); // una lista per ogni sensore
                t[x] = 5 + (52 * x);
            }
            try
            {
                Program.LocalDate = DateTime.Now; //Setta la data corrente per eseguire il calcolo dei tempi sui pacchetti

                while (true)
                {
                    for (int i = 0; i < numSensori; i++)
                    {
                        byte[] temp = new byte[4];
                        for (int tr = 0; tr < 13; tr++)// 13 campi, 3 * 3 + 4
                        {
                            if (numSensori < 5)
                            {
                                temp[0] = pacchetto[t[i] + 3]; // lettura inversa
                                temp[1] = pacchetto[t[i] + 2];
                                temp[2] = pacchetto[t[i] + 1];
                                temp[3] = pacchetto[t[i]];
                            }
                            else
                            {
                                temp[0] = pacchetto[t[i] + 5];
                                temp[1] = pacchetto[t[i] + 4];
                                temp[2] = pacchetto[t[i] + 3];
                                temp[3] = pacchetto[t[i] + 2];
                            }
                            valore = BitConverter.ToSingle(temp, 0); // convers
                            array[i].Add(valore); // memorizzazione
                            t[i] += 4;
                        }
                    }

                    for (int x = 0; x < numSensori; x++)
                    {
                        t[x] = 5 + (52 * x);
                    }

                    Packet pkt = new Packet(); //lista di liste di 5 sensori con 13 valori
                    for (int i = 0; i < 5; i++)
                    {
                        pkt.Add(new List<double>()); //una lista per ogni sensore
                        for (int m = 0; m < 13; m++)
                        {
                            pkt[i].Add(array[i][m]); //lista(sensore) contenente 13 valori
                        }
                    }

                    packetBuffer.AddPacketToList(pkt); //Aggiungi pacchetto al buffer locale

                    for (int j = 0; j < numSensori; j++)
                    {
                        array[j].RemoveRange(0, 13); // cancellazione dati
                    }

                    if (numSensori < 5) // lettura pacchetto seguente
                    {
                        pacchetto = bin.ReadBytes(byteToRead + 4);
                    }
                    else
                    {
                        pacchetto = bin.ReadBytes(byteToRead + 6);
                    }
                }

            }
            catch (System.IndexOutOfRangeException e)
            {
                while (!packetBuffer.ConsiderLastPackets()) ; //Loop fino a quando Recognizer non è disponibile
                stream.Close();
                bin.Close();
            }
        }
    }
}