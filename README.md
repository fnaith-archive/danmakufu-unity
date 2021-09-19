# Goal

This project aims to play Touhou Danmakufu ph3 script in Unity 3D.

Currently under development, no useful thing here.

# Roadmap

| Folder | Package | Module | Status |
| --- | --- | --- | --- |
| GcLib | | | |
| | gstd | | |
| | | Application | TODO |
| | | File | TODO |
| | | FpsController | TODO |
| | | GstdUtility | TODO |
| | | Logger | TODO |
| | | MersenneTwister | TODO |
| | | Script | TODO |
| | | ScriptClient | TODO |
| | | SmartPointer | REMOVE |
| | | Task | TODO |
| | | Thread | TODO |
| | | Window | TODO |
| | directx | | Use Unity |
| | ext | | Use Unity |
| TouhouDanmakufu | | | |
| | Common |  | |
| | | DnhCommon | TODO |
| | | DnhGcLibImpl | TODO |
| | | DnhReplay | TODO |
| | | DnhScript | TODO |
| | | StgCommon | TODO |
| | | StgControlScript | TODO |
| | | StgEnemy | TODO |
| | | StgIntersection | TODO |
| | | StgItem | TODO |
| | | StgPackageController | TODO |
| | | StgPackageScript | TODO |
| | | StgPlayer | TODO |
| | | StgShot | TODO |
| | | StgStageController | TODO |
| | | StgStageScript | TODO |
| | | StgSystem | TODO |
| | | StgUserExtendScene | TODO |
| | DnhConfig | | TODO |
| | DnhExecutor | | TODO |
| | DnhViewer | | TBD |
| FileArchiver | | | TBD |

# Line of Code in [GcProject](https://github.com/fnaith/danmakufu-ph3)

| Folder | Package | Lines |
| --- | --- | ---: |
| GcLib | | 42099 |
| | gstd | 15300 |
| | directx | 26101 |
| | ext | 691 |
| TouhouDanmakufu | | 24376 |
| | Common | 18485 |
| | DnhConfig | 1035 |
| | DnhExecutor | 2951 |
| | DnhViewer | 1905 |
| FileArchiver | | 604 |

# Supported Function

