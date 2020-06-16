using CSGOCheat.Helper;
using CSGOCheat.Helper_and_ImportDLL;
using System.Threading;

namespace CSGOCheat.Function
{
    class FOVHack
    {
        public static void FOVChanger()
        {
            int namedurself = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwLocalPlayer);
            while (Variable.fovhack_is == 1)
            {
                int isScoped = FindProcess.mem.Read<int>(namedurself + netvars.m_bIsScoped);
                if (isScoped == 0) //проверка на то, находится ли игрок в прицеле
                    FindProcess.mem.Write(namedurself + netvars.m_iFOV, Variable.fov);
                Thread.Sleep(1);
            }
        }

        //Я глубоко в душе не знаю, как обратно возвращать значение 90 и не влезать в цикл, чтоб визуально оно выглядело, как 90
        public static void FOVBackup()
        {
            int namedurself = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwLocalPlayer);
            int CurrentFOV = FindProcess.mem.Read<int>(namedurself + netvars.m_iFOV);
            while (Variable.fovhack_is == 2)
            {
                if (FindProcess.mem.Read<int>(namedurself + netvars.m_iFOV) != Variable.backupfov)
                {
                    FindProcess.mem.Write(namedurself + netvars.m_iFOV, Variable.backupfov);
                }

                int isScoped = FindProcess.mem.Read<int>(namedurself + netvars.m_bIsScoped);
                if (isScoped == 1)
                    FindProcess.mem.Write(namedurself + netvars.m_iFOV, 40);
                else
                    FindProcess.mem.Write(namedurself + netvars.m_iFOV, Variable.backupfov);
                if (FindProcess.mem.Read<int>(namedurself + netvars.m_iFOV) == 90)
                    Variable.fovhack_is = 0;

                Thread.Sleep(1);
            }
        }
    }
}
