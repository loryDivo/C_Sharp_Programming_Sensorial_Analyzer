using Progetto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class UtilityFunctions
    {
        /** Calcolo del modulo dei 3 valori passati. */
        public static double ModuleCalculator(List<double> values)
        {
            double y = 0;
            for (int j = 0; j < 3; j++)
            {
                y += Math.Pow(values[j], 2);
            }
            y = Math.Sqrt(y);
            return y;
        }

        /** Calcolo della derivata eseguita come rapporto incrementale. */
        public static List<double> DerivativeCalculator(List<double> values)
        {
            List<double> result = new List<double>(values.Count);

            for (int i = 0; i < result.Count - 1; i++)
            {
                result[i] = values[i + 1] - values[i];
            }
            result[values.Count - 1] = 0;

            return result;
        }

        /** Funzione di Smoothing per la lista di valori.
         * Esegue lo smoothing lungo il Range e esclude i primi "Range" pacchetti se il boolean è False. */
        public static List<double> SmoothingCalculator(List<double> values, int range, bool isFirstWindow)
        {
            List<double> smoothedList = new List<double>(values.Count);
            List<double> mediaList = new List<double>();

            double media = 0;

            for (int i = 0; i < values.Count; i++)
            {
                if (i >= range || isFirstWindow)
                {
                    int sxRange = i - range >= 0 ? i - range : 0;
                    int dxRange = i + range < values.Count ? i + range : values.Count - 1;
                    for (int m = sxRange; m <= dxRange; m++)
                    {
                        mediaList.Add(values[m]);
                    }
                    media = FunzioneMedia(mediaList);
                    mediaList.Clear();
                    smoothedList.Add(media);
                }
                else
                {
                    smoothedList.Add(values[i]);
                }
            }
            return smoothedList;
        }

        /** Metodo che restituisce per tutti i pacchetti passati il dato (es. Sensore 0 Acc X). */
        public static List<double> RetrieveComponent(List<Packet> localBuffer, Sensor sensor, DataInfo type, Axis axis)
        {
            List<double> result = new List<double>();
            GetSingleDataListValues(localBuffer, result, type.GetInfoNumber(axis), sensor.GetSensorNumber());
            return result;
        }

        /** Calcolo della deviazione standard con media fissa. */
        public static List<double> DevStandardCalculator(List<double> values, int range, double referenceMedia)
        {
            List<double> devStandardList = new List<double>(values.Count);

            double media = 0;

            List<double> mediaList = new List<double>();

            for (int i = 0; i < values.Count; i++)
            {

                /*
                  * Se indice attuale minore del range calcolo
                  * differenza tra valore meno la media al quadrato
                  *  da 0 a indice attuale più il range e divido 
                  *  per (range + j + 1)
                  * -> ovvero da 0 a j + range + 1
                 */
                if (i < range)
                {
                    for (int m = 0; m <= i + range; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - referenceMedia, 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
                /*
                * Se indice attuale maggiore del range e minore della capacità
                * totale meno il range allora determino differenza 
                * tra valore meno la media al quadrato delle posizioni comprese 
                * in un intorno i + range e i - range e divido per 2*range +1
                * -> ovvero l'intorno + 1
                * 
               */
                else if (i >= range && i < values.Count - range)
                {
                    for (int m = i - range; m <= i + range; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - referenceMedia, 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
                /*
                 * Se indice attuale maggiore della capacità meno il range
                 * allora calcolo differenza del valore meno la media al quadrato
                 * dall'indice meno il range fino alla capacità totale e divido il tutto
                 * per la capacità totale meno (j - range) 
                 * -> ovvero da j - range fino a capacità totale
                 */
                else if (i >= values.Count - range)
                {
                    for (int m = i - range - 1; m < values.Count; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - referenceMedia, 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
            }
            return devStandardList;
        }

        /** Calcolo della deviazione standard con media mobile. */
        public static List<double> DevStandardCalculator(List<double> values, int range)
        {
            List<double> devStandardList = new List<double>(values.Count);

            List<double> mobileMedia = SmoothingCalculator(values, range, true);

            double media = 0;

            List<double> mediaList = new List<double>();

            for (int i = 0; i < values.Count; i++)
            {

                /*
                  * Se indice attuale minore del range calcolo
                  * differenza tra valore meno la media al quadrato
                  *  da 0 a indice attuale più il range e divido 
                  *  per (range + j + 1)
                  * -> ovvero da 0 a j + range + 1
                 */
                if (i < range)
                {
                    for (int m = 0; m <= i + range; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - mobileMedia[m], 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
                /*
                * Se indice attuale maggiore del range e minore della capacità
                * totale meno il range allora determino differenza 
                * tra valore meno la media al quadrato delle posizioni comprese 
                * in un intorno i + range e i - range e divido per 2*range +1
                * -> ovvero l'intorno + 1
                * 
               */
                else if (i >= range && i < values.Count - range)
                {
                    for (int m = i - range; m <= i + range; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - mobileMedia[m], 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
                /*
                 * Se indice attuale maggiore della capacità meno il range
                 * allora calcolo differenza del valore meno la media al quadrato
                 * dall'indice meno il range fino alla capacità totale e divido il tutto
                 * per la capacità totale meno (j - range) 
                 * -> ovvero da j - range fino a capacità totale
                 */
                else if (i >= values.Count - range)
                {
                    for (int m = i - range - 1; m < values.Count; m++)
                    {
                        mediaList.Add(Math.Pow(values[m] - mobileMedia[m], 2));
                    }
                    media = Math.Sqrt(FunzioneMedia(mediaList));
                    mediaList.Clear();
                    devStandardList.Add(media);
                }
            }
            return devStandardList;
        }

        /** Calcola la media. */
        private static double FunzioneMedia(List<double> mediaList)
        {
            double sum = 0;
            for (int i = 0; i < mediaList.Count; i++)
            {
                sum += mediaList[i];
            }
            return sum / mediaList.Count;
        }

        /** Ritorna il tempo corretto a partire dal numero del pacchetto. */
        public static DateTime GetRealTime(int pktIndex)
        {
            DateTime date = Program.LocalDate;
            int freq = Program.Settings.Frequency;
            DateTime result = date.AddSeconds((1D / (double)freq) * pktIndex);
            return result;
        }

        /** Calcola gli angoli di Eulero. */
        public static List<Packet> EuleroCalculator(List<Packet> values)
        {
            List<List<List<double>>> euleroList = new List<List<List<double>>>(values.Count);
            for (int j = 0; j < values.Count; j++)
            {
                euleroList.Add(new List<List<double>>(5));
                for (int i = 0; i < 5; i++)
                {
                    euleroList[j].Add(new List<double>(3));
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < values.Count; j++)
                {
                    euleroList[j][i].Add(Math.Atan2((2 * values[j][i][11] * values[j][i][12] + 2 * values[j][i][9] * values[j][i][10]),
                                                (2 * Math.Pow(values[j][i][9], 2) + 2 * Math.Pow(values[j][i][12], 2) - 1)));
                    euleroList[j][i].Add(Math.Asin(2 * values[j][i][10] * values[j][i][12] - 2 * values[j][i][9] * values[j][i][11]));

                    euleroList[j][i].Add(Math.Atan2((2 * values[j][i][10] * values[j][i][11] + 2 * values[j][i][9] * values[j][i][12]),
                                                (2 * Math.Pow(values[j][i][9], 2) + 2 * Math.Pow(values[j][i][10], 2) - 1)));
                }
            }
            return euleroList;
        }

        /** Ricava l'angolo theta a partire dai valori del magnetometro, considerando l'angolo precedente. */
        public static double FunzioneOrientamento(List<double> values, double previousAngle)
        {
            double y;
            double z;
            if (values.Count < 3) //Se arriva da RetrieveTheta
            {
                y = values[0];
                z = values[1];
            }
            else //In tutti gli altri casi in cui ci sono tutti e tre i valori xyz
            {
                y = values[1];
                z = values[2];
            }

            double result = Math.Atan2(y, z);

            if (result >= -Math.PI && result < 0) //Angoli solo da 0 a 360, quindi aumentiamo solo il terzo e quarto quadrante
                result += Math.PI * 2;

            double difference = Math.Abs(previousAngle - result);

            if (!System.Double.IsNaN(previousAngle))
            {
                while (difference > DegreeToRadian(180D)) //Se lo stacco è >= 180° dobbiamo aumentare/diminuire di 360°
                {
                    if (previousAngle > result)
                    {
                        result += Math.PI * 2;
                        difference = Math.Abs(previousAngle - result);
                    }
                    else
                    {
                        result -= Math.PI * 2;
                        difference = Math.Abs(previousAngle - result);
                    }
                }
            }

            return result;
        }

        public static List<double> RetrieveModule(List<Packet> values, Sensor s, DataInfo d)
        {
            List<double> sensorModule = new List<double>(values.Count);

            //Lista contenente 3 assi 
            List<double> tmpListData = new List<double>(3);

            //Dato temporaneo 
            double tempData;

            //Ritorna numbero sensore
            int sensorNumber = s.GetSensorNumber();

            for (int i = 0; i < values.Count; i++)
            {
                tempData = 0;

                if (d == DataInfo.Acc)
                {
                    GetDataValues(values, tmpListData, i, 0, 3, sensorNumber);
                    tempData = ModuleCalculator(tmpListData);
                }
                else if (d == DataInfo.Gir)
                {
                    GetDataValues(values, tmpListData, i, 3, 6, sensorNumber);
                    tempData = ModuleCalculator(tmpListData);
                }

                //Aggiunta valore modulo
                sensorModule.Add(tempData);
                //Pulizia lista temporanea
                tmpListData.Clear();
            }

            return sensorModule;
        }


        public static List<double> RetrieveTheta(List<Packet> values, Sensor s, DataInfo d, double prevValue)
        {
            //Lista contenente valori senza discontinuità arcotangente
            List<double> thetaValues = new List<double>(values.Count);
            //Lista contenente i tre valori del magnetometro
            List<double> tmpValues = new List<double>(3);

            //Ritorna numero sensore
            int sensorNumber = s.GetSensorNumber();

            List<double> yValues = new List<double>();
            List<double> zValues = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                GetDataValues(values, yValues, i, 7, 8, sensorNumber);
                GetDataValues(values, zValues, i, 8, 9, sensorNumber);
            }

            yValues = UtilityFunctions.SmoothingCalculator(yValues, 15, true);
            zValues = UtilityFunctions.SmoothingCalculator(zValues, 15, true);

            List<List<double>> valuesTheta = new List<List<double>>();
            valuesTheta.Add(yValues);
            valuesTheta.Add(zValues);

            //Dato temporaneo 
            double previousAngle;

            for (int i = 0; i < valuesTheta[0].Count; i++)
            {
                previousAngle = 0;
                tmpValues.Add(valuesTheta[0][i]);
                tmpValues.Add(valuesTheta[1][i]);

                if (i == 0)
                {
                    thetaValues.Add(FunzioneOrientamento(tmpValues, prevValue));
                }
                else if (i > 0)
                {
                    previousAngle = thetaValues[i - 1];
                    thetaValues.Add(FunzioneOrientamento(tmpValues, previousAngle));
                }
                tmpValues.Clear();
            }
            return thetaValues;
        }

        /** Metodo che restituisce per tutti i pacchetti passati il dato (es. Sensore 0 Acc X). */
        private static void GetSingleDataListValues(List<Packet> values, List<double> tmp, int index, int numberOfSensor)
        {
            for (int i = 0; i < values.Count; i++)
            {
                tmp.Add(values[i][numberOfSensor][index]);
            }
        }

        /** Restituisce lista di liste contenente i Data values richiesti (es. Acc X, AccY e AccZ). */
        private static void GetDataListValues(List<Packet> values, List<List<double>> tmp, int championIndex, int initialIndex, int finalIndex, int numberOfSensor)
        {
            for (int i = 0; i < values.Count; i++)
            {
                List<double> res = new List<double>();
                GetDataValues(values, res, i, initialIndex, finalIndex, numberOfSensor);
                tmp.Add(res);
            }
        }

        /** Restituisce lista contenente i Data values richiesti (es. Acc X, AccY e AccZ) del pacchetto. */
        private static void GetDataValues(List<Packet> values, List<double> tmp, int championIndex, int initialIndex, int finalIndex, int numberOfSensor)
        {
            for (int i = initialIndex; i < finalIndex; i++)
            {
                tmp.Add(values[championIndex][numberOfSensor][i]);
            }
        }

        public static double DegreeToRadian(double angle)
        {
            return angle * Math.PI / 180.0D;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0D / Math.PI);
        }
    }
}