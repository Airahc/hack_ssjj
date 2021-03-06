﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace ssjj_hack
{
    public class Loop : MonoBehaviour
    {
        private static Loop ins = null;

        void Awake()
        {
            try
            {
                DontDestroyOnLoad(this);
                InitPlugins();
            }
            catch (Exception ex)
            {
                var log = ("Init Exception: " + ex.Message + "\r\n" + ex.StackTrace);
                Log.PrintToFile(log);
            }

            foreach (var t in modules)
            {
                try
                {
                    t.Value.Awake();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void Start()
        {
            foreach (var t in modules)
            {
                try
                {
                    t.Value.Start();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void OnGUI()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " OnGUI");
                    t.Value.OnGUI();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void Update()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " Update");
                    t.Value.Update();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void FixedUpdate()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " FixedUpdate");
                    t.Value.FixedUpdate();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void LateUpdate()
        {
            foreach (var t in modules)
            {
                try
                {
                    BeginWatch(t.Key.Name + " LateUpdate");
                    t.Value.LateUpdate();
                    EndWatch();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        void OnDestroy()
        {
            foreach (var t in modules)
            {
                try
                {
                    t.Value.OnDestroy();
                }
                catch (Exception ex)
                {
                    Log.Print(ex);
                }
            }
        }

        private Stopwatch watch = new Stopwatch();
        private string _currentWatchName = "";
        private void BeginWatch(string name)
        {
            _currentWatchName = name;
            watch.Reset();
            watch.Start();
        }

        private void EndWatch()
        {
            Watcher.Record($"{_currentWatchName}", watch.ElapsedTicks);
            watch.Stop();
        }

        public Dictionary<Type, ModuleBase> modules = new Dictionary<Type, ModuleBase>();

        public void AddPlugin<T>() where T : ModuleBase, new()
        {
            if (!modules.ContainsKey(typeof(T)))
            {
                Log.Print("Run Plugin: " + typeof(T));
                modules.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);
            }
        }

        public static T GetPlugin<T>() where T : ModuleBase
        {
            if (ins == null)
                ins = GameObject.Find("HACK").GetComponent<Loop>();
            if (ins.modules.TryGetValue(typeof(T), out var m))
                return m as T;
            return null;
        }

        public void InitPlugins()
        {
            AddPlugin<Log>();
            AddPlugin<SettingsModule>();
            AddPlugin<GizmosPro>();
            AddPlugin<PlayerCollector>();
            AddPlugin<Hierarchy>();
            AddPlugin<Inspector>();
            AddPlugin<Esp>();
            // AddPlugin<Aim>();
            AddPlugin<Fun>();
            AddPlugin<Watcher>();
            // AddPlugin<Chat>();
            // AddPlugin<Punch>();
            // AddPlugin<Spread>();
        }
    }
}
