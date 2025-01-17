﻿using Utilities.Attributes;

namespace FalconCampaign.Enums
{
    // Campaign Enums

    public enum AirActionType
    {
        AACTION_NOTHING = 0,
        AACTION_DCA = 1,
        AACTION_OCA = 2,
        AACTION_INTERDICT = 3,
        AACTION_ATTRITION = 4,
        AACTION_CAS = 5,
    };

    public enum AirTacticType
    {
        TAT_DEFENSIVE = 1,
        TAT_OFFENSIVE = 2,
        TAT_INTERDICT = 3,
        TAT_ATTRITION = 4,
        TAT_CAS = 5,		// CAS must always be last tactic
    };

    public enum AltLethality
    {
        HIGH_ALT_LETHALITY = 0,
        LOW_ALT_LETHALITY,
        NUM_ALT_LETHALITY
    }

    /* Bounding Box Class table Entries */
    public enum Bbox_Types
    {
        BBOX_NOTHING = 0,
        BBOX_120MMAP = 1,
        BBOX_275ROCK = 2,
        BBOX_2S19 = 3,
        BBOX_2S6 = 4,
        BBOX_370GAL = 5,
        BBOX_A10 = 6,
        BBOX_A37 = 7,
        BBOX_A50 = 8,
        BBOX_A6 = 9,
        BBOX_AA10 = 10,
        BBOX_AA10C = 11,
        BBOX_AA11 = 12,
        BBOX_AA12 = 13,
        BBOX_AA2 = 14,
        BBOX_AA2R = 15,
        BBOX_AA3 = 16,
        BBOX_AA7 = 17,
        BBOX_AA7R = 18,
        BBOX_AA8 = 19,
        BBOX_AA9 = 20,
        BBOX_AAV7A1 = 21,
        BBOX_AC130 = 22,
        BBOX_AGM114 = 23,
        BBOX_AGM119 = 24,
        BBOX_AGM122 = 25,
        BBOX_AGM45 = 26,
        BBOX_AGM65B = 27,
        BBOX_AGM65D = 28,
        BBOX_AGM65G = 29,
        BBOX_AGM78 = 30,
        BBOX_AGM84 = 31,
        BBOX_AGM88 = 32,
        BBOX_AH1 = 33,
        BBOX_AH64 = 34,
        BBOX_AH64D = 35,
        BBOX_AH66 = 36,
        BBOX_AIM120 = 37,
        BBOX_AIM54 = 38,
        BBOX_AIM7 = 39,
        BBOX_AIM9M = 40,
        BBOX_AIM9P = 41,
        BBOX_AIM9R = 42,
        BBOX_AKULA = 43,
        BBOX_ALQ131 = 44,
        BBOX_AN12 = 45,
        BBOX_AN124 = 46,
        BBOX_AN14 = 47,
        BBOX_AN2 = 48,
        BBOX_AN225 = 49,
        BBOX_AN24 = 50,
        BBOX_AN70 = 51,
        BBOX_AN72 = 52,
        BBOX_APT1 = 53,
        BBOX_AS10 = 54,
        BBOX_AS14 = 55,
        BBOX_AS15 = 56,
        BBOX_AS4 = 57,
        BBOX_AS6 = 58,
        BBOX_AS7 = 59,
        BBOX_AS9 = 60,
        BBOX_AT3 = 61,
        BBOX_B1 = 62,
        BBOX_B2 = 63,
        BBOX_B52 = 64,
        BBOX_B52G = 65,
        BBOX_BARRACKS = 66,
        BBOX_BM24 = 67,
        BBOX_BMP = 68,
        BBOX_BRDM2 = 69,
        BBOX_BRIDGE1 = 70,
        BBOX_BRIDGE2 = 71,
        BBOX_BRIDGE3 = 72,
        BBOX_BTR80 = 73,
        BBOX_BUILD1 = 74,
        BBOX_BUILD2 = 75,
        BBOX_BUNKER1 = 76,
        BBOX_C130 = 77,
        BBOX_C141 = 78,
        BBOX_C17 = 79,
        BBOX_C5 = 80,
        BBOX_CBU528 = 81,
        BBOX_CH46 = 82,
        BBOX_CH47 = 83,
        BBOX_CHAPARRAL = 84,
        BBOX_CITY_HALL = 85,
        BBOX_CNC1 = 86,
        BBOX_DELTA = 87,
        BBOX_DOCK1 = 88,
        BBOX_DURANDAL = 89,
        BBOX_E2C = 90,
        BBOX_E3 = 91,
        BBOX_E3BOOM = 92,
        BBOX_E8C = 93,
        BBOX_EA6B = 94,
        BBOX_EF111 = 95,
        BBOX_EF2000 = 96,
        BBOX_F117 = 97,
        BBOX_F14 = 98,
        BBOX_F15C = 99,
        BBOX_F15E = 100,
        BBOX_F16A = 101,
        BBOX_F16C = 102,
        BBOX_F18 = 103,
        BBOX_F18A = 104,
        BBOX_F18D = 105,
        BBOX_F18E = 106,
        BBOX_F22 = 107,
        BBOX_F4 = 108,
        BBOX_F4G = 109,
        BBOX_F5 = 110,
        BBOX_F5E = 111,
        BBOX_FAB250 = 112,
        BBOX_FACTORY1 = 113,
        BBOX_FACTORY2 = 114,
        BBOX_FB111 = 115,
        BBOX_FROG7 = 116,
        BBOX_FROG7L = 117,
        BBOX_FUELTANK2 = 118,
        BBOX_FUELTRUCK = 119,
        BBOX_GBU10 = 120,
        BBOX_GBU12 = 121,
        BBOX_GBU15 = 122,
        BBOX_GBU24B = 123,
        BBOX_HARRIER = 124,
        BBOX_HAWK = 125,
        BBOX_HAWKL = 126,
        BBOX_HELLFIRE = 127,
        BBOX_HIGHRISE1 = 128,
        BBOX_HMMWV = 129,
        BBOX_HMVAMB = 130,
        BBOX_HMVAVG = 131,
        BBOX_HMVCAR = 132,
        BBOX_HMVTOW = 133,
        BBOX_HOUSE1 = 134,
        BBOX_HOUSE2 = 135,
        BBOX_HOUSE3 = 136,
        BBOX_HOUSE4 = 137,
        BBOX_IL28 = 138,
        BBOX_IL76 = 139,
        BBOX_IL78 = 140,
        BBOX_J5 = 141,
        BBOX_J7 = 142,
        BBOX_J8 = 143,
        BBOX_KC10 = 144,
        BBOX_KC130 = 145,
        BBOX_KC135 = 146,
        BBOX_KH50 = 147,
        BBOX_KILO = 148,
        BBOX_LAU3A = 149,
        BBOX_LAV25 = 150,
        BBOX_LAVADV = 151,
        BBOX_LAVTOW = 152,
        BBOX_LOSANGELES = 153,
        BBOX_M110A2 = 154,
        BBOX_M113 = 155,
        BBOX_M163 = 156,
        BBOX_M1A1 = 157,
        BBOX_M2 = 158,
        BBOX_M2ADAT = 159,
        BBOX_M2ADV = 160,
        BBOX_M48 = 161,
        BBOX_M60 = 162,
        BBOX_M88 = 163,
        BBOX_M901 = 164,
        BBOX_M977C = 165,
        BBOX_M977F = 166,
        BBOX_M997 = 167,
        BBOX_M998 = 168,
        BBOX_MI24 = 169,
        BBOX_MI26 = 170,
        BBOX_MI8 = 171,
        BBOX_MIG19 = 172,
        BBOX_MIG21 = 173,
        BBOX_MIG23 = 174,
        BBOX_MIG25 = 175,
        BBOX_MIG27 = 176,
        BBOX_MIG29 = 177,
        BBOX_MIG31 = 178,
        BBOX_MINUTEMAN = 179,
        BBOX_MIRAGE = 180,
        BBOX_MK82 = 181,
        BBOX_MK84 = 182,
        BBOX_MLRS = 183,
        BBOX_MTLBU = 184,
        BBOX_MV22 = 185,
        BBOX_NEW1 = 186,
        BBOX_NEW2 = 187,
        BBOX_NEW3 = 188,
        BBOX_NEW4 = 189,
        BBOX_NONE = 190,
        BBOX_NUKE_PLANT = 191,
        BBOX_OFFICE1 = 192,
        BBOX_OFFICE2 = 193,
        BBOX_OH58 = 194,
        BBOX_OSCAR = 195,
        BBOX_OV10D = 196,
        BBOX_P3 = 197,
        BBOX_PATLAUNCH = 198,
        BBOX_PATRIOT = 199,
        BBOX_PEACEKEEPER = 200,
        BBOX_PT76 = 201,
        BBOX_Q5 = 202,
        BBOX_RAIL1 = 203,
        BBOX_RAPIER = 204,
        BBOX_RC135 = 205,
        BBOX_REFINERY1 = 206,
        BBOX_RTOWER = 207,
        BBOX_S_RADAR = 208,
        BBOX_S1 = 209,
        BBOX_S3 = 210,
        BBOX_SA10 = 211,
        BBOX_SA13 = 212,
        BBOX_SA13L = 213,
        BBOX_SA2 = 214,
        BBOX_SA2L = 215,
        BBOX_SA2R = 216,
        BBOX_SA3 = 217,
        BBOX_SA3L = 218,
        BBOX_SA3R = 219,
        BBOX_SA4 = 220,
        BBOX_SA4L = 221,
        BBOX_SA4R = 222,
        BBOX_SA5 = 223,
        BBOX_SA5L = 224,
        BBOX_SA5R = 225,
        BBOX_SA6 = 226,
        BBOX_SA6L = 227,
        BBOX_SA7 = 228,
        BBOX_SA8 = 229,
        BBOX_SA8L = 230,
        BBOX_SCUDL = 231,
        BBOX_SEAWOLF = 232,
        BBOX_SH3 = 233,
        BBOX_SINGLE_RACK = 234,
        BBOX_SMALL_CRATER1 = 235,
        BBOX_SR71 = 236,
        BBOX_SS1 = 237,
        BBOX_SS18 = 238,
        BBOX_SS21 = 239,
        BBOX_SS24 = 240,
        BBOX_SS45 = 241,
        BBOX_STINGER = 242,
        BBOX_SU15 = 243,
        BBOX_SU24 = 244,
        BBOX_SU25 = 245,
        BBOX_SU27 = 246,
        BBOX_SU7 = 247,
        BBOX_T62 = 248,
        BBOX_T72 = 249,
        BBOX_T80 = 250,
        BBOX_TAXI2 = 251,
        BBOX_TR1 = 252,
        BBOX_TR2 = 253,
        BBOX_TRIDENT = 254,
        BBOX_TRIPLE_RACK = 255,
        BBOX_TU16 = 256,
        BBOX_TU160 = 257,
        BBOX_TU16N = 258,
        BBOX_TU20 = 259,
        BBOX_TU22 = 260,
        BBOX_TU95 = 261,
        BBOX_TYPHOON = 262,
        BBOX_U2 = 263,
        BBOX_UH1H = 264,
        BBOX_UH60 = 265,
        BBOX_UH60L = 266,
        BBOX_WAREHOUSE1 = 267,
        BBOX_WAReHOUSE2 = 268,
        BBOX_Y12 = 269,
        BBOX_Y8 = 270,
        BBOX_ZIL135 = 271,
        BBOX_ZSU23_4 = 272,
        BBOX_ZSU30 = 273,
        BBOX_ZSU57 = 274,
        BBOX_M109 = 277,
        BBOX_GPU5 = 278,
        BBOX_GBU28B = 279,
        BBOX_FAB1000 = 280,
        BBOX_LNTRN = 281,
        BBOX_AGM130 = 282,
        BBOX_AT4 = 283,
        BBOX_SA14 = 284,
        BBOX_SA12 = 285,
        BBOX_CHAPMIS = 286,
        BBOX_HYDRA = 287,
        BBOX_UB1957 = 288,
        BBOX_PTK250 = 289,
        BBOX_RPK180 = 290,
        BBOX_RPK500 = 291,
        BBOX_TOWM1046 = 292,
        BBOX_M47 = 293,
        BBOX_AA8R = 294,
        BBOX_300GALW = 295,
        BBOX_300GAL = 296,
        BBOX_CHAFF = 297,
        BBOX_57MMROCK = 298,
        BBOX_M198 = 299,
        BBOX_KRAZ255B = 300,
        BBOX_APT4 = 301,
        BBOX_APT5 = 302,
        BBOX_HOUSE6 = 303,
        BBOX_HOUSE7 = 304,
        BBOX_HOUSE8 = 305,
        BBOX_BARN1 = 306,
        BBOX_BARN2 = 307,
        BBOX_HOUSE9 = 308,
        BBOX_HOUSE10 = 309,
        BBOX_HOUSE11 = 310,
        BBOX_TWNHALL1 = 311,
        BBOX_YARD1 = 312,
        BBOX_SHOP4 = 313,
        BBOX_SHOP5 = 314,
        BBOX_HANGAR9 = 315,
        BBOX_HANGAR10 = 316,
        BBOX_RUNWAY3 = 317,
        BBOX_RUNWAY4 = 318,
        BBOX_TEMPLE1 = 319,
        BBOX_SHOP6 = 320,
        BBOX_HOUSE5 = 321,
        BBOX_SHED2 = 322,
        BBOX_SHED3 = 323,
        BBOX_WTOWER2 = 324,
        BBOX_WTOWER3 = 325,
        BBOX_SHRINE1 = 326,
        BBOX_OFFICE7 = 327,
        BBOX_APARTMENT6 = 328,
        BBOX_RAMP = 329,
        BBOX_RAMP2 = 330,
        BBOX_STABLE1 = 331,
        BBOX_PARK1 = 332,
        BBOX_TREE2 = 333,
        BBOX_TREE3 = 334,
        BBOX_TREE4 = 335,
        BBOX_HOUSE12 = 336,
        BBOX_OFFICE3 = 337,
        BBOX_OFFICE4 = 338,
        BBOX_OFFICE5 = 339,
        BBOX_OFFICE6 = 340,
        BBOX_OFFICE8 = 341,
        BBOX_SKYSCRAPER1 = 342,
        BBOX_SKYSCRAPER2 = 343,
        BBOX_OFF_BLOCK = 344,
        BBOX_APARTMENT3 = 345,
        BBOX_CITY_HALL2 = 346,
        BBOX_BARRACKS2 = 347,
        BBOX_CHURCH2 = 348,
        BBOX_TEMPLE2 = 349,
        BBOX_RPG_7VAT = 350,
        BBOX_HN_5_A = 351,
        BBOX_ADATS = 352,
        BBOX_BLOWNUP = 353,
        BBOX_OFFICE9 = 354,
        BBOX_OFFICE10 = 355,
        BBOX_OFFICE11 = 356,
        BBOX_OFFICE12 = 357,
        BBOX_OFFICE13 = 358,
        BBOX_OFFICE14 = 359,
        BBOX_OFFICE15 = 360,
        BBOX_OFFICE16 = 361,
        BBOX_OFFICE17 = 362,
        BBOX_RAMP1 = 363,
        BBOX_LRAMP1 = 364,
        BBOX_LSPAN1 = 365,
        BBOX_LTOWER1 = 366,
        BBOX_LRAMP2 = 367,
        BBOX_LSPAN2 = 368,
        BBOX_TEST = 369,
        BBOX_OFF_BLOCK2 = 370,
        BBOX_OFF_BLOCK3 = 371,
        BBOX_OFF_BLOCK4 = 372,
        BBOX_OFF_BLOCK5 = 373,
        BBOX_OFF_BLOCK6 = 374,
        BBOX_OFF_BLOCK7 = 375,
        BBOX_OFF_BLOCK9 = 376,
        BBOX_WAREHOUSE3 = 377,
        BBOX_WAREHOUSE4 = 378,
        BBOX_TVSTN1 = 379,
        BBOX_HOTEL1 = 380,
        BBOX_SM_BRIDGE3 = 381,
        BBOX_MISS_LAUN = 382,
        BBOX_SAM_LAUN = 383,
        BBOX_MISS_FLAME = 384,
        BBOX_ENG_FIRE = 385,
        BBOX_SM_FACT1 = 386,
        BBOX_SM_FACT2 = 387,
        BBOX_SM_FACT3 = 388,
        BBOX_4KRUNWAY = 389,
        BBOX_FLARE1 = 390,
        BBOX_EXPLOSION1 = 391,
        BBOX_TANK_HULK = 392,
        BBOX_RUN2K = 393,
        BBOX_RN23K = 394,
        BBOX_RN2MID = 395,
        BBOX_RN2NUM = 396,
        BBOX_RN2THR = 397,
        BBOX_FFG = 398,
        BBOX_ENFAC = 399,
        BBOX_GENTANKER = 400,
        BBOX_CV67 = 401,
        BBOX_CG47 = 402,
        BBOX_ULSAN = 403,
        BBOX_OFFICE18 = 404,
        BBOX_OFFICE19 = 405,
        BBOX_OFFICE20 = 406,
        BBOX_OFFICE21 = 407,
        BBOX_OFFICE22 = 408,
        BBOX_OFFICE23 = 409,
        BBOX_OFFICE24 = 410,
        BBOX_OFFICE25 = 411,
        BBOX_OFFICE26 = 412,
        BBOX_KIROV = 413,
        BBOX_RUNWAY = 414,
        BBOX_RUNWAY_GA = 415,
        BBOX_RUNWAY_NPGA = 416,
        BBOX_RUNWAY_NPGB = 417,
        BBOX_RUNWAY_NPGC = 418,
        BBOX_RUNWAY_PFLA = 419,
        BBOX_RUNWAY_PFGB = 420,
        BBOX_RUNWAY_PFGA = 421,
        BBOX_RUNWAY_PFLB = 422,
        BBOX_LIGHTS_1 = 423,
        BBOX_LIGHTS_2 = 424,
        BBOX_HANGAR11 = 425,
        BBOX_HANGAR12 = 426,
        BBOX_WTOWER4 = 427,
        BBOX_CTRL_TOWER2 = 428,
        BBOX_EJECT1 = 429,
        BBOX_EJECT2 = 430,
        BBOX_VASI_LB = 431,
        BBOX_VASI_LF = 432,
        BBOX_VASI_RB = 433,
        BBOX_VASI_RF = 434,
        BBOX_AEXPLOSION1 = 435,
        BBOX_SIX_RACK = 436,
        BBOX_QUAD_RACK = 437,
    };

