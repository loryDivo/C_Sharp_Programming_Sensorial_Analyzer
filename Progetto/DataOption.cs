using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto
{ 
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum Sensor
    {
        Sensor1,
        Sensor2,
        Sensor3,
        Sensor4,
        Sensor5,
        none,
    }

    public static class SensorMethods
    {
        public static int GetSensorNumber(this Sensor s)
        {
            switch (s)
            {
                case Sensor.Sensor1:
                    {
                        return 0;
                    }
                case Sensor.Sensor2:
                    {
                        return 1;
                    }
                case Sensor.Sensor3:
                    {
                        return 2;
                    }
                case Sensor.Sensor4:
                    {
                        return 3;
                    }
                case Sensor.Sensor5:
                    {
                        return 4;
                    }
            }

            return -1;
        }
    }

    public enum DataInfo
    {
        Acc,
        Gir,
        Magn,
        Quatern,
        none,
        StandLaySit,
    }

    public enum ActionClass
    {
        Rotation,
        Motion,
        SitStandLay,
        none,
    }

    public static class ActionClassMethods
    {
        
        public static ActionClass GetActionClassEnum(this String a)
        {
            if (a.Equals("Motion"))
            {
                return ActionClass.Motion;
            }
            else if (a.Equals("SitStandLay"))
            {
                return ActionClass.SitStandLay;
            }
            else if (a.Equals("Rotation"))
            {
                return ActionClass.Rotation;
            }
            return ActionClass.none;
        }

    }


    public static class DataInfoMethods
    {
        /** Ritorna l'indice dell'array di quel campo. */
        public static int GetInfoNumber(this DataInfo d, Axis axis)
        {
            switch (d)
            {
                case DataInfo.Acc:
                    {
                        if (axis == Axis.X)
                            return 0;
                        else if (axis == Axis.Y)
                            return 1;
                        else
                            return 2;
                    }
                case DataInfo.Gir:
                    {
                        if (axis == Axis.X)
                            return 3;
                        else if (axis == Axis.Y)
                            return 4;
                        else
                            return 5;
                    }
                case DataInfo.Magn:
                    {
                        if (axis == Axis.X)
                            return 6;
                        else if (axis == Axis.Y)
                            return 7;
                        else
                            return 8;
                    }
            }

            return -1;
        }
    }
}