| Category | Name | Status |
| --- | --- | --- |
| Math | | |
| | min | TODO |
| | max | TODO |
| | log | TODO |
| | log10 | TODO |
| | cos | TODO |
| | sin | TODO |
| | tan | TODO |
| | acos | TODO |
| | asin | TODO |
| | atan | TODO |
| | atan2 | TODO |
| | rand | TODO |
| | round | TODO |
| | truncate | TODO |
| | ceil | TODO |
| | floor | TODO |
| | absolute | TODO |
| | modc | TODO |
| Text | | |
| | InstallFont | TODO |
| | ToString | TODO |
| | IntToString | TODO |
| | itoa | TODO |
| | rtoa | TODO |
| | rtos | TODO |
| | vtos | TODO |
| | atoi | TODO |
| | ator | TODO |
| | TrimString | TODO |
| | SplitString | TODO |
| Path | | TBD |
| Time | | |
| | GetCurrentDateTimeS | TODO |
| | GetStageTime | TODO |
| | GetPackageTime | TODO |
| | GetCurrentFps | TODO |
| | GetReplayFps | TODO |
| Debug | | |
| | WriteLog | TODO |
| | RaiseError | TODO |
| Common Data | | |
| | SetCommonData | TODO |
| | GetCommonData | TODO |
| | ClearCommonData | TODO |
| | DeleteCommonData | TODO |
| | SetAreaCommonData | TODO |
| | GetAreaCommonData | TODO |
| | ClearAreaCommonData | TODO |
| | DeleteAreaCommonData | TODO |
| | CreateCommonDataArea | TODO |
| | IsCommonDataAreaExists | TODO |
| | CopyCommonDataArea | TODO |
| | GetCommonDataAreaKeyList | TODO |
| | GetCommonDataValueKeyList | TODO |
| | SaveCommonDataAreaA1 | TODO |
| | LoadCommonDataAreaA1 | TODO |
| | SaveCommonDataAreaA2 | TODO |
| | LoadCommonDataAreaA2 | TODO |
| | SaveCommonDataAreaToReplayFile | TODO |
| | LoadCommonDataAreaFromReplayFile | TODO |
| Audio | | |
| | LoadSound | TODO |
| | RemoveSound | TODO |
| | PlayBGM | TODO |
| | PlaySE | TODO |
| | StopSound | TODO |
| Input | | |
| | GetVirtualKeyState | TODO |
| | SetVirtualKeyState | TODO |
| | AddVirtualKey | TODO |
| | AddReplayTargetVirtualKey | TODO |
| | GetKeyState | TODO |
| | GetMouseState | TODO |
| | GetMouseX | TODO |
| | GetMouseY | TODO |
| | GetMouseMoveZ | TODO |
| | SetSkipModeKey | TODO |
| Render | | |
| | LoadTexture | TODO |
| | LoadTextureInLoadThread | TODO |
| | RemoveTexture | TODO |
| | GetTextureWidth | TODO |
| | GetTextureHeight | TODO |
| | SetFogEnable | TODO |
| | SetFogParam | TODO |
| | ClearInvalidRenderPriority | TODO |
| | SetInvalidRenderPriorityA1 | TODO |
| | GetReservedRenderTargetName | TODO |
| | CreateRenderTarget | TODO |
| | RenderToTextureA1 | TODO |
| | RenderToTextureB1 | TODO |
| | SaveRenderedTextureA1 | TODO |
| | SaveRenderedTextureA2 | TODO |
| | SaveSnapShotA1 | TODO |
| | SaveSnapShotA2 | TODO |
| | IsPixelShaderSupported | TODO |
| | SetShader | TODO |
| | SetShaderI | TODO |
| | ResetShader | TODO |
| | ResetShaderI | TODO |
| 3D Camera | | TBD |
| 2D Camera | | |
| | Set2DCameraFocusX | TODO |
| | Set2DCameraFocusY | TODO |
| | Set2DCameraAngleZ | TODO |
| | Set2DCameraRatio | TODO |
| | Set2DCameraRatioX | TODO |
| | Set2DCameraRatioY | TODO |
| | Reset2DCamera | TODO |
| | Get2DCameraX | TODO |
| | Get2DCameraY | TODO |
| | Get2DCameraAngleZ | TODO |
| | Get2DCameraRatio | TODO |
| | Get2DCameraRatioX | TODO |
| | Get2DCameraRatioY | TODO |
| Script | | TBD |
| System | | |
| | SetStgFrame | TODO |
| | GetScore | TODO |
| | AddScore | TODO |
| | GetGraze | TODO |
| | AddGraze | TODO |
| | GetPoint | TODO |
| | AddPoint | TODO |
| | SetItemRenderPriorityI | TODO |
| | SetShotRenderPriorityI | TODO |
| | GetStgFrameRenderPriorityMinI | TODO |
| | GetStgFrameRenderPriorityMaxI | TODO |
| | GetItemRenderPriorityI | TODO |
| | GetShotRenderPriorityI | TODO |
| | GetPlayerRenderPriorityI | TODO |
| | GetCameraFocusPermitPriorityI | TODO |
| | GetStgFrameLeft | TODO |
| | GetStgFrameTop | TODO |
| | GetStgFrameWidth | TODO |
| | GetStgFrameHeight | TODO |
| | GetScreenWidth | TODO |
| | GetScreenHeight | TODO |
| | IsReplay | TODO |
| | AddArchiveFile | TODO |
| Player | | |
| | GetPlayerObjectID | TODO |
| | GetPlayerScriptID | TODO |
| | SetPlayerSpeed | TODO |
| | SetPlayerClip | TODO |
| | SetPlayerLife | TODO |
| | SetPlayerSpell | TODO |
| | SetPlayerPower | TODO |
| | SetPlayerInvincibilityFrame | TODO |
| | SetPlayerDownStateFrame | TODO |
| | SetPlayerRebirthFrame | TODO |
| | SetPlayerRebirthLossFrame | TODO |
| | SetPlayerAutoItemCollectLine | TODO |
| | SetForbidPlayerShot | TODO |
| | SetForbidPlayerSpell | TODO |
| | GetPlayerX | TODO |
| | GetPlayerY | TODO |
| | GetPlayerState | TODO |
| | GetPlayerSpeed | TODO |
| | GetPlayerClip | TODO |
| | GetPlayerLife | TODO |
| | GetPlayerSpell | TODO |
| | GetPlayerPower | TODO |
| | GetPlayerInvincibilityFrame | TODO |
| | GetPlayerDownStateFrame | TODO |
| | GetPlayerRebirthFrame | TODO |
| | GetAngleToPlayer | TODO |
| | IsPermitPlayerShot | TODO |
| | IsPermitPlayerSpell | TODO |
| | IsPlayerLastSpellWait | TODO |
| | IsPlayerSpellActive | TODO |
| | GetPlayerID | TODO |
| | GetPlayerReplayName | TODO |
| Enemy | | |
| | GetEnemyBossSceneObjectID | TODO |
| | GetEnemyBossObjectID | TODO |
| | GetAllEnemyID | TODO |
| | GetIntersectionRegistedEnemyID | TODO |
| | GetAllEnemyIntersectionPosition | TODO |
| | GetEnemyIntersectionPosition | TODO |
| | GetEnemyIntersectionPositionByIdA1 | TODO |
| | GetEnemyIntersectionPositionByIdA2 | TODO |
| | LoadEnemyShotData | TODO |
| | ReloadEnemyShotData | TODO |
| Shot | | |
| | DeleteShotAll | TODO |
| | DeleteShotInCircle | TODO |
| | CreateShotA1 | TODO |
| | CreateShotA2 | TODO |
| | CreateShotOA1 | TODO |
| | CreateShotB1 | TODO |
| | CreateShotB2 | TODO |
| | CreateShotOB1 | TODO |
| | CreateLooseLaserA1 | TODO |
| | CreateStraightLaserA1 | TODO |
| | CreateCurveLaserA1 | TODO |
| | SetShotIntersectionCircle | TODO |
| | SetShotIntersectionLine | TODO |
| | GetShotIdInCircleA1 | TODO |
| | GetShotIdInCircleA2 | TODO |
| | GetShotCount | TODO |
| | SetShotAutoDeleteClip | TODO |
| | GetShotDataInfoA1 | TODO |
| | StartShotScript | TODO |
| Item | | |
| | CreateItemA1 | TODO |
| | CreateItemA2 | TODO |
| | CreateItemU1 | TODO |
| | CreateItemU2 | TODO |
| | CollectAllItems | TODO |
| | CollectItemsByType | TODO |
| | CollectItemsInCircle | TODO |
| | CancelCollectItems | TODO |
| | StartItemScript | TODO |
| | SetDefaultBonusItemEnable | TODO |
| | LoadItemData | TODO |
| | ReloadItemData | TODO |
| Other | | TBD |
| Object | | |
| | Obj_Delete | TODO |
| | Obj_IsDeleted | TODO |
| | Obj_SetVisible | TODO |
| | Obj_IsVisible | TODO |
| | Obj_SetRenderPriority | TODO |
| | Obj_SetRenderPriorityI | TODO |
| | Obj_GetRenderPriority | TODO |
| | Obj_GetRenderPriorityI | TODO |
| | Obj_GetValue | TODO |
| | Obj_GetValueD | TODO |
| | Obj_SetValue | TODO |
| | Obj_DeleteValue | TODO |
| | Obj_IsValueExists | TODO |
| | Obj_GetType | TODO |
| Render Object | | |
| | ObjRender_SetX | TODO |
| | ObjRender_SetY | TODO |
| | ObjRender_SetZ | TODO |
| | ObjRender_SetPosition | TODO |
| | ObjRender_SetAngleX | TODO |
| | ObjRender_SetAngleY | TODO |
| | ObjRender_SetAngleZ | TODO |
| | ObjRender_SetAngleXYZ | TODO |
| | ObjRender_SetScaleX | TODO |
| | ObjRender_SetScaleY | TODO |
| | ObjRender_SetScaleZ | TODO |
| | ObjRender_SetScaleXYZ | TODO |
| | ObjRender_SetColor | TODO |
| | ObjRender_SetColorHSV | TODO |
| | ObjRender_SetAlpha | TODO |
| | ObjRender_SetBlendType | TODO |
| | ObjRender_GetX | TODO |
| | ObjRender_GetY | TODO |
| | ObjRender_GetZ | TODO |
| | ObjRender_GetAngleX | TODO |
| | ObjRender_GetAngleY | TODO |
| | ObjRender_GetAngleZ | TODO |
| | ObjRender_GetScaleX | TODO |
| | ObjRender_GetScaleY | TODO |
| | ObjRender_GetScaleZ | TODO |
| | ObjRender_GetBlendType | TODO |
| | ObjRender_SetZWrite | TODO |
| | ObjRender_SetZTest | TODO |
| | ObjRender_SetFogEnable | TODO |
| | ObjRender_SetPermitCamera | TODO |
| Primitive Object | | TBD |
| 2D Sprite Object | | TBD |
| 2D Sprite List Object | | TBD |
| 3D Sprite Object | | TBD |
| Mesh Object | | TBD |
| Text Object | | |
| | ObjText_Create | TODO |
| | ObjText_SetText | TODO |
| | ObjText_SetFontType | TODO |
| | ObjText_SetFontSize | TODO |
| | ObjText_SetFontBold | TODO |
| | ObjText_SetFontColorTop | TODO |
| | ObjText_SetFontColorBottom | TODO |
| | ObjText_SetFontBorderWidth | TODO |
| | ObjText_SetFontBorderType | TODO |
| | ObjText_SetFontBorderColor | TODO |
| | ObjText_SetMaxWidth | TODO |
| | ObjText_SetMaxHeight | TODO |
| | ObjText_SetLinePitch | TODO |
| | ObjText_SetSidePitch | TODO |
| | ObjText_SetTransCenter | TODO |
| | ObjText_SetAutoTransCenter | TODO |
| | ObjText_SetHorizontalAlignment | TODO |
| | ObjText_SetSyntacticAnalysis | TODO |
| | ObjText_GetTextLength | TODO |
| | ObjText_GetTextLengthCU | TODO |
| | ObjText_GetTextLengthCUL | TODO |
| | ObjText_GetTotalWidth | TODO |
| | ObjText_GetTotalHeight | TODO |
| Shader Object | | TBD |
| Sound Object | | |
| | ObjSound_Create | TODO |
| | ObjSound_Load | TODO |
| | ObjSound_Play | TODO |
| | ObjSound_Stop | TODO |
| | ObjSound_SetVolumeRate | TODO |
| | ObjSound_SetPanRate | TODO |
| | ObjSound_SetFade | TODO |
| | ObjSound_SetLoopEnable | TODO |
| | ObjSound_SetLoopTime | TODO |
| | ObjSound_SetLoopSampleCount | TODO |
| | ObjSound_SetRestartEnable | TODO |
| | ObjSound_SetSoundDivision | TODO |
| | ObjSound_IsPlaying | TODO |
| | ObjSound_GetVolumeRate | TODO |
| File Object | | TBD |
| Text File Object | | TBD |
| Binary File Object | | TBD |
| Move Object | | |
| | ObjMove_SetX | TODO |
| | ObjMove_SetY | TODO |
| | ObjMove_SetPosition | TODO |
| | ObjMove_SetSpeed | TODO |
| | ObjMove_SetAngle | TODO |
| | ObjMove_SetAcceleration | TODO |
| | ObjMove_SetMaxSpeed | TODO |
| | ObjMove_SetAngularVelocity | TODO |
| | ObjMove_SetDestAtSpeed | TODO |
| | ObjMove_SetDestAtFrame | TODO |
| | ObjMove_SetDestAtWeight | TODO |
| | ObjMove_AddPatternA1 | TODO |
| | ObjMove_AddPatternA2 | TODO |
| | ObjMove_AddPatternA3 | TODO |
| | ObjMove_AddPatternA4 | TODO |
| | ObjMove_AddPatternB1 | TODO |
| | ObjMove_AddPatternB2 | TODO |
| | ObjMove_AddPatternB3 | TODO |
| | ObjMove_GetX | TODO |
| | ObjMove_GetY | TODO |
| | ObjMove_GetSpeed | TODO |
| | ObjMove_GetAngle | TODO |
| Enemy Object | | |
| | ObjEnemy_Create | TODO |
| | ObjEnemy_Regist | TODO |
| | ObjEnemy_GetInfo | TODO |
| | ObjEnemy_SetLife | TODO |
| | ObjEnemy_AddLife | TODO |
| | ObjEnemy_SetDamageRate | TODO |
| | ObjEnemy_SetIntersectionCircleToShot | TODO |
| | ObjEnemy_SetIntersectionCircleToPlayer | TODO |
| Boss Object | | |
| | ObjEnemyBossScene_Create | TODO |
| | ObjEnemyBossScene_Regist | TODO |
| | ObjEnemyBossScene_Add | TODO |
| | ObjEnemyBossScene_LoadInThread | TODO |
| | ObjEnemyBossScene_GetInfo | TODO |
| | ObjEnemyBossScene_SetSpellTimer | TODO |
| | ObjEnemyBossScene_StartSpell | TODO |
| Shot Object | | |
| | ObjShot_Create | TODO |
| | ObjShot_Regist | TODO |
| | ObjShot_SetAutoDelete | TODO |
| | ObjShot_FadeDelete | TODO |
| | ObjShot_SetDeleteFrame | TODO |
| | ObjShot_SetDelay | TODO |
| | ObjShot_SetSpellResist | TODO |
| | ObjShot_SetGraphic | TODO |
| | ObjShot_SetSourceBlendType | TODO |
| | ObjShot_SetDamage | TODO |
| | ObjShot_SetPenetration | TODO |
| | ObjShot_SetEraseShot | TODO |
| | ObjShot_SetSpellFactor | TODO |
| | ObjShot_ToItem | TODO |
| | ObjShot_AddShotA1 | TODO |
| | ObjShot_AddShotA2 | TODO |
| | ObjShot_SetIntersectionCircleA1 | TODO |
| | ObjShot_SetIntersectionCircleA2 | TODO |
| | ObjShot_SetIntersectionLine | TODO |
| | ObjShot_SetIntersectionEnable | TODO |
| | ObjShot_SetItemChange | TODO |
| | ObjShot_GetDamage | TODO |
| | ObjShot_GetPenetration | TODO |
| | ObjShot_GetDelay | TODO |
| | ObjShot_IsSpellResist | TODO |
| | ObjShot_GetImageID | TODO |
| | ObjLaser_SetLength | TODO |
| | ObjLaser_SetRenderWidth | TODO |
| | ObjLaser_SetIntersectionWidth | TODO |
| | ObjLaser_SetGrazeInvalidFrame | TODO |
| | ObjLaser_SetInvalidLength | TODO |
| | ObjLaser_SetItemDistance | TODO |
| | ObjLaser_GetLength | TODO |
| | ObjStLaser_SetAngle | TODO |
| | ObjStLaser_GetAngle | TODO |
| | ObjStLaser_SetSource | TODO |
| | ObjCrLaser_SetTipDecrement | TODO |
| Item Object | | |
| | ObjItem_SetItemID | TODO |
| | ObjItem_SetRenderScoreEnable | TODO |
| | ObjItem_SetAutoCollectEnable | TODO |
| | ObjItem_SetDefinedMovePatternA1 | TODO |
| | ObjItem_GetInfo | TODO |
| Player Object | | |
| | ObjPlayer_AddIntersectionCircleA1 | TODO |
| | ObjPlayer_AddIntersectionCircleA2 | TODO |
| | ObjPlayer_ClearIntersection | TODO |
| Collision Object | | |
| | ObjPlayer_AddIntersec
| | ObjCol_IsIntersected | TODO |
| | ObjCol_GetListOfIntersectedEnemyID | TODO |
| | ObjCol_GetIntersectedCount | TODO |
| Player Script | | |
| | CreatePlayerShotA1 | TODO |
| | CallSpell | TODO |
| | LoadPlayerShotData | TODO |
| | ReloadPlayerShotData | TODO |
| | GetSpellManageObject | TODO |
| Spell Object| | |
| | ObjSpell_Create | TODO |
| | ObjSpell_Regist | TODO |
| | ObjSpell_SetDamage | TODO |
| | ObjSpell_SetEraseShot | TODO |
| | ObjSpell_SetIntersectionCircle | TODO |
| | ObjSpell_SetIntersectionLine | TODO |
| System Script | | |
| | SetPauseScriptPath | TODO |
| | SetEndSceneScriptPath | TODO |
| | SetReplaySaveSceneScriptPath | TODO |
| | GetTransitionRenderTargetName | TODO |
| Shot Custom Script | | |
| | SetShotDeleteEventEnable | TODO |
| Package Script | | |
| | ClosePackage | TODO |
| | InitializeStageScene | TODO |
| | FinalizeStageScene | TODO |
| | StartStageScene | TODO |
| | SetStageIndex | TODO |
| | SetStageMainScript | TODO |
| | SetStagePlayerScript | TODO |
| | SetStageReplayFile | TODO |
| | GetStageSceneState | TODO |
| | GetStageSceneResult | TODO |
| | PauseStageScene | TODO |
| | TerminateStageScene | TODO |
| | GetLoadFreePlayerScriptList | TODO |
| | GetFreePlayerScriptCount | TODO |
| | GetFreePlayerScriptInfo | TODO |
| | LoadReplayList | TODO |
| | GetValidReplayIndices | TODO |
| | IsValidReplayIndex | TODO |
| | GetReplayInfo | TODO |
| | SetReplayInfo | TODO |
| | SaveReplay | TODO |
