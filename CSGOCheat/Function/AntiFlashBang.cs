using CSGOCheat.Helper;
using CSGOCheat.Helper_and_ImportDLL;
using System.Threading;

namespace CSGOCheat.Function
{
    class AntiFlashBang
    {
        public static void ToAntiFlash()
        {
            int Player = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwLocalPlayer); //находим localplayer
            int Flash; //в Cheat Engine в смещениях можете ввести flashmaxAlpha и найдёте значение
            while (Variable.antiflash_is == true)
            {
                Flash = FindProcess.mem.Read<int>(Player + netvars.m_flFlashMaxAlpha);
                if (Flash > 1)
                {
                    FindProcess.mem.Write<int>(Player + netvars.m_flFlashMaxAlpha, 0);
                }
                Thread.Sleep(1);
            }
            FindProcess.mem.Write<int>(Player + netvars.m_flFlashMaxAlpha, Variable.backupflash);
            //проверка ниже на случай, если не получилось нормально присвоить значение предыдущему и оно всё ещё равно 0
            if ((Player + netvars.m_flFlashMaxAlpha) == 0)
                FindProcess.mem.Write<int>(Player + netvars.m_flFlashMaxAlpha, 1132396544);
        }
    }
}