    public enum CampaignType : int
    {
        Campaign,
        TacticalEngagment,
        Training,
        Saved,
        New
    }

    public enum GroundActionType
    {
        GACTION_DEFENSIVE = 1,
        GACTION_CONSOLIDATE = 2,
        GACTION_MINOROFFENSIVE = 3,
        GACTION_OFFENSIVE = 4,
    };

    public enum MissionContext
    {
        noContext,                      // We don't really know why
        enemyUnitAdvanceBridge,         // An enemy unit is advancing over a bridge
        enemyUnitMoveBridge,            // An enemy unit is moving over a bridge
        enemyUnitAdvance,               // An enemy unit is advancing
        enemyUnitMove,                  // An enemy unit is moving
        enemyUnitAttacking,             // An enemy unit is attacking our forces
        enemyUnitDefending,             // An enemy unit is defending against our forces
        enemyForcesPresent,             // Enemy forces are suspected to be here
        attackEnemyUnit,                // FAC CALL: Attack specific enemy unit (8)
        emptyUnit1,                     // TBD (9)
        emptyUnit2,                     // TBD (10)
        friendlyUnitAirborneMovement,   // a friendly unit needs airborne transportation
        emptyUnit5,                     // TBD (12)
        emptyUnit6,                     // TBD (13)
        enemyStrikesExpected,           // Enemy air strikes expected in this area
        enemyAircraftPresent,           // Enemy aircraft are expected (generic) (15)
        enemyGroundForcesPresent,       // Enemy ground forces are expected (JSTAR trigger)
        enemyRadarPresent,              // Enemy radar operating (ECM trigger)
        enemySupportAircraftPresent,    // Enemy support aircraft operating
        enemyCASAircraftPresent,        // Enemy ground attack aircraft are present
        interceptEnemyAircraft,         // AWACS CALL: Intercept specific enemy aircraft (20)
        emptyEnemyAir1,                 // TBD (21)
        hostileAircraftPresent,         // Hostile aircraft operating in an area
        emptyHostileAir1,               // TBD (23)
        friendlyRescueExpected,         // Friendly SAR craft will be operating in the area
        friendlyCASExpected,            // Friendly CAS aircraft will be entering the area (25)
        friendlyAssetsExpected,         // Generic 'Friendly assets' will be operating here
        friendlyAssetsRefueling,        // Friendly aircraft will be refueling in the area (TANKER trigger)
        emptyFriendly1,                 // TBD (28)
        emptyFriendly2,                 // TBD (29)
        enemySupplyInterdictionBridge,  // Enemy supplies are being transported through here (30)
        enemySupplyInterdictionPort,    // Enemy supplies are being transported through here
        enemySupplyInterdictionDepot,   // Enemy supplies are being stored here
        enemySupplyInterdictionZone,    // Enemy supplies are being moved through here
        emptySupply1,                   // TBD (34)
        enemyProductionSource,          // This is producing enemy war materials
        enemyFuelSource,                // This is producing or storing fuel.
        enemyEnergySource,              // This is a source of enemy electrical power
        enemyCommand,                   // This is being used as an enemy CCC module
        enemyAirDefense,                // Enemy air defenses are blocking friendly missions
        enemyAirPowerAirbase,           // This is being used to promote enemy air power (40)
        enemyAirPowerRadar,             // This is being used to promote enemy air power
        emptyObj1,                      // TBD (42)
        emptyObj2,                      // TBD (43)
        friendlyAWACSNeeded,            // We need an awacs (AWACS trigger)
        friendlySuppliesIncomingAir,    // We've got friendly supplies coming in by air
        friendlySuppliesIncomingNaval,  // Same by naval
        friendlySuppliesIncomingGround, // Same by ground
        friendlySuppliesIncomingRail,   // Same by rail
        targetReconNeeded,              // Need recon of this objective
        emptyx,                         // TBD (50)
        emptyy,                         // TBD (51)
        AirActionPrepAD,                // OCA vs Air Defenses part of any air action
        AirActionPrepAB,                // OCA vs Air Bases/Radar part of non-OCA action
        AirActionPrepAir,               // Sweep/Escort part of a any action
        AirActionDCA,                   // Part of a DCA action
        AirActionOCA,                   // Part of an OCA action
        AirActionInterdiction,          // Part of an interdiction action
        AirActionAttrition,             // Part of an attrition action
        AirActionCAS,                   // Part of a CAS action (59)
        emptyAction1,                   // TBD (60)
        enemyNavalForceActive,          // Ships are operating here
        enemyNavalForceStatic,          // Ships in port
        enemyNavalForceUnloading,      // Transport ships unloading in port
        InterceptWithAlertOnly,
        otherContext
    }

