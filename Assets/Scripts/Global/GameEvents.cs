using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
}
public enum NetworkEvent : byte
{
    SEND_READY = 0,
    SEND_TEAMS = 1,
    SEND_ROOM_STATE=2,
    SEND_READY_COUNTDOWN=3,
    SEND_LEAVE_ROOM=4,
    SEND_HERO_COUNTDOWN = 5,
    SEND_LOCK_HERO = 6,
    SEND_RANDOM_HERO = 7,
    SEND_LOAD_GAME_SCENE = 8,
    SEND_SCENE_LOADED = 9,
    SEND_ACTIVE_GAME_SCENE = 10
}
