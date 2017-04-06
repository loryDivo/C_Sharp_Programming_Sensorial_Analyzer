using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Packet = System.Collections.Generic.List<System.Collections.Generic.List<double>>;

namespace Progetto
{
    public class DataSaver
    {
        //Path base (...\bin\Debug)
        private static string appPath = Application.StartupPath;
        
        //Nome cartella csv
        private static string csvFolder = "Dati grezzi rilevati";
        //Nome file csv
        private string pathFileCsv = "/dati_grezzi.csv";
        //Path csv file
        private String csvPathFile;

        //Path xml file
        private String xmlPathFile;
        //nome cartella xml
        private static string xmlFolder = "Azioni rilevate";
        //Nome file xml
        private String xmlFileName = "/azioni_rilevate.xml";
        //Documento xml
        private XmlDocument xmlDoc;

        private static String format_date_xml = Action.format_date_xml;

        public DataSaver()
        {
            CheckExistFile();
        }

        private void CheckExistFile()
        {
            CheckCsvExistFile();
            CheckXmlExistFile();
        }

        /** Verifica esistenza file csv e creazione del file csv dell'acquisizione corrente */
        private void CheckCsvExistFile()
        {
            //Correlazione cartella con path base
            string folder_path_csv = string.Format(@"{0}\{1}", appPath, csvFolder);

            //Creazione directory se non esiste
            System.IO.Directory.CreateDirectory(folder_path_csv);

            //path di destinazione
            this.csvPathFile = folder_path_csv + pathFileCsv;

            DeleteExistFileIfExist(csvPathFile);
        }

        /** Verifica esistenza file xml e creazione del file xml dell'acquisizione corrente con conseguente aggiunta del nodo principale "actions". */
        private void CheckXmlExistFile()
        {
            //Correlazione cartella con path base
            string folder_path_xml = string.Format(@"{0}\{1}", appPath, xmlFolder);
            //Creazione directory se non esiste
            System.IO.Directory.CreateDirectory(folder_path_xml);

            this.xmlPathFile = folder_path_xml + xmlFileName;

            //creazione nuovo file xml e cancellazione file già esistente qualora esista
            //Istanziazione reader 
            XmlTextReader reader = new XmlTextReader(xmlPathFile);

            DeleteExistFileIfExist(xmlPathFile);

            this.xmlDoc = new XmlDocument();

            XmlNode impactsNode;

            //Aggiunta nodo principale (azioni) e prima creazione file
            impactsNode = this.xmlDoc.CreateElement("actions");
            this.xmlDoc.AppendChild(impactsNode);
            //creazione effettiva file xml
            this.xmlDoc.Save(xmlPathFile);
        }

        private void DeleteExistFileIfExist(String path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /** Salvataggio dati su file CSV. */
        public void SaveCsvPacket(Packet packet)
        {

            //stream di scrittura su file (true sta ad indicare di andare a capo ad ogni WriteLine)
            StreamWriter streamFileCsv = new StreamWriter(csvPathFile, true);

            //Separatore 
            const string separator = ";";

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    stringBuilder.Append(packet[i][j]);
                    stringBuilder.Append(separator);
                }

                //Separazione sensori con una casella vuota
                stringBuilder.Append(separator);
            }
            //Scrittura su file e chiusura
            streamFileCsv.WriteLine(stringBuilder);
            streamFileCsv.Close();
        }