    public enum MissionProfileEnum
    {
        MPROF_LOW = 0x01,				// Low engress profile
        MPROF_HIGH = 0x02,				// High engress profile
    };

    public enum MissionRollEnum
    {
        ARO_UNUSED = 0,		    // Free to use
        ARO_CA = 1,
        ARO_TACTRANS = 2,       // AMIS_SAR | AMIS_AIRCAV
        ARO_S = 3,              // AMIS_OCASTRIKE | AMIS_INTSTRIKE | 													// Strike target
        ARO_GA = 4,             // AMIS_SAD | AMIS_BAI | AMIS_ONCALLCAS | AMIS_PRPLANCAS	 							// Ground attack
        ARO_SB = 5,             // AMIS_STRATBOMB           															// Strategic bomb
        ARO_ECM = 6,            // AMIS_ELJAM           																// Jam radar
        ARO_SEAD = 7,           // AMIS_SEADSTRIKE | AMIS_SEADESCORT													// SEAD
        ARO_ASW = 8,            // AMIS_ASW
        ARO_ASHIP = 9,          // AMIS_ASHIP
        ARO_REC = 10,           // AMIS_BDA | AMIS_RECON | AMIS_PATROL													// Recon
        ARO_TRANS = 11,         // AMIS_AIRBORNE | AMIS_AIRLIFT          												// Drop off cargo
        ARO_AWACS = 12,         // AMIS_AWACS
        ARO_JSTAR = 13,         // AMIS_JSTAR
        ARO_ELINT = 13,         // JSTARS and ELINT on same role
        ARO_TANK = 14,          // AMIS_TANKER
        ARO_FAC = 15,
        ARO_OTHER = 16,
    };

