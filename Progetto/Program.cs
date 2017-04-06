using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progetto
{
    static class Program
    {
        public const int FIRST_WINDOW_PKTS = 500;
        public const int NEXT_WINDOWS_PKTS = 250;

        private static Settings settings;
        /** Buffer che accumula i pacchetti ricevuti per consentire le operazioni di visualizzazione, di analisi e di salvataggio. */
        private static PacketBuffer packetBuffer;
        /** Istanza che si occupa di salvare i dati ricevuti e i risultati delle analisi. */
        private static DataSaver dataSaver;
        private static DateTime localDate;

        public static void Main()
        {
            settings = new Settings();

            InitialPanel iPanel = new InitialPanel();
            Application.EnableVisualStyles();
            Application.Run(iPanel);
            VisualThread vThread = new VisualThread();
            Thread graphicThread = new Thread(vThread.StartThread);
            graphicThread.Start();
            dataSaver = new DataSaver();
            Recognizer recognizer = new Recognizer(vThread);
            packetBuffer = new PacketBuffer(vThread, recognizer, dataSaver);
            recognizer.SetPacketBuffer(PacketBuffer);
            Listener listener = new Listener(PacketBuffer);
            TcpClient client = listener.Connection();
            listener.StreamRead(client);
        }

        public static Settings Settings
        {
            get
            {
                return settings;
            }
        }

        public static PacketBuffer PacketBuffer
        {
            get
            {
                return packetBuffer;
            }
        }

        public static DataSaver DataSaver
        {
            get
            {
                return dataSaver;
            }
        }

        public static DateTime LocalDate
        {
            get
            {
                return localDate;
            }

            set
            {
                localDate = value;
            }
        }

        public static void CloseApplication()
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