        /** 
         * Salvataggio azioni su file XML.
        */
        public void savaXmlActions(List<Action> actionsList)
        {
            Action tmpAction;

            //Ricerca nodo radice
            XmlNode root = xmlDoc.GetElementsByTagName("actions")[0];


            //Ricerca azioni all'interno del documento
            XmlNodeList xmlNodeList;

            //Nodo precedente a nodo da inserire
            XmlNode prevNode = null;

            XmlNode prevActionNode;

            for (int i = 0; i < actionsList.Count; i++)
            {
                xmlNodeList = this.xmlDoc.SelectNodes("/actions/action");
                tmpAction = actionsList[i];
                //Console.WriteLine("Start time " + actionsList[i].StartTime + " End time " + actionsList[i].EndTime);
                if (!IsDuplicatedNode(tmpAction, xmlNodeList))
                {
                    prevNode = SearchCorrectNodePosition(tmpAction, xmlNodeList);
                    if (prevNode != null)           //Controllo correttezza data di fine precedente nodo e data di inizio nodo successivo (se prevNode == null allora è la prima azione)
                    {
                        prevActionNode = SearchPrevActionClassNode(tmpAction, xmlNodeList); //Ricerca nodo della stessa classe precedente
                        if (prevActionNode != null)         //Se non null allora non è il primo ad essere inserito
                        {
                            tmpAction.StartTime = CorrectDateTime(prevActionNode, tmpAction);      
                        }
                    }
                    WriteDataXml(this.xmlDoc, tmpAction, prevNode, root);
                }
            }
        }

        /** Ritrova l'Action più recente della stessa classe di Actions. */
        private XmlNode SearchPrevActionClassNode(Action newAction, XmlNodeList xmlNodeList)
        {
            XmlNode prevNodeOfSameActionClass = null;

            if (xmlNodeList.Count > 0)
            {
                XmlNode tmp = xmlNodeList.Item(xmlNodeList.Count - 1);
                while (tmp != null)
                {
                    ActionClass tmpNameActionClass;
                    tmpNameActionClass = tmp.SelectSingleNode("actionClass").InnerText.GetActionClassEnum();
                    XmlNode prevTmp = tmp;

                    if (SameActionClassExceptRotation(tmpNameActionClass, newAction.ActionClass))
                    {
                        prevNodeOfSameActionClass = prevTmp;
                        break;
                    }

                    tmp = tmp.PreviousSibling;
                }
            }

            return prevNodeOfSameActionClass;
        }

        private void WriteDataXml(XmlDocument xmlDoc, Action newAction, XmlNode prevAction, XmlNode actionsNode)
        {
            //Dichiarazione nodo azione
            XmlNode actionNode = null;
            //Dichiarazione tempo di inizio
            XmlNode startTime;
            //Dichiarazione tempo di fine
            XmlNode endTime;
            //Dichiarazione azione
            XmlNode action;
            //Classe di appartenenza azione
            XmlNode actionClass;

            //Aggiunta nodo singolo (azione)
            actionNode = xmlDoc.CreateElement("action");

            //Aggiunta nodo tempo di inizio azione (start time)
            startTime = xmlDoc.CreateElement("time_start");
            if (SameAction(prevAction, newAction))
            {
                startTime.InnerText = prevAction.SelectSingleNode("time_start").InnerText;
            }
            else
            {
                startTime.InnerText = newAction.StartTime.ToString(format_date_xml);
            }

            //Aggiunta nodo tempo di fine azione (end time)
            endTime = xmlDoc.CreateElement("time_end");
            endTime.InnerText = newAction.EndTime.ToString(format_date_xml);

            //Aggiunta nodo azione
            action = xmlDoc.CreateElement("action");
            action.InnerText = newAction.CarriedAction;

            //Aggiunta classe di appartenenza azione
            actionClass = xmlDoc.CreateElement("actionClass");
            actionClass.InnerText = newAction.ActionClass.ToString();

            actionNode.AppendChild(startTime);
            actionNode.AppendChild(endTime);
            actionNode.AppendChild(action);
            actionNode.AppendChild(actionClass);

            if (prevAction != null)
            {
                //Aggiunta nodo impact a nodo principale in posizione corretta
                actionsNode.InsertAfter(actionNode, prevAction);

                if(SameAction(prevAction, newAction))
                {
                    actionsNode.RemoveChild(prevAction);
                }

            }//Aggiungi in testa dopo actions
            else
            {
                actionsNode.InsertBefore(actionNode, actionsNode.FirstChild);
            }
            xmlDoc.Save(xmlPathFile);
        }

