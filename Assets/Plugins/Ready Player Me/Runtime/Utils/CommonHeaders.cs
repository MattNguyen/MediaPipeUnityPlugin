﻿using System.Collections.Generic;
using UnityEngine;

namespace ReadyPlayerMe
{
    public static class CommonHeaders
    {
        private const string RPM_SOURCE = "RPM_Source";
        private const string RPM_SDK_VERSION = "RPM-SDK-Version";
        private const string UNITY_PREFIX = "unity";
        private const string EDITOR = "editor";
        private const string RUNTIME = "runtime";
        private const string PLAYMODE = "playmode";
        private const string EDITMODE = "editmode";
        private const string SEPARATOR = "-";

        public static Dictionary<string, string> GetRequestHeaders()
        {
            var source = UNITY_PREFIX + SEPARATOR;
            if (Application.isEditor)
            {
                source += EDITOR + SEPARATOR + (Application.isPlaying ? PLAYMODE : EDITMODE);
            }
            else
            {
                source += RUNTIME;
            }
            return new Dictionary<string, string>()
            {
                { RPM_SOURCE, source },
                { RPM_SDK_VERSION, ApplicationData.GetData().SDKVersion }
            };
        }
    }
}
