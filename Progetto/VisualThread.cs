using Progetto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class VisualThread
    {
        /** Aspetta questo numero di pacchetti per stampare sui grafici. Evita un forte lag in caso di alta frequenza. */
        public static int PACKETS_TO_UPDATE = 10;
        public PointPairList pointPlAccSensor1;
        public PointPairList pointPlAccSensor2;
        public PointPairList pointPlAccSensor3;
        public PointPairList pointPlAccSensor4;
        public PointPairList pointPlAccSensor5;
        public PointPairList pointPlGirSensor1;
        public PointPairList pointPlGirSensor2;
        public PointPairList pointPlGirSensor3;
        public PointPairList pointPlGirSensor4;
        public PointPairList pointPlGirSensor5;
        public PointPairList pointPlThetaSensor1;
        /*public PointPairList pointPlThetaSensor2;
        public PointPairList pointPlThetaSensor3;
        public PointPairList pointPlThetaSensor4;
        public PointPairList pointPlThetaSensor5;*/
        public PointPairList pointPlStandLaySit;
        /** Valore sull'asseX dei grafici che si aggiornano con i valori RAW. */
        private double xAxis;
        /** Istanza WindowsForm contenente i grafici ZedGraph per la rappresentazione dei pacchetti. */
        private DataGraph dataGraph;

        /** Buffer di accumulo pacchetti. */
        private List<Packet> incomingPackets;

        public VisualThread()
        {
        }

        public void StartThread()
        {
            incomingPackets = new List<Packet>();
            pointPlAccSensor1 = new PointPairList();
            pointPlAccSensor2 = new PointPairList();
            pointPlAccSensor3 = new PointPairList();
            pointPlAccSensor4 = new PointPairList();
            pointPlAccSensor5 = new PointPairList();
            pointPlGirSensor1 = new PointPairList();
            pointPlGirSensor2 = new PointPairList();
            pointPlGirSensor3 = new PointPairList();
            pointPlGirSensor4 = new PointPairList();
            pointPlGirSensor5 = new PointPairList();
            pointPlThetaSensor1 = new PointPairList();
            /*pointPlThetaSensor2 = new PointPairList();
            pointPlThetaSensor3 = new PointPairList();
            pointPlThetaSensor4 = new PointPairList();
            pointPlThetaSensor5 = new PointPairList();*/
            pointPlStandLaySit = new PointPairList();
            dataGraph = new DataGraph(this);
            Application.EnableVisualStyles();
            Application.Run(dataGraph);
        }

        public void AddPointZedGraph(PointPairList pointPL, double x, double y)
        {
            pointPL.Add(x, y);
        }

        /** Aggiunge il pacchetto per la rappresentazione nel buffer. Se il secondo parametro è True aggiunge immediatamente tutti i pacchetti accumulati sul grafico. */
        public void AddPointsFromPacket(Packet packet, bool shouldUpdateImmediately)
        {
            if (dataGraph.InvokeRequired)
            {
                dataGraph.BeginInvoke(new MethodInvoker(delegate ()
                {
                    AddPointsFromPacket(packet, shouldUpdateImmediately);
                }));
            }
            else
            {
                if (shouldUpdateImmediately)
                {
                    foreach (Packet pkt in incomingPackets)
                    {
                        AddValuesToLists(pkt);
                    }
                    dataGraph.UpdateAllZedGraph();
                    incomingPackets.Clear();
                }
                else
                {
                    incomingPackets.Add(packet);

                    if (incomingPackets.Count >= PACKETS_TO_UPDATE)
                    {
                        foreach (Packet pkt in incomingPackets)
                        {
                            AddValuesToLists(pkt);
                            dataGraph.UpdateAllZedGraph();
                        }
                        incomingPackets.Clear();
                    }
                }
            }
        }

        /** Disegna il pacchetto sugli ZedGraph. */
        private void AddValuesToLists(Packet packet)
        {
            AddPointZedGraph(pointPlAccSensor1, xAxis, RetrieveModule(packet, Sensor.Sensor1, DataInfo.Acc));
            AddPointZedGraph(pointPlAccSensor2, xAxis, RetrieveModule(packet, Sensor.Sensor2, DataInfo.Acc));
            AddPointZedGraph(pointPlAccSensor3, xAxis, RetrieveModule(packet, Sensor.Sensor3, DataInfo.Acc));
            AddPointZedGraph(pointPlAccSensor4, xAxis, RetrieveModule(packet, Sensor.Sensor4, DataInfo.Acc));
            AddPointZedGraph(pointPlAccSensor5, xAxis, RetrieveModule(packet, Sensor.Sensor5, DataInfo.Acc));
            AddPointZedGraph(pointPlGirSensor1, xAxis, RetrieveModule(packet, Sensor.Sensor1, DataInfo.Gir));
            AddPointZedGraph(pointPlGirSensor2, xAxis, RetrieveModule(packet, Sensor.Sensor2, DataInfo.Gir));
            AddPointZedGraph(pointPlGirSensor3, xAxis, RetrieveModule(packet, Sensor.Sensor3, DataInfo.Gir));
            AddPointZedGraph(pointPlGirSensor4, xAxis, RetrieveModule(packet, Sensor.Sensor4, DataInfo.Gir));
            AddPointZedGraph(pointPlGirSensor5, xAxis, RetrieveModule(packet, Sensor.Sensor5, DataInfo.Gir));
            AddPointZedGraph(pointPlThetaSensor1, xAxis, RetrieveTheta(packet, Sensor.Sensor1, DataInfo.Magn));
            /*AddPointZedGraph(pointPlThetaSensor2, xAxis, RetrieveTheta(packet, Sensor.Sensor2, DataInfo.Magn));
            AddPointZedGraph(pointPlThetaSensor3, xAxis, RetrieveTheta(packet, Sensor.Sensor3, DataInfo.Magn));
            AddPointZedGraph(pointPlThetaSensor4, xAxis, RetrieveTheta(packet, Sensor.Sensor4, DataInfo.Magn));
            AddPointZedGraph(pointPlThetaSensor5, xAxis, RetrieveTheta(packet, Sensor.Sensor5, DataInfo.Magn));*/
            xAxis++;
        }

        private double RetrieveModule(Packet packet, Sensor s, DataInfo d)
        {
            List<double> data = SpecificSensorData(packet, s, d);
            return UtilityFunctions.ModuleCalculator(data);
        }

        private double RetrieveTheta(Packet packet, Sensor s, DataInfo d)
        {
            List<double> data = SpecificSensorData(packet, s, d);
            double resultRadians = UtilityFunctions.FunzioneOrientamento(data, UtilityFunctions.DegreeToRadian(GetSpecificPreviousAngle(s)));
            return UtilityFunctions.RadianToDegree(resultRadians);
        }

        private double GetSpecificPreviousAngle(Sensor s)
        {
            PointPairList list = null;

            switch (s)
            {
                case Sensor.Sensor1:
                    {
                        list = pointPlThetaSensor1;
                        break;
                    }
                    /*case Sensor.Sensor2:
                        {
                            list = pointPlThetaSensor2;
                            break;
                        }
                    case Sensor.Sensor3:
                        {
                            list = pointPlThetaSensor3;
                            break;
                        }
                    case Sensor.Sensor4:
                        {
                            list = pointPlThetaSensor4;
                            break;
                        }
                    case Sensor.Sensor5:
                        {
                            list = pointPlThetaSensor5;
                            break;
                        }*/
            }

            if (list.Count == 0)
            {
                return Double.NaN;
            }

            return list[list.Count - 1].Y; //Latest point Y value
        }

        /** Funzione apposita per il grafico SitLayStand, chiamata da Recognizer dopo l'analisi. */
        public void AddPointsForSitLayStand(List<KeyValuePair<int, double>> points)
        {
            if (dataGraph.InvokeRequired)
            {
                dataGraph.BeginInvoke(new MethodInvoker(delegate ()
                {
                    AddPointsForSitLayStand(points);
                }));
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pointPlStandLaySit.Add(points[i].Key, points[i].Value);
                }
                dataGraph.UpdateAllZedGraph();
            }
        }

        /** Restituisce i dati specifici dalla lista di pacchetti a seconda del sensore e del campo scelti. */
        private List<double> SpecificSensorData(Packet pkt, Sensor s, DataInfo d)
        {
            List<double> data = new List<double>();
            int position = s.GetSensorNumber();

            if (d == DataInfo.Acc)
            {
                for (int j = 0; j < 3; j++)
                {
                    data.Add(pkt[position][j]);
                }
            }
            else if (d == DataInfo.Gir)
            {
                for (int j = 0; j < 3; j++)
                {
                    data.Add(pkt[position][3 + j]);
                }
            }
            else if (d == DataInfo.Magn)
                for (int j = 0; j < 3; j++)
                {
                    data.Add(pkt[position][6 + j]);
                }

            return data;
        }
    }
}