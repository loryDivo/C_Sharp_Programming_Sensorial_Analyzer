using Progetto;
using System;
using System.Collections.Generic;
using System.Threading;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class Recognizer
    {
        /** Valore minimo che deve assumere il giroscopio X del sensore 0 affinchè si rilevi una rotazione. */
        public const double ROTATION_THRESHOLD = 0.05D;
        /** Angolo minimo che si considera come rotazione volontaria. */
        public const double MIN_ROTATION_ANGLE = 10D;

        /** Soglia al di sotto della quale si è sdraiati. */
        public const double LAY_THRESHOLD = 2.7D;
        /** Soglia al di sotto della quale si è sdraiati/seduti. */
        public const double LAY_SIT_THRESHOLD = 3.7D;
        /** Soglia al di sotto della quale si è sdraiati. */
        public const double SIT_THRESHOLD = 7.1D;

        private VisualThread vThread;
        private PacketBuffer packetBuffer;
        /** Buffer dei pacchetti della finestra. */
        private List<Packet> packets;
        /** Offset dovuto alle finestre semisovrapposte. Usato per calcolare i tempi. */
        private int pktOffset;
        /** Questa flag è True quando stiamo analizzando gli ultimi pacchetti. */
        private Boolean lastPacketsFlag = false;
        /** Angolo theta della finestra precedente per calcolare correttamente quelli nuovi. */
        private double prevThetaForWindow = Double.NaN;
        /** Ultimi pacchetti della finestra precedente usati per le operazioni di Smoothing. */
        private List<Packet> prevWindowLastPackets;
        /** Lista di Action da aggiungere sul file di output. */
        private List<Progetto.Action> actionsToAdd;
        /** Buffer da passare per il grafico di SitLayStand sugli ZedGraph. */
        private List<KeyValuePair<int, double>> sitLayStandActions;

        public Recognizer(VisualThread vThread)
        {
            this.vThread = vThread;
            packets = new List<Packet>();
            prevWindowLastPackets = new List<Packet>();
            actionsToAdd = new List<Progetto.Action>();
            sitLayStandActions = new List<KeyValuePair<int, double>>();
        }

        public void SetPacketBuffer(PacketBuffer packetBuffer)
        {
            this.packetBuffer = packetBuffer;
        }

        /** Delegato con buffer locale come parametro */
        delegate void MethodDelegate(List<Packet> buffer, bool isLast);

        /**
         * Metodo delegato per inizio chiamata asincrona volta a processare
         * la finestra.
         */
        public void ManageWindow(List<Packet> buffer, bool isLast)
        {
            List<Packet> tmp = new List<Packet>(buffer);
            MethodDelegate delegateWindowWork = new MethodDelegate(this.SetWindowAndWork);
            delegateWindowWork.BeginInvoke(tmp, isLast, null, null);
        }

        public void SetWindowAndWork(List<Packet> buffer, bool isLast)
        {
            Monitor.Enter(packets);

            if (isLast)
            {
                this.lastPacketsFlag = true;
            }

            this.packets.AddRange(buffer);

            DetectMotion();
            DetectRotation();
            DetectStandLaySit();

            this.prevWindowLastPackets.Clear();

            if (!lastPacketsFlag)
            {
                for (int i = 190; i < 250; i++) //Aggiungi i pacchetti della finestra precedente
                {
                    this.prevWindowLastPackets.Add(this.packets[i]);
                }
            }

            if (this.packets.Count >= 250) //Rimuovi dai pacchetti i pacchetti non più necessari
                this.packets.RemoveRange(0, 250);

            this.pktOffset += Program.NEXT_WINDOWS_PKTS; //Aggiorna l'offset per la prossima finestra
            packetBuffer.EmptyBuffer(); //Svuota il buffer di PacketBuffer in modo da poterlo riempire nuovamente

            WriteActionsToFile();

            Monitor.Pulse(packets);
            Monitor.Exit(packets);
        }

        private void DetectRotation()
        {
            List<Packet> tmp = new List<Packet>();
            if (this.pktOffset != 0) //Always except the very first window
                tmp.AddRange(this.prevWindowLastPackets.GetRange(0, 60));

            tmp.AddRange(packets);
            List<double> thetas = UtilityFunctions.RetrieveTheta(this.packets, Sensor.Sensor1, DataInfo.Magn, prevThetaForWindow);
            List<double> girX = UtilityFunctions.RetrieveComponent(tmp, Sensor.Sensor1, DataInfo.Gir, Axis.X);

            girX = UtilityFunctions.SmoothingCalculator(girX, 60, this.pktOffset == 0);

            if (this.pktOffset != 0) //Always except the very first window
                girX.RemoveRange(0, 60);

            bool started = false;
            int startTime = -1, endTime = -1;
            double startAngle = 0D, endAngle = 0D;

            for (int i = 0; i < thetas.Count; i++)
            {
                double currentGirX = girX[i];
                double currentTheta = thetas[i];
                if (i == 249) //Middle of the window, before the start of the next window
                {
                    prevThetaForWindow = thetas[i];
                }

                if (currentGirX > ROTATION_THRESHOLD && !started)
                {
                    startTime = i;
                    startAngle = currentTheta;
                    started = true;
                }
                else if (currentGirX < ROTATION_THRESHOLD && started)
                {
                    endTime = i;
                    endAngle = currentTheta;
                    started = false;

                    double result = UtilityFunctions.RadianToDegree(Math.Abs(endAngle - startAngle));

                    if (result >= MIN_ROTATION_ANGLE)
                    {
                        if (endAngle > startAngle)
                        {
                            AddAction(startTime, endTime, "Girata sx di " + result + " gradi", ActionClass.Rotation);
                            //Console.WriteLine("Girata sx " + currentGirX + " " + result + " " + (startTime + pktOffset) + " " + (endTime + pktOffset) + " | " + UtilityFunctions.RadianToDegree(startAngle) + " " + UtilityFunctions.RadianToDegree(endAngle) + " || " + startAngle + " " + endAngle);
                        }
                        else
                        {
                            AddAction(startTime, endTime, "Girata dx di " + result + " gradi", ActionClass.Rotation);
                            //Console.WriteLine("Girata dx " + currentGirX + " " + result + " " + (startTime + pktOffset) + " " + (endTime + pktOffset) + " | " + UtilityFunctions.RadianToDegree(startAngle) + " " + UtilityFunctions.RadianToDegree(endAngle) + " || " + startAngle + " " + endAngle);
                        }
                    }
                }
            }
        }

        private void DetectStandLaySit()
        {
            List<Packet> tmp = new List<Packet>();
            List<double> accXSensor1 = new List<double>();
            List<Packet> correctRange = new List<Packet>();
            if (prevWindowLastPackets.Count > 0)
            {
                correctRange = this.prevWindowLastPackets.GetRange(30, 30);
            }
            tmp.AddRange(correctRange);
            tmp.AddRange(this.packets);

            accXSensor1 = UtilityFunctions.RetrieveComponent(tmp, Sensor.Sensor1, DataInfo.Acc, Axis.X);


            //Smoothing per evitare rilevazioni errate a causa di dati singoli al di sotto di un certo valore
            List<double> smoothingAcc = UtilityFunctions.SmoothingCalculator(accXSensor1, 30, this.pktOffset == 0);

            if (this.pktOffset != 0) //Always except the very first window
                smoothingAcc.RemoveRange(0, 30);

            double current = Double.NaN;
            int previous = -1;
            int layStandOrSit = -1;
            int startTime = 0;

            for (int i = 0; i < smoothingAcc.Count; i++)
            {
                current = smoothingAcc[i];
                if (current <= LAY_THRESHOLD)
                {
                    layStandOrSit = 1; //Lay
                }
                else if (current <= LAY_SIT_THRESHOLD)
                {
                    layStandOrSit = 2; //LaySit
                }
                else if (current <= SIT_THRESHOLD)
                {
                    layStandOrSit = 3; //Sit
                }
                else
                {
                    layStandOrSit = 4; //Stand
                }

                if (this.pktOffset == 0) //If it is the very first window
                {
                    this.sitLayStandActions.Add(new KeyValuePair<int, double>(i + this.pktOffset, current));
                }
                else if (i >= 250) //Do not reconsider the previous 250 values
                {
                    this.sitLayStandActions.Add(new KeyValuePair<int, double>(i + this.pktOffset, current));
                }

                if (i != 0)
                {
                    if (previous != layStandOrSit)
                    {
                        CheckActionLayStandSit(startTime, i, previous);
                        startTime = i;
                    }
                }

                if ((lastPacketsFlag) && (i == smoothingAcc.Count - 1))
                {
                    CheckActionLayStandSit(startTime, i, previous);
                }
                previous = layStandOrSit;
            }

            vThread.AddPointsForSitLayStand(new List<KeyValuePair<int, double>>(sitLayStandActions));
            sitLayStandActions.Clear();
        }

        private void CheckActionLayStandSit(int startTime, int endTime, int previous)
        {
            if (previous == 1)
            {
                AddAction(startTime, endTime, "Sdraiato", ActionClass.SitStandLay);
                //Console.WriteLine("Sdraiato " + (startTime + this.pktOffset) + " " + (endTime + this.pktOffset));
            }
            else if (previous == 2)
            {
                AddAction(startTime, endTime, "Sdraiato/Seduto", ActionClass.SitStandLay);
                //Console.WriteLine("Sdraiato/Seduto " + (startTime + this.pktOffset) + " " + (endTime + this.pktOffset));
            }
            else if (previous == 3)
            {
                AddAction(startTime, endTime, "Seduto", ActionClass.SitStandLay);
                //Console.WriteLine("Seduto " + (startTime + this.pktOffset) + " " + (endTime + this.pktOffset));
            }
            else if (previous == 4)
            {
                AddAction(startTime, endTime, "In Piedi", ActionClass.SitStandLay);
                //Console.WriteLine("In Piedi " + (startTime + this.pktOffset) + " " + (endTime + this.pktOffset));
            }
        }


        private void DetectMotion()
        {
            List<double> devStandardAccSensor1 = UtilityFunctions.DevStandardCalculator(UtilityFunctions.RetrieveModule(this.packets, Sensor.Sensor1, DataInfo.Acc), 60, 9.81D);

            double current = Double.NaN;
            bool previous = false;
            bool fermo = false;
            int startTime = 0;

            for (int i = 0; i < devStandardAccSensor1.Count; i++)
            {
                current = devStandardAccSensor1[i];
                if (current > 0.8D)
                {
                    fermo = false;      //non fermo
                }
                else
                {
                    fermo = true;       //fermo
                }

                if (i != 0)
                {
                    if (previous != fermo)
                    {
                        CheckActionMoveStop(startTime, i, previous);
                        startTime = i;
                    }
                }
                if (lastPacketsFlag && i == devStandardAccSensor1.Count - 1)
                {
                    CheckActionMoveStop(startTime, i, previous);
                }
                previous = fermo;
            }
        }

        private void CheckActionMoveStop(int startTime, int endTime, Boolean fermo)
        {
            if (fermo) //precedentemente fermo
            {
                AddAction(startTime, endTime, "Fermo", ActionClass.Motion);
                //Console.WriteLine("Fermo" + "tempo inizio " + (startTime + this.pktOffset) + " tempo fine" + (endTime + this.pktOffset));
            }
            else //precedentemente non fermo
            {
                AddAction(startTime, endTime, "Non-Fermo", ActionClass.Motion);
                //Console.WriteLine("Non-Fermo" + "tempo inizio " + (startTime + this.pktOffset) + " tempo fine" + (endTime + this.pktOffset));
            }
        }

        private void AddAction(int startTime, int endTime, String ActionInfo, ActionClass actionClass)
        {
            actionsToAdd.Add(
                new Progetto.Action(UtilityFunctions.GetRealTime(startTime + this.pktOffset),
                UtilityFunctions.GetRealTime(endTime + this.pktOffset),
                ActionInfo, actionClass));
        }

        private void WriteActionsToFile()
        {
            Program.DataSaver.savaXmlActions(this.actionsToAdd);
            this.actionsToAdd.Clear();
        }
    }
}