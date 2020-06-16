using CSGOCheat.Helper;
using CSGOCheat.Helper_and_ImportDLL;
using System.Threading;

namespace CSGOCheat.Function
{
    class GlowHack
    {
        public static void ToGlow() //glow
        {
            while (Variable.glow_is == true)
            {
                int LocalPlayer = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwLocalPlayer);
                int PlayerTeam = FindProcess.mem.Read<int>(LocalPlayer + netvars.m_iTeamNum);
                for (int i = 0; i < 64; i++)
                {
                    int EntityList = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwEntityList + i * Offsets.dwEntityListDistance);
                    int EntityTeam = FindProcess.mem.Read<int>(EntityList + netvars.m_iTeamNum);
                    if (EntityTeam != 0 && EntityTeam != PlayerTeam)
                    {
                        int GlowIndex = FindProcess.mem.Read<int>(EntityList + netvars.m_iGlowIndex);
                        float HP = FindProcess.mem.Read<int>(EntityList + netvars.m_iHealth);
                        DrawEntityHP(GlowIndex, HP);
                    }
                }
                Thread.Sleep(Variable.Glow_Update);
            }
        }
        public static void DrawEntityHP(int GlowIndex, float HP) //рисуем обводку в зависимости от хп
        {
            int GlowObject = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwGlowObjectManager);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 4, 1 - HP / 100f);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 8, HP / 100f);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 12, 0 / 100f);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 0x10, 255 / 100f);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 0x24, true);
            FindProcess.mem.Write(GlowObject + (GlowIndex * 0x38) + 0x25, false);
        }
    }
}
