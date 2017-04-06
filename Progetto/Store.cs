/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Progetto
{
    public class Store
    {
        private List<List<List<double>>> buffer = new List<List<List<double>>>();
        public Store()
        {
        }

        public void Set(List<List<List<double>>> tmp)
        {
            Monitor.Enter(buffer);
            buffer.Clear();

            for (int i = 0; i < tmp.Count; i++)
            {
                buffer.Add(new List<List<double>>());
                for (int j = 0; j < 5; j++)
                {
                    buffer[i].Add(new List<double>());
                    for (int tr=0; tr < 9; tr++)
                    {
                        buffer[i][j].Add(tmp[i][j][tr]);
                        
                    }
                }
            }

            Monitor.Pulse(buffer);
            Monitor.Exit(buffer);
        }
        public void InsertIntoBuffer(List<List<double>> array)
        {
            Monitor.Enter(buffer);
            buffer.Add(array);                      //Aggiunta campione i-esimo al buffer
            Monitor.Pulse(buffer);
            Monitor.Exit(buffer);
        }

        public List<List<List<double>>> RetrieveData()
        {
            Monitor.Enter(buffer);
            return buffer;
        }

        public void ReleaseMonitor()
        {
            Monitor.Pulse(buffer);
            Monitor.Exit(buffer);
        }

        public int GetCount()
        {
            Monitor.Enter(buffer);
            int i = buffer.Count; 
            Monitor.Pulse(buffer);
            Monitor.Exit(buffer);

            return i;
        }

        public void Remove(int start, int end)
        {
            Monitor.Enter(buffer);
            buffer.RemoveRange(start, end);  
            Monitor.Pulse(buffer);
            Monitor.Exit(buffer);
        }
    }
}
*/