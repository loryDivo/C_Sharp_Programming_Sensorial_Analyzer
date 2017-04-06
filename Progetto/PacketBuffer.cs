using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class PacketBuffer
    {
        /** Buffer che accumula pacchetti man mano che vengono ricevuti. */
        private List<Packet> localBuffer = new List<Packet>();
        private VisualThread vThread;
        private DataSaver dataSaver;
        private Recognizer recognizer;
        /** Flag che indica se la prima finestra è stata già riempita (se True è una finestra successiva). */
        private bool firstWindowDone = false;
        /** Flag che indica se la prima finestra è stata già svuotata (se True è una finestra successiva). */
        private bool firstWindowClear = false;
        /** Flag che indica se è stato mandata la finestra al recognizer e che non è ancora stata svuotata. */
        private bool sent = false;

        public PacketBuffer(VisualThread vThread, Recognizer recognizer, DataSaver dataSaver)
        {
            this.vThread = vThread;
            this.recognizer = recognizer;
            this.dataSaver = dataSaver;
        }

        /** Aggiunge pacchetto al buffer, lo visualizza sui grafici, lo salva su CSV e controlla se è ora di mandare la finestra
         per il lavoro di analisi. */
        public void AddPacketToList(Packet packet)
        {
            Monitor.Enter(localBuffer);
            localBuffer.Add(packet);
            vThread.AddPointsFromPacket(packet, false);
            dataSaver.SaveCsvPacket(packet);
            if (!sent)
            {
                if (!firstWindowDone && localBuffer.Count() >= Program.FIRST_WINDOW_PKTS)
                {
                    firstWindowDone = true;
                    sent = true;
                    recognizer.ManageWindow(localBuffer.GetRange(0, Program.FIRST_WINDOW_PKTS), false);
                }
                else if (firstWindowDone && localBuffer.Count() >= Program.NEXT_WINDOWS_PKTS)
                {
                    sent = true;
                    recognizer.ManageWindow(localBuffer.GetRange(0, Program.NEXT_WINDOWS_PKTS), false);
                }
            }
            Monitor.Pulse(localBuffer);
            Monitor.Exit(localBuffer);
        }

        /** Metodo chiamato quando sono rimasti solo gli ultimi pacchetti di numero minore della dimensione della finestra. */
        public bool ConsiderLastPackets()
        {
            Monitor.Enter(localBuffer);
            if (!sent)
            {
                //Console.WriteLine("FINALLY PASSING LAST PACKETS");
                vThread.AddPointsFromPacket(null, true);
                recognizer.ManageWindow(localBuffer, true);
                Monitor.Pulse(localBuffer);
                Monitor.Exit(localBuffer);
                return true;
            } else
            {
                Monitor.Pulse(localBuffer);
                Monitor.Exit(localBuffer);
                return false;
            }
        }

        /** Svuota il buffer locale, togliendo SOLO i pacchetti della finestra usata. Altri pacchetti successivi potrebbero essersi
         * accumulati. */
        public void EmptyBuffer()
        {
            Monitor.Enter(localBuffer);
            //Console.WriteLine("CLEARING PACKET BUFFER");
            if (!firstWindowClear)
            {
                firstWindowClear = true;
                sent = false;
                if (localBuffer.Count >= Program.FIRST_WINDOW_PKTS)
                {
                    localBuffer.RemoveRange(0, Program.FIRST_WINDOW_PKTS);
                }
            }
            else
            {
                sent = false;
                if (localBuffer.Count >= Program.NEXT_WINDOWS_PKTS)
                    localBuffer.RemoveRange(0, Program.NEXT_WINDOWS_PKTS);
            }
            Monitor.Pulse(localBuffer);
            Monitor.Exit(localBuffer);
        }
    }
}
