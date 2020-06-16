using CSGOCheat.Helper;
using CSGOCheat.Helper_and_ImportDLL;
using System.Threading;

namespace CSGOCheat.Function
{
    class RadarHack
    {
        public static void ToRadarHack()
        {
            while (Variable.radarhack_is == true)
            {
                for (int i = 0; i < 64; i++)
                {
                    int entity = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwEntityList + i * Offsets.dwEntityListDistance);
                    if (entity != null) //если значение на том месте, где должна быть инфа о игроке, не пустое, то значение bSpotted = true
                    {
                        FindProcess.mem.Write(entity + netvars.m_bSpotted, true);
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
