using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto
{
    public class Action
    {
        /** Data inizio azione */
        private DateTime startTime;
        /** Data fine azione */
        private DateTime endTime;
        /** Azione compiuta */
        private String carriedAction;
        /** Classe di azione di appartenenza */
        private ActionClass actionClass;

        public static String format_date_xml = "HH:mm:ss";

        public Action(DateTime startTime, DateTime endTime, String action, ActionClass actionClass)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.CarriedAction = action;
            this.ActionClass = actionClass;
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
            }
        }

        public string CarriedAction
        {
            get
            {
                return carriedAction;
            }

            set
            {
                carriedAction = value;
            }
        }

        public ActionClass ActionClass
        {
            get
            {
                return actionClass;
            }

            set
            {
                actionClass = value;
            }
        }

        /** Metodo usato per eseguire un minimo di reasoning nel file di output.
         * Controlla se due azioni sono le stesse, escludendo le rotazioni.
         */
        public Boolean EqualsActionString(Action other)
        {
            if (!this.endTime.ToString(format_date_xml).Equals(other.endTime.ToString(format_date_xml)))
            {
                return false;
            }

            if (this.actionClass != other.actionClass)
            {
                return false;
            }

            if (this.actionClass != ActionClass.Rotation && !this.carriedAction.Equals(other.carriedAction))
            {
                return false;
            }

            return true;
        }
    }
}
