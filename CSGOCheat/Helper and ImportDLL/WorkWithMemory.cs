using CSGOCheat.Helper_and_ImportDLL;
using CSGOCheat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CSGOCheat.Helper
{
    class FindProcess
    {
        public static Memory mem;
        public static int client_dll;
        public static int engine_dll;

        #region CheckCSGO (если NN выходит из игры, выключение чита)
        public static void Check()
        {
            while (Variable.check == true)
            {
                try
                {
                    Process csgo = Process.GetProcessesByName("csgo")[0];
                    mem = new Memory("csgo");
                    foreach (ProcessModule module in csgo.Modules)
                    {
                        if (module.ModuleName == "client_panorama.dll")
                            client_dll = (int)module.BaseAddress;
                    }
                    foreach (ProcessModule processModule in csgo.Modules)
                    {
                        if (processModule.ModuleName == "engine.dll")
                        {
                            engine_dll = processModule.BaseAddress.ToInt32();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch
                {
                    Thread.Sleep(1000);
                    Variable.check = false;

                    try
                    {
                        Form1._overlay.Close();
                        Form1._overlayStatus = false;
                    }
                    catch { }

                    Process[] listprosecc = Process.GetProcesses();
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName(Variable.CheatName))
                            proc.Kill();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion

        #region Проверка существования процесса игры и её dll
        public static bool GetDll() //подключение к csgo
        {
            try
            {
                Process csgo = Process.GetProcessesByName("csgo")[0];
                mem = new Memory("csgo");
                foreach (ProcessModule module in csgo.Modules)
                {
                    if (module.ModuleName == "client_panorama.dll")
                        client_dll = (int)module.BaseAddress;
                }
                foreach (ProcessModule processModule in csgo.Modules)
                {
                    if (processModule.ModuleName == "engine.dll")
                    {
                        engine_dll = processModule.BaseAddress.ToInt32();
                    }
                }
                return true;
            }
            catch
            {
                Thread.Sleep(500);
                return false;
            }
        }
        #endregion
    }

    public class MemoryWork
    {
        private Process _gameProcess;
        private IntPtr _gameHandle;
        private Int32 _clientModule;

        public int ClientModule { get => _clientModule; private set => _clientModule = value; }

        public MemoryWork()
        {
            _gameProcess = GetGameProcess();
            _gameHandle = GetGameHandle(_gameProcess);
            _clientModule = GetClientModule(_gameProcess);
        }

        ~MemoryWork()
        {
            Kernel32.CloseHandle(_gameHandle);
        }

        #region Получаем playerList (их координаты; хп; координаты головы; то, куда они смотрят; то, какой команде принадлежат)
        public List<PlayerModel> GetPlayer()
        {
            List<PlayerModel> playerList = new List<PlayerModel>();

            for (int i = 0; i < 64; i++)
            {
                Int32 playerBase = ReadInt32(_clientModule + signatures.dwEntityList + (i * Offsets.dwEntityListDistance));

                if (playerBase == 0)
                    continue;

                float playerX = ReadFloat(playerBase + netvars.m_vecOrigin + (0 * 0x4));
                float playerY = ReadFloat(playerBase + netvars.m_vecOrigin + (1 * 0x4));
                float playerZ = ReadFloat(playerBase + netvars.m_vecOrigin + (2 * 0x4));

                float playerHeadX = playerX + ReadFloat(playerBase + netvars.m_vecViewOffset + (0 * 0x4));
                float playerHeadY = playerY + ReadFloat(playerBase + netvars.m_vecViewOffset + (1 * 0x4));
                float playerHeadZ = playerZ + ReadFloat(playerBase + netvars.m_vecViewOffset + (2 * 0x4));

                Int32 playerHealth = ReadInt32(playerBase + netvars.m_iHealth);

                Int32 playerTeam = ReadInt32(playerBase + netvars.m_iTeamNum);

                PlayerModel currentPlayer = new PlayerModel
                {
                    PosX = playerX,
                    PosY = playerY,
                    PosZ = playerZ,
                    HeadX = playerHeadX,
                    HeadY = playerHeadY,
                    HeadZ = playerHeadZ,
                    Health = playerHealth,
                    Team = playerTeam
                };
                playerList.Add(currentPlayer);
            }
            return playerList;
        }
        #endregion

        //я правда не знаю, чем отличаются две эти переменные, которые по итогу выходят. Если знаете, отпишите в ВК или под постом
        public float ReadFloat(Int32 offsetPointer)
        {
            byte[] buffer = new byte[sizeof(float)];
            Kernel32.ReadProcessMemory(_gameHandle, offsetPointer, buffer, sizeof(float), out int bytesRead);
            return BitConverter.ToSingle(buffer, 0);
        }
        public Int32 ReadInt32(Int32 offsetPointer)
        {
            byte[] buffer = new byte[sizeof(Int32)];
            Kernel32.ReadProcessMemory(_gameHandle, offsetPointer, buffer, sizeof(Int32), out int bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        #region Получение процесса игры
        private Process GetGameProcess()
        {
            Process[] processList = Process.GetProcessesByName("csgo");
            if (processList.Length < 0)
            {
                MessageBox.Show("Could not find the game process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return processList[0];
        }
        #endregion

        #region Дескриптор (хорошая статья на stackoverflow - https://cutt.ly/mykKJyN (сокращено с помощью сайта Cutt.ly))
        private IntPtr GetGameHandle(Process gameProcess)
        {
            return Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ, false, gameProcess.Id);
        }
        #endregion

        #region Получение cliend_panorama.dll
        private Int32 GetClientModule(Process gameProcess)
        {
            foreach (ProcessModule processModule in gameProcess.Modules)
            {
                if (processModule.ModuleName == "client_panorama.dll")
                    return processModule.BaseAddress.ToInt32();
            }
            MessageBox.Show("Could not find the game module", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 0;
        }
        #endregion
    }
}
