﻿using System;

namespace CSGOCheat.Helper
{
    public struct Offsets
    {
        public const Int32 dwEntityListDistance = 0x10; //шаг (в Cheat Engine подписано, как смещение), через который встречаются игроки
    }
    //ниже просто пустые переменные, которые могут пригодиться. Они заполняются благодаря OffsetsUpdater.cs
    public static class netvars
    {
        public static Int32 cs_gamerules_data;
        public static Int32 m_ArmorValue;
        public static Int32 m_Collision;
        public static Int32 m_CollisionGroup;
        public static Int32 m_Local;
        public static Int32 m_MoveType;
        public static Int32 m_OriginalOwnerXuidHigh;
        public static Int32 m_OriginalOwnerXuidLow;
        public static Int32 m_SurvivalGameRuleDecisionTypes;
        public static Int32 m_SurvivalRules;
        public static Int32 m_aimPunchAngle;
        public static Int32 m_aimPunchAngleVel;
        public static Int32 m_angEyeAnglesX;
        public static Int32 m_angEyeAnglesY;
        public static Int32 m_bBombPlanted;
        public static Int32 m_bFreezePeriod;
        public static Int32 m_bGunGameImmunity;
        public static Int32 m_bHasDefuser;
        public static Int32 m_bHasHelmet;
        public static Int32 m_bInReload;
        public static Int32 m_bIsDefusing;
        public static Int32 m_bIsQueuedMatchmaking;
        public static Int32 m_bIsScoped;
        public static Int32 m_bIsValveDS;
        public static Int32 m_bSpotted;
        public static Int32 m_bSpottedByMask;
        public static Int32 m_bStartedArming;
        public static Int32 m_clrRender;
        public static Int32 m_dwBoneMatrix;
        public static Int32 m_fAccuracyPenalty;
        public static Int32 m_fFlags;
        public static Int32 m_flC4Blow;
        public static Int32 m_flDefuseCountDown;
        public static Int32 m_flDefuseLength;
        public static Int32 m_flFallbackWear;
        public static Int32 m_flFlashDuration;
        public static Int32 m_flFlashMaxAlpha;
        public static Int32 m_flLastBoneSetupTime;
        public static Int32 m_flLowerBodyYawTarget;
        public static Int32 m_flNextAttack;
        public static Int32 m_flNextPrimaryAttack;
        public static Int32 m_flSimulationTime;
        public static Int32 m_flTimerLength;
        public static Int32 m_hActiveWeapon;
        public static Int32 m_hMyWeapons;
        public static Int32 m_hObserverTarget;
        public static Int32 m_hOwner;
        public static Int32 m_hOwnerEntity;
        public static Int32 m_iAccountID;
        public static Int32 m_iClip1;
        public static Int32 m_iCompetitiveRanking;
        public static Int32 m_iCompetitiveWins;
        public static Int32 m_iCrosshairId;
        public static Int32 m_iEntityQuality;
        public static Int32 m_iFOV;
        public static Int32 m_iFOVStart;
        public static Int32 m_iGlowIndex;
        public static Int32 m_iHealth;
        public static Int32 m_iItemDefinitionIndex;
        public static Int32 m_iItemIDHigh;
        public static Int32 m_iMostRecentModelBoneCounter;
        public static Int32 m_iObserverMode;
        public static Int32 m_iShotsFired;
        public static Int32 m_iState;
        public static Int32 m_iTeamNum;
        public static Int32 m_lifeState;
        public static Int32 m_nFallbackPaintKit;
        public static Int32 m_nFallbackSeed;
        public static Int32 m_nFallbackStatTrak;
        public static Int32 m_nForceBone;
        public static Int32 m_nTickBase;
        public static Int32 m_rgflCoordinateFrame;
        public static Int32 m_szCustomName;
        public static Int32 m_szLastPlaceName;
        public static Int32 m_thirdPersonViewAngles;
        public static Int32 m_vecOrigin;
        public static Int32 m_vecVelocity;
        public static Int32 m_vecViewOffset;
        public static Int32 m_viewPunchAngle;
    }
    public static class signatures
    {
        public static Int32 clientstate_choked_commands;
        public static Int32 clientstate_delta_ticks;
        public static Int32 clientstate_last_outgoing_command;
        public static Int32 clientstate_net_channel;
        public static Int32 convar_name_hash_table;
        public static Int32 dwClientState;
        public static Int32 dwClientState_GetLocalPlayer;
        public static Int32 dwClientState_IsHLTV;
        public static Int32 dwClientState_Map;
        public static Int32 dwClientState_MapDirectory;
        public static Int32 dwClientState_MaxPlayer;
        public static Int32 dwClientState_PlayerInfo;
        public static Int32 dwClientState_State;
        public static Int32 dwClientState_ViewAngles;
        public static Int32 dwEntityList;
        public static Int32 dwForceAttack;
        public static Int32 dwForceAttack2;
        public static Int32 dwForceBackward;
        public static Int32 dwForceForward;
        public static Int32 dwForceJump;
        public static Int32 dwForceLeft;
        public static Int32 dwForceRight;
        public static Int32 dwGameDir;
        public static Int32 dwGetAllClasses;
        public static Int32 dwGlobalVars;
        public static Int32 dwGlowObjectManager;
        public static Int32 dwInput;
        public static Int32 dwInterfaceLinkList;
        public static Int32 dwLocalPlayer;
        public static Int32 dwMouseEnable;
        public static Int32 dwMouseEnablePtr;
        public static Int32 dwPlayerResource;
        public static Int32 dwRadarBase;
        public static Int32 dwSensitivity;
        public static Int32 dwSensitivityPtr;
        public static Int32 dwSetClanTag;
        public static Int32 dwViewMatrix;
        public static Int32 dwWeaponTable;
        public static Int32 dwWeaponTableIndex;
        public static Int32 dwYawPtr;
        public static Int32 dwZoomSensitivityRatioPtr;
        public static Int32 dwbSendPackets;
        public static Int32 dwppDirect3DDevice9;
        public static Int32 force_update_spectator_glow;
        public static Int32 interface_engine_cvar;
        public static Int32 is_c4_owner;
        public static Int32 m_bDormant;
        public static Int32 m_pStudioHdr;
        public static Int32 m_pitchClassPtr;
        public static Int32 m_yawClassPtr;
        public static Int32 model_ambient_min;
        public static Int32 set_abs_angles;
        public static Int32 set_abs_origin;
    }
}
