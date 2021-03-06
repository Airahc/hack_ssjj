﻿using Assets.Sources.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ssjj_hack
{
    public static class MathTool
    {
        public static float PitchToScreen(float angle)
        {
            // y = x * 2 * rev * (0.01f + 0.001f * sen) * (fov / 90f)
            var rev = GameSetting.Config.MouseReversal == 1 ? -1 : 1;
            var sen = GameSetting.Config.MouseSensitivity;
            var fov = Contexts.sharedInstance.player.cameraOwnerEntity.fov.Fov;
            var x = angle / (2f * rev * (0.01f + 0.001f * sen) * (fov / 90f));
            return x;
        }

        public static float YawToScreen(float angle)
        {
            // y = x * -2 * (0.01f + 0.001f * sen) * (fov / 90f)
            var sen = GameSetting.Config.MouseSensitivity;
            var fov = Contexts.sharedInstance.player.cameraOwnerEntity.fov.Fov;
            var x = angle / (-2f * (0.01f + 0.001f * sen) * (fov / 90f));
            return x;
        }

        public static float SignedAngle(Vector2 v1, Vector2 v2)
        {
            float angle = Vector3.Angle(v1, v2);
            angle *= Mathf.Sign(Vector3.Cross(v1, v2).y);
            return angle;
        }
    }
}
