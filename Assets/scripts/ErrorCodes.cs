using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorTypes
{
    ErrorBadFuse,
    ErrorBadPowerConnector,
    ErrorBadMonitor,
    ErrorBadPowerSwitch,
    ErrorBadPump,
    ErrorLowAirCanister,
    ErrorHighCarbonCanister,
    ErrorLowPump
}
public static class ErrorCodes
{
    private const float ErrorThresholdDurability = 0.60f;
    private const float BrokenThresholdDurability = 0.20f;
    private const float ErrorThresholdPressure = 0.20f;
    private const float ErrorThresholdDirt = 0.80f;
    private static bool _Printed;
    public static bool Printed
    {
        get { return _Printed; }
        set { _Printed = value; }
    }
    public static void ErrorCheck(ItemRegister itemRegister, out string OutputString , out List<ErrorTypes> ErrorList)
    {
        ErrorList = new List<ErrorTypes>();
        if (CheckForBadCompontent<MonitorController>()) { ErrorList.Add(ErrorTypes.ErrorBadMonitor); }
        if (CheckForBadCompontent<FuseController>()) { ErrorList.Add(ErrorTypes.ErrorBadFuse); }
        if (CheckForBadCompontent<PowerConnectorController>()) { ErrorList.Add(ErrorTypes.ErrorBadPowerConnector); }
        if (CheckForBadCompontent<PowerSwitchController>()) { ErrorList.Add(ErrorTypes.ErrorBadPowerSwitch); }
        if (CheckForBadCompontent<PumpController>()) { ErrorList.Add(ErrorTypes.ErrorBadPump); }
        if (CheckForLowCompontent<AirCanisterController>()) { ErrorList.Add(ErrorTypes.ErrorLowAirCanister); }
        if (CheckForLowCompontent<Co2CanisterController>()) { ErrorList.Add(ErrorTypes.ErrorHighCarbonCanister); }
        if (CheckForDirtyCompontent<AirFilterController>() || CheckForBadCompontent<PumpController>() || CheckForLowCompontent<Co2CanisterController>()) { ErrorList.Add(ErrorTypes.ErrorLowPump); }
        bool CheckForBadCompontent<T>()
        {
            if (itemRegister.HasObject<T>(out List<T> List))
            {
                foreach (T Object in List)
                {
                    if (Object is IDurable durableObject && durableObject.GetPercentDurability() < ErrorThresholdDurability)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CheckForLowCompontent<T>()
        {
            if (itemRegister.HasObject<T>(out List<T> List))
            {
                foreach (T Object in List)
                {
                    if (Object is ICapacity capacityObject && capacityObject.GetPercentPressure() < ErrorThresholdPressure)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        bool CheckForDirtyCompontent<T>()
        {
            if (itemRegister.HasObject<T>(out List<T> List))
            {
                foreach (T Object in List)
                {
                    if (Object is IDirt dirtObject && dirtObject.GetPercentDirt() > ErrorThresholdDirt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        OutputString = "";
        //Pressure Errors
        if (ErrorList.Contains(ErrorTypes.ErrorLowAirCanister))
        {
            if (!Printed) { OutputString += "CF13\n"; }
            else OutputString += "LowAirCanister\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorHighCarbonCanister))
        {
            if (!Printed) { OutputString += "CF14"; }
            else OutputString += "Full Carbon Canister\n";
        }
        //Durability Errors
        if (ErrorList.Contains(ErrorTypes.ErrorBadPowerConnector))
        {
            if (!Printed) { OutputString += "EDA1\n"; }
            else OutputString += "BadPowerConnector\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadFuse))
        {
            if(!Printed) { OutputString += "EDA2\n"; }
            else OutputString += "BadFuse\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadPowerSwitch))
        {
            if (!Printed) { OutputString += "EDA3\n"; }
            else OutputString += "BadPowerSwitch\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadPump))
        {
            if (!Printed) { OutputString += "EDA4\n"; }
            else OutputString += "BadPump\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorBadMonitor))
        {
            if (!Printed) { OutputString += "EDA5\n"; }
            else OutputString += "BadMonitor\n";
        }
        if (ErrorList.Contains(ErrorTypes.ErrorLowPump))
        {
            if (!Printed) { OutputString += "CFA0\n"; }
            else OutputString += "LowPump\n";
        }
    }
    public static bool CheckWorking(ObjectDirector objectIn)
    {
        bool HasWorking = true;
        //TODO: FIX THIS
        //if (objectIn.objectType == ObjectType.AirFilter)
        //{
        //    if (objectIn.Dirt >= objectIn.MaxDirt)
        //    {
        //        HasWorking = false;
        //    }
        //}
        //else if (objectIn.objectType == ObjectType.AirCanister || objectIn.objectType == ObjectType.Co2Canister)
        //{
        //    if (objectIn.Pressure <= 100)
        //    {
        //        HasWorking = false;
        //    }
        //}
        //else if (objectIn.Durability < BrokenThresholdDurability)
        //{
        //    HasWorking = false;
        //}
        return HasWorking;
    }
    public static bool CheckWorking(List<ObjectDirector> objectsIn)
    {
        bool HasWorking = true; 
        //TODO: FIX THIS
        //foreach(ObjectDirector Item in objectsIn)
        //{
        //    if (Item.objectType == ObjectType.AirFilter)
        //    {
        //        if (Item.Dirt <= Item.MaxDirt)
        //        {
        //            HasWorking = false;
        //        }
        //    }
        //    else if (Item.objectType == ObjectType.AirCanister || Item.objectType == ObjectType.Co2Canister)
        //    {
        //        if (Item.Pressure >= 100)
        //        {
        //            HasWorking = false;
        //        }
        //    }
        //    else if (Item.Durability < BrokenThresholdDurability)
        //    {
        //        HasWorking = false;
        //    }
        //}
        return HasWorking;
    }
    public static bool CheckWorking<T>(ItemRegister register)
    {
        if (register.HasObject<T>(out List<T> ListToTest))
        {
            bool HasWorking = true;
            //TODO: FIX THIS
            //foreach (ObjectDirector Item in ListToTest)
            //{
            //    if (Item.objectType == ObjectType.AirFilter)
            //    {
            //        if (Item.Dirt >= Item.MaxDirt)
            //        {
            //            HasWorking = false;
            //        }
            //    }
            //    else if (Item.objectType == ObjectType.AirCanister || Item.objectType == ObjectType.Co2Canister)
            //    {
            //        if (Item.Pressure <= 100)
            //        {
            //            HasWorking = false;
            //        }
            //    }
            //    else if (Item.Durability < BrokenThresholdDurability)
            //    {
            //        HasWorking = false;
            //    }
            //}
            return HasWorking;
        }
        return false;
    }
}