        /** Controllo se si tratta della stessa azione confrontando solo i nomi. */
        private Boolean SameAction(XmlNode prevAction, Action newAction)
        {
            String namePrevAction;
            if (prevAction != null)
            {
                namePrevAction = prevAction.SelectSingleNode("action").InnerText;
                if (namePrevAction.Equals(newAction.CarriedAction))
                {
                    return true;
                }
            }
            return false;
        }

        private DateTime CorrectDateTime(XmlNode prevNode, Action tmpAction)
        {
            String stringEndTimePrevNode;
            String stringStartTimeActualNode;
            DateTime dateEndTimePrevNode;
            DateTime startTimeActualNode;
            String n = prevNode.SelectSingleNode("action").InnerText;
            stringEndTimePrevNode = prevNode.SelectSingleNode("time_end").InnerText;
            dateEndTimePrevNode = Convert.ToDateTime(stringEndTimePrevNode);
            stringEndTimePrevNode = dateEndTimePrevNode.ToString(format_date_xml);

            startTimeActualNode = Convert.ToDateTime(tmpAction.StartTime);
            stringStartTimeActualNode = startTimeActualNode.ToString(format_date_xml);

            if (!(stringStartTimeActualNode.Equals(stringEndTimePrevNode)))
            {
                return dateEndTimePrevNode;
            }
            return startTimeActualNode;
        }

        /** Ricerca duplicato all'interno del file xml. */
        private Boolean IsDuplicatedNode(Action action, XmlNodeList xmlNodeList)
        {
            if (!(xmlNodeList.Count == 0))
            {
                String tmpStartTime;
                String tmpEndTime;
                String tmpCarriedAction;
                ActionClass tmpActionClass;
                Action tmpAction;

                //Itero nodi e verifico duplicato
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    tmpStartTime = xmlNode.SelectSingleNode("time_start").InnerText;
                    tmpEndTime = xmlNode.SelectSingleNode("time_end").InnerText;
                    tmpCarriedAction = xmlNode.SelectSingleNode("action").InnerText;
                    tmpActionClass = xmlNode.SelectSingleNode("actionClass").InnerText.GetActionClassEnum();
                    tmpAction = new Action(Convert.ToDateTime(tmpStartTime), Convert.ToDateTime(tmpEndTime), tmpCarriedAction, tmpActionClass);
                    if (action.EqualsActionString(tmpAction))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /** Ricerca posizione corretta per inserire il nodo. */
        private XmlNode SearchCorrectNodePosition(Action action, XmlNodeList xmlNodeList)
        {
            //trasformazione data azione in formato data senza considerare i millisecondi
            String endTimeStringFormat = action.EndTime.ToString(format_date_xml);
            DateTime endTimeDateFormat = Convert.ToDateTime(endTimeStringFormat);

            XmlNode tmpImpact = null;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode tmpEndTime = xmlNode.SelectSingleNode("time_end");

                //Trasformazione in formato data
                DateTime tmpEndTimeDateFormat = Convert.ToDateTime(tmpEndTime.InnerText);

                //Appena trovo azione con data finale maggiore della data finale dell'azione da inserire, la restituisco
                if (tmpEndTimeDateFormat.CompareTo(endTimeDateFormat) <= 0)
                {
                    tmpImpact = xmlNode;
                }
                else
                {
                    break;
                }
            }
            return tmpImpact;
        }

        private Boolean SameActionClassExceptRotation(ActionClass prevActionClass, ActionClass actualActionClass)
        {
            if (actualActionClass == ActionClass.Rotation)
            {
                return false;
            }

            if (!(prevActionClass.ToString().Equals(actualActionClass.ToString())))
            {
                return false;
            }

            return true;
        }
    }
}
