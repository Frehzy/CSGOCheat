using System;
using CSGOCheat.Function;
using CSGOCheat.Helper;
using CSGOCheat.Helper_and_ImportDLL;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Tulpep.NotificationWindow;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace CSGOCheat
{
    public partial class Form1 : Form
    {
        #region Передвижение формы
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        public static bool _overlayStatus;
        public static Overlay _overlay;
        GlobalKeyboardHook gHook;

        public static PopupNotifier notifier = null;

        #region Form Initialize
        public Form1()
        {
            InitializeComponent();
            //если процесс игры не найден
            while (!FindProcess.GetDll())
            {
                DialogResult ifnotcsgo = MessageBox.Show("Game process not found. Continue?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (ifnotcsgo == DialogResult.No)
                {
                    try { Environment.Exit(0); }
                    catch { Application.Exit(); }
                }
            }

            tabPageAdv1.TabVisible = false;

            //checkcsgo
            Thread checkcs = new Thread(FindProcess.Check);
            Variable.check = true;
            checkcs.Start();

            //уведомление в начале
            string notif_text = "Press 'Insert' to enable Cheat";
            Notifier.Notif(notif_text);

            //распаковка hazedumper и config.json
            string temp_hazedumper = Application.StartupPath + "/hazedumper.exe";
            if (File.Exists(temp_hazedumper))
                File.Delete(temp_hazedumper);
            File.WriteAllBytes(temp_hazedumper, Properties.Resources.hazedumper);
            string temp_config = Application.StartupPath + "/config.json";
            if (File.Exists(temp_config))
                File.Delete(temp_config);
            File.WriteAllText(temp_config, Properties.Resources.config);

            _overlayStatus = false;

            OffsetsUpdater.DeleteFile();
            OffsetsUpdater.GetOffsets();

            //DEBUG
            try
            {
                string pathToFile = Application.StartupPath + $"/csgo.cs";
                // Считываем строки в массив
                string[] allLines = File.ReadAllLines(pathToFile, Encoding.GetEncoding(1251));
                // Добавляем каждую строку
                Offsets_csgo_cs.Items.Add("Information from the file: csgo.cs");
                foreach (string line in allLines)
                    Offsets_csgo_cs.Items.Add(line);

                string pathToLog = Application.StartupPath + $"/hazedumper.log";
                string[] allLinesLog = File.ReadAllLines(pathToLog, Encoding.GetEncoding(1251));
                DebugLog.Items.Add("Information from the file: hazedumper.log");
                foreach (string lineLog in allLinesLog)
                    DebugLog.Items.Add(lineLog);
            }
            catch (Exception ex)
            {
                Offsets_csgo_cs.Items.Add("Failed to load offsets");
                Offsets_csgo_cs.Items.Add(ex);
                Offsets_csgo_process.Items.Add("Failed to load offsets");
                Offsets_csgo_process.Items.Add(ex);
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //проверка оффсетов на их роботоспособность
            bool isOkay = true;
            string keyWord = "WARN";
            var lines = File.ReadLines(Application.StartupPath + $"/hazedumper.log");
            string result = string.Join("\n", lines.Where(s => s.IndexOf(keyWord, StringComparison.InvariantCultureIgnoreCase) >= 0));
            if (result.Length != 0)
            {
                MessageBox.Show(result, "Offsets are not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isOkay = false;
            }

            OffsetsUpdater.Update();
            OffsetsUpdater.DeleteFile();
            if (File.Exists(temp_hazedumper))
                File.Delete(temp_hazedumper);
            if (File.Exists(temp_config))
                File.Delete(temp_config);

            if (isOkay == false)
            {
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


            // СТРАШНО

            Offsets_csgo_process.Items.Add("Information from the cheat");
            #region Заполнение Offsets_csgo_process
            Offsets_csgo_process.Items.Add("public const Int32 dwEntityListDistance = 0x" + Offsets.dwEntityListDistance.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "cs_gamerules_data " + "= 0x" + netvars.cs_gamerules_data.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_ArmorValue " + "= 0x" + netvars.m_ArmorValue.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_Collision " + "= 0x" + netvars.m_Collision.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_CollisionGroup " + "= 0x" + netvars.m_CollisionGroup.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_Local " + "= 0x" + netvars.m_Local.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_MoveType " + "= 0x" + netvars.m_MoveType.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_OriginalOwnerXuidHigh " + "= 0x" + netvars.m_OriginalOwnerXuidHigh.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_OriginalOwnerXuidLow " + "= 0x" + netvars.m_OriginalOwnerXuidLow.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_SurvivalGameRuleDecisionTypes " + "= 0x" + netvars.m_SurvivalGameRuleDecisionTypes.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_SurvivalRules " + "= 0x" + netvars.m_SurvivalRules.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_aimPunchAngle " + "= 0x" + netvars.m_aimPunchAngle.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_aimPunchAngleVel " + "= 0x" + netvars.m_aimPunchAngleVel.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_angEyeAnglesX " + "= 0x" + netvars.m_angEyeAnglesX.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_angEyeAnglesY " + "= 0x" + netvars.m_angEyeAnglesY.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bBombPlanted " + "= 0x" + netvars.m_bBombPlanted.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bFreezePeriod " + "= 0x" + netvars.m_bFreezePeriod.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bGunGameImmunity " + "= 0x" + netvars.m_bGunGameImmunity.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bHasDefuser " + "= 0x" + netvars.m_bHasDefuser.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bHasHelmet " + "= 0x" + netvars.m_bHasHelmet.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bInReload " + "= 0x" + netvars.m_bInReload.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bIsDefusing " + "= 0x" + netvars.m_bIsDefusing.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bIsQueuedMatchmaking " + "= 0x" + netvars.m_bIsQueuedMatchmaking.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bIsScoped " + "= 0x" + netvars.m_bIsScoped.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bIsValveDS " + "= 0x" + netvars.m_bIsValveDS.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bSpotted " + "= 0x" + netvars.m_bSpotted.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bSpottedByMask " + "= 0x" + netvars.m_bSpottedByMask.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bStartedArming " + "= 0x" + netvars.m_bStartedArming.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_clrRender " + "= 0x" + netvars.m_clrRender.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_dwBoneMatrix " + "= 0x" + netvars.m_dwBoneMatrix.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_fAccuracyPenalty " + "= 0x" + netvars.m_fAccuracyPenalty.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_fFlags " + "= 0x" + netvars.m_fFlags.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flC4Blow " + "= 0x" + netvars.m_flC4Blow.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flDefuseCountDown " + "= 0x" + netvars.m_flDefuseCountDown.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flDefuseLength " + "= 0x" + netvars.m_flDefuseLength.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flFallbackWear " + "= 0x" + netvars.m_flFallbackWear.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flFlashDuration " + "= 0x" + netvars.m_flFlashDuration.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flFlashMaxAlpha " + "= 0x" + netvars.m_flFlashMaxAlpha.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flLastBoneSetupTime " + "= 0x" + netvars.m_flLastBoneSetupTime.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flLowerBodyYawTarget " + "= 0x" + netvars.m_flLowerBodyYawTarget.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flNextAttack " + "= 0x" + netvars.m_flNextAttack.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flNextPrimaryAttack " + "= 0x" + netvars.m_flNextPrimaryAttack.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flSimulationTime " + "= 0x" + netvars.m_flSimulationTime.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_flTimerLength " + "= 0x" + netvars.m_flTimerLength.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_hActiveWeapon " + "= 0x" + netvars.m_hActiveWeapon.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_hMyWeapons " + "= 0x" + netvars.m_hMyWeapons.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_hObserverTarget " + "= 0x" + netvars.m_hObserverTarget.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_hOwner " + "= 0x" + netvars.m_hOwner.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_hOwnerEntity " + "= 0x" + netvars.m_hOwnerEntity.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iAccountID " + "= 0x" + netvars.m_iAccountID.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iClip1 " + "= 0x" + netvars.m_iClip1.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iCompetitiveRanking " + "= 0x" + netvars.m_iCompetitiveRanking.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iCompetitiveWins " + "= 0x" + netvars.m_iCompetitiveWins.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iCrosshairId " + "= 0x" + netvars.m_iCrosshairId.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iEntityQuality " + "= 0x" + netvars.m_iEntityQuality.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iFOV " + "= 0x" + netvars.m_iFOV.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iFOVStart " + "= 0x" + netvars.m_iFOVStart.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iGlowIndex " + "= 0x" + netvars.m_iGlowIndex.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iHealth " + "= 0x" + netvars.m_iHealth.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iItemDefinitionIndex " + "= 0x" + netvars.m_iItemDefinitionIndex.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iItemIDHigh " + "= 0x" + netvars.m_iItemIDHigh.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iMostRecentModelBoneCounter " + "= 0x" + netvars.m_iMostRecentModelBoneCounter.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iObserverMode " + "= 0x" + netvars.m_iObserverMode.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iShotsFired " + "= 0x" + netvars.m_iShotsFired.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iState " + "= 0x" + netvars.m_iState.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_iTeamNum " + "= 0x" + netvars.m_iTeamNum.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_lifeState " + "= 0x" + netvars.m_lifeState.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_nFallbackPaintKit " + "= 0x" + netvars.m_nFallbackPaintKit.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_nFallbackSeed " + "= 0x" + netvars.m_nFallbackSeed.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_nFallbackStatTrak " + "= 0x" + netvars.m_nFallbackStatTrak.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_nForceBone " + "= 0x" + netvars.m_nForceBone.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_nTickBase " + "= 0x" + netvars.m_nTickBase.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_rgflCoordinateFrame " + "= 0x" + netvars.m_rgflCoordinateFrame.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_szCustomName " + "= 0x" + netvars.m_szCustomName.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_szLastPlaceName " + "= 0x" + netvars.m_szLastPlaceName.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_thirdPersonViewAngles " + "= 0x" + netvars.m_thirdPersonViewAngles.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_vecOrigin " + "= 0x" + netvars.m_vecOrigin.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_vecVelocity " + "= 0x" + netvars.m_vecVelocity.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_vecViewOffset " + "= 0x" + netvars.m_vecViewOffset.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_viewPunchAngle " + "= 0x" + netvars.m_viewPunchAngle.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "clientstate_choked_commands " + "= 0x" + signatures.clientstate_choked_commands.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "clientstate_delta_ticks " + "= 0x" + signatures.clientstate_delta_ticks.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "clientstate_last_outgoing_command " + "= 0x" + signatures.clientstate_last_outgoing_command.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "clientstate_net_channel " + "= 0x" + signatures.clientstate_net_channel.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "convar_name_hash_table " + "= 0x" + signatures.convar_name_hash_table.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState " + "= 0x" + signatures.dwClientState.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_GetLocalPlayer " + "= 0x" + signatures.dwClientState_GetLocalPlayer.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_IsHLTV " + "= 0x" + signatures.dwClientState_IsHLTV.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_Map " + "= 0x" + signatures.dwClientState_Map.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_MapDirectory " + "= 0x" + signatures.dwClientState_MapDirectory.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_MaxPlayer " + "= 0x" + signatures.dwClientState_MaxPlayer.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_PlayerInfo " + "= 0x" + signatures.dwClientState_PlayerInfo.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_State " + "= 0x" + signatures.dwClientState_State.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwClientState_ViewAngles " + "= 0x" + signatures.dwClientState_ViewAngles.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwEntityList " + "= 0x" + signatures.dwEntityList.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceAttack " + "= 0x" + signatures.dwForceAttack.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceAttack2 " + "= 0x" + signatures.dwForceAttack2.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceBackward " + "= 0x" + signatures.dwForceBackward.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceForward " + "= 0x" + signatures.dwForceForward.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceJump " + "= 0x" + signatures.dwForceJump.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceLeft " + "= 0x" + signatures.dwForceLeft.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwForceRight " + "= 0x" + signatures.dwForceRight.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwGameDir " + "= 0x" + signatures.dwGameDir.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwGetAllClasses " + "= 0x" + signatures.dwGetAllClasses.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwGlobalVars " + "= 0x" + signatures.dwGlobalVars.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwGlowObjectManager " + "= 0x" + signatures.dwGlowObjectManager.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwInput " + "= 0x" + signatures.dwInput.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwInterfaceLinkList " + "= 0x" + signatures.dwInterfaceLinkList.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwLocalPlayer " + "= 0x" + signatures.dwLocalPlayer.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwMouseEnable " + "= 0x" + signatures.dwMouseEnable.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwMouseEnablePtr " + "= 0x" + signatures.dwMouseEnablePtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwPlayerResource " + "= 0x" + signatures.dwPlayerResource.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwRadarBase " + "= 0x" + signatures.dwRadarBase.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwSensitivity " + "= 0x" + signatures.dwSensitivity.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwSensitivityPtr " + "= 0x" + signatures.dwSensitivityPtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwSetClanTag " + "= 0x" + signatures.dwSetClanTag.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwViewMatrix " + "= 0x" + signatures.dwViewMatrix.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwWeaponTable " + "= 0x" + signatures.dwWeaponTable.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwWeaponTableIndex " + "= 0x" + signatures.dwWeaponTableIndex.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwYawPtr " + "= 0x" + signatures.dwYawPtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwZoomSensitivityRatioPtr " + "= 0x" + signatures.dwZoomSensitivityRatioPtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwbSendPackets " + "= 0x" + signatures.dwbSendPackets.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "dwppDirect3DDevice9 " + "= 0x" + signatures.dwppDirect3DDevice9.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "force_update_spectator_glow " + "= 0x" + signatures.force_update_spectator_glow.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "interface_engine_cvar " + "= 0x" + signatures.interface_engine_cvar.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "is_c4_owner " + "= 0x" + signatures.is_c4_owner.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_bDormant " + "= 0x" + signatures.m_bDormant.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_pStudioHdr " + "= 0x" + signatures.m_pStudioHdr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_pitchClassPtr " + "= 0x" + signatures.m_pitchClassPtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "m_yawClassPtr " + "= 0x" + signatures.m_yawClassPtr.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "model_ambient_min " + "= 0x" + signatures.model_ambient_min.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "set_abs_angles " + "= 0x" + signatures.set_abs_angles.ToString("X"));
            Offsets_csgo_process.Items.Add("public static Int32 " + "set_abs_origin " + "= 0x" + signatures.set_abs_origin.ToString("X"));
            #endregion


            //отслеживание клавиши Insert
            gHook = new GlobalKeyboardHook();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gHook.HookedKeys.Add(key);
            gHook.hook();
        }
        #endregion

        #region Insert
        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Text += ((char)e.KeyValue).ToString();
            if (textBox1.Text == "-")
            {
                Variable.on_off = Variable.on_off + 1;
                if (Variable.on_off % 2 == 0)
                {
                    TopMost = false;
                    Visible = false;
                    Enabled = false;
                    ShowIcon = false;
                    ShowInTaskbar = false;
                    WindowState = FormWindowState.Minimized;
                }
                else
                {
                    TopMost = true;
                    Visible = true;
                    Enabled = true;
                    ShowIcon = true;
                    ShowInTaskbar = true;
                    WindowState = FormWindowState.Normal;
                    Activate();
                }
            }
            textBox1.Clear();
        }
        #endregion

        #region ESP
        private void enableESP_CheckedChanged(object sender, EventArgs e)
        {
            if (enableESP.Checked)
            {
                try
                {
                    if (!_overlayStatus)
                    {
                        _overlay = new Overlay();
                        _overlay.Show();
                        _overlayStatus = true;

                        ESP_BoostFPS_CheckBox.Enabled = true;
                        ESPThicknessTrackBar.Enabled = true;
                        ESPBrightnessTrackBar.Enabled = true;
                        redCheckBox.Enabled = true;
                        greenCheckBox.Enabled = true;
                        blueCheckBox.Enabled = true;
                        thicknessLabel.Enabled = true;
                        brightnessLabel.Enabled = true;
                        colorLabel.Enabled = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    _overlay.Close();
                    _overlayStatus = false;

                    ESP_BoostFPS_CheckBox.Enabled = false;
                    ESPThicknessTrackBar.Enabled = false;
                    ESPBrightnessTrackBar.Enabled = false;
                    redCheckBox.Enabled = false;
                    greenCheckBox.Enabled = false;
                    blueCheckBox.Enabled = false;
                    thicknessLabel.Enabled = false;
                    brightnessLabel.Enabled = false;
                    colorLabel.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("Something went wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ESP_BoostFPS_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ESP_BoostFPS_CheckBox.Checked)
            {
                Variable.ESPBoostFPS = 10;
            }
            else
            {
                Variable.ESPBoostFPS = 0;
            }
        }

        private void ESPThicknessTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            float value = (float)ESPThicknessTrackBar.Value / 10;
            thicknessLabel.Text = "Thickness: " + value;
            Variable.ESPThickness = value;
        }

        private void ESPBrightnessTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            float value = (float)ESPBrightnessTrackBar.Value / 10;
            brightnessLabel.Text = "Brightness: " + value;
            Variable.ESPBrightness = value;
        }

        private void redCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (redCheckBox.Checked)
            {
                Variable.Red = 255;
            }
            else
            {
                Variable.Red = 0;
            }
        }

        private void greenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (greenCheckBox.Checked)
            {
                Variable.Green = 255;
            }
            else
            {
                Variable.Green = 0;
            }
        }

        private void blueCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (blueCheckBox.Checked)
            {
                Variable.Blue = 255;
            }
            else
            {
                Variable.Blue = 0;
            }
        }
        #endregion

        #region Glow
        private void GlowChecked_CheckedChanged(object sender, EventArgs e)
        {
            if (GlowChecked.Checked)
            {
                Thread glow = new Thread(GlowHack.ToGlow);
                Variable.glow_is = true;
                glow.Start();

                GlowUpdate.Enabled = true;
                glowUpdLabel.Enabled = true;
            }
            else
            {
                Variable.glow_is = false;

                GlowUpdate.Enabled = false;
                glowUpdLabel.Enabled = false;
            }
        }

        private void GlowUpdate_Scroll(object sender, ScrollEventArgs e)
        {
            Int32 value = 251 - (Int32)GlowUpdate.Value;
            glowUpdLabel.Text = "GlowUpdate: " + value;
            Variable.Glow_Update = value;
        }
        #endregion

        #region FOV
        private void FOVCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (FOVCheck.Checked)
            {
                FOVTrackBar.Enabled = true;
                fovLabel.Enabled = true;

                Variable.fov = Convert.ToInt32((int)FOVTrackBar.Value);
                Thread fovhack = new Thread(FOVHack.FOVChanger);
                Variable.fovhack_is = 1;
                fovhack.Start();
            }
            else
            {
                FOVTrackBar.Enabled = false;
                fovLabel.Enabled = false;

                Thread fovhack_backup = new Thread(FOVHack.FOVBackup);
                Variable.fovhack_is = 2;
                Variable.fov = Variable.backupfov;
                fovhack_backup.Start();
            }
        }

        private void FOVTrackBar_Scroll(object sender, ScrollEventArgs e)
        {
            int value = (int)FOVTrackBar.Value;
            fovLabel.Text = "FOV: " + value;
            Variable.fov = value;
        }
        #endregion

        #region Bhop
        private void BunnyHopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BunnyHopCheckBox.Checked)
            {
                Thread bhop = new Thread(BhopHack.ToBunnyHop);
                Variable.bunnyhop_is = true;
                bhop.Start();
            }
            else
            {
                Variable.bunnyhop_is = false;
            }
        }
        #endregion

        #region Radar
        private void RadarHackCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (RadarHackCheck.Checked)
            {
                Thread radarhack = new Thread(RadarHack.ToRadarHack);
                Variable.radarhack_is = true;
                radarhack.Start();
            }
            else
            {
                Variable.radarhack_is = false;
            }
        }
        #endregion

        #region AntiFlash
        private void AntiFlashCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AntiFlashCheckBox.Checked)
            {
                int Player = FindProcess.mem.Read<int>(FindProcess.client_dll + signatures.dwLocalPlayer);
                int Flash = FindProcess.mem.Read<int>(Player + netvars.m_flFlashMaxAlpha);
                Variable.backupflash = Flash;

                Thread aflash = new Thread(AntiFlashBang.ToAntiFlash);
                Variable.antiflash_is = true;
                aflash.Start();
            }
            else
            {
                Variable.antiflash_is = false;
            }
        }
        #endregion

        #region FormLoad
        private void Form1_Load(object sender, EventArgs e)
        {
            ESP_BoostFPS_CheckBox.Enabled = false;
            ESPThicknessTrackBar.Enabled = false;
            ESPBrightnessTrackBar.Enabled = false;
            redCheckBox.Enabled = false;
            greenCheckBox.Enabled = false;
            blueCheckBox.Enabled = false;
            GlowUpdate.Enabled = false;
            FOVTrackBar.Enabled = false;
            thicknessLabel.Enabled = false;
            brightnessLabel.Enabled = false;
            colorLabel.Enabled = false;
            glowUpdLabel.Enabled = false;
            fovLabel.Enabled = false;
        }
        #endregion

        #region Exit button / Form closing
        private void EXIT_Click(object sender, EventArgs e)
        {
            try
            {
                _overlay.Close();
                _overlayStatus = false;
            }
            catch { }
            enableESP.Enabled = false;
            EXIT.Enabled = false;
            //выкл ESP
            ESPThicknessTrackBar.Visible = false;
            ESPThicknessTrackBar.Value = 5;
            thicknessLabel.Visible = false;
            thicknessLabel.Text = "ESPThickness: 0.5";
            ESPBrightnessTrackBar.Visible = false;
            ESPBrightnessTrackBar.Value = 5;
            brightnessLabel.Visible = false;
            brightnessLabel.Text = "ESPBrightness: 0.5";
            redCheckBox.Visible = false;
            redCheckBox.Checked = false;
            greenCheckBox.Visible = false;
            greenCheckBox.Checked = false;
            blueCheckBox.Visible = false;
            blueCheckBox.Checked = false;
            ESP_BoostFPS_CheckBox.Checked = false;
            ESP_BoostFPS_CheckBox.Visible = false;
            //esp
            GlowChecked.Checked = false;
            GlowChecked.Enabled = false;
            //radarhack
            RadarHackCheck.Checked = false;
            RadarHackCheck.Enabled = false;
            //bhop
            BunnyHopCheckBox.Checked = false;
            BunnyHopCheckBox.Enabled = false;
            //FOV
            FOVCheck.Checked = false;
            FOVCheck.Enabled = false;
            //AntiFlash
            AntiFlashCheckBox.Checked = false;
            AntiFlashCheckBox.Enabled = false;
            //отслеживание Insert
            gHook.unhook();
            //выход из всех окон
            Thread.Sleep(500);

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
        #endregion

        #region Передвижение формы
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        private void tabControlAdv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlAdv1.SelectedTab == tabPageAdv1)
            {
                MessageBox.Show("It is not for you", "Sorry ^_^", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControlAdv1.SelectedTab = aimbotPage;
            }
        }
    }
}