    public enum MissionTargetTypeEnum
    {
        AMIS_TAR_NONE = 0,
        AMIS_TAR_OBJECTIVE = 1,
        AMIS_TAR_UNIT = 2,
        AMIS_TAR_LOCATION = 3,
    };

    public enum MissionType
    {
        None,
        [StringValue("Barrier Close Air Patrol 1")]
        BARCAP1,
        [StringValue("Barrier Close Air Patrol 2")]
        BARCAP2,
        [StringValue("High Asset Value Close Air Patrol")]
        HAVCAP,
        [StringValue("Target Close Air Patrol")]
        TARCAP,
        [StringValue("Rescue Escort")]
        RESCORT,
        [StringValue("Ambush Close Air Patrol")]
        AMBUSHCAP,
        [StringValue("Sweep")]
        Sweep,
        [StringValue("Alert")]
        Alert,
        [StringValue("Intercept")]
        Intercept,
        [StringValue("Escort")]
        Escort,
        [StringValue("SEAD Strike")]
        SEADStrike,
        [StringValue("SEAD Escort")]
        SEADEscort,
        [StringValue("Offensive Counter Air Strike")]
        OCAStrike,
        [StringValue("Interdiction Strike")]
        INTStrike,
        [StringValue("Strike")]
        Strike,
        [StringValue("Deep Strike")]
        DeepStrike,
        [StringValue("Stealth Strike")]
        StStrike,
        [StringValue("Strategic Bombing")]
        StratBomb,
        [StringValue("Forward Air Control")]
        FAC,
        [StringValue("On-Call Close Air Support")]
        ONCallCAS,
        [StringValue("Pre-Planned Close Air Support")]
        PRPlanCAS,
        [StringValue("Close Air Support")]
        CAS,
        [StringValue("Search & Destroy")]
        SAD,
        [StringValue("Interdiction")]
        INT,
        [StringValue("Bomber Air Interdiction")]
        BAI,
        [StringValue("Airborne Warning and Control Surveillance")]
        AWACS,
        [StringValue("Joint Surveillance Target Attack Radar")]
        JSTAR,
        [StringValue("Tanker")]
        Tanker,
        [StringValue("Recon")]
        Recon,
        [StringValue("Battle Damage Assessment")]
        BDA,
        [StringValue("Electronic Counter Measures")]
        ECM,
        [StringValue("Airborne Cavalry")]
        AirCAV,
        [StringValue("Airlift")]
        Airlift,
        [StringValue("Search & Rescue")]
        SAR,
        [StringValue("Anti-Submarine")]
        ASW,
        [StringValue("Anti-Ship")]
        AShip,
        [StringValue("Patrol")]
        Patrol,
        [StringValue("Recon Patrol")]
        ReconPatrol,
        [StringValue("Abort")]
        Abort,
        [StringValue("Training")]
        Training,
        [StringValue("Relocate")]
        Relocate,
        Dummy_02,
        Dummy_03,
        Dummy_04,
        Dummy_05,
        Dummy_06,
        Dummy_07,
        Dummy_08,
        Dummy_09
    }

    public enum TeamStance
    {
        None,
        Allied,
        Friendly,
        Neutral,
        Hostile,
        AtWar
    }

    /// <summary>
    /// List of Available Steerpoint Actions
    /// </summary>
    public enum STPTAirAction
    {
        Precision = -1,
        Nav,
        Takeoff, // Altitude needs to be Airbase Elevation
        Push,
        Split,
        Refuel,
        Rearm,
        Pickup,
        Land, // Altitude needs to be Airbase Elevation
        Hold,
        Contact,
        Escort,
        Sweep,
        CAP,
        Intercept,
        GrndAttack,
        SurfaceAttack,
        SearchAndDestroy,
        Strike,
        Bomb,
        SEAD,
        ELINT,
        Recon,
        Rescue,
        ASW,
        Fuel,
        Airdrop,
        Jamming
    }

}
