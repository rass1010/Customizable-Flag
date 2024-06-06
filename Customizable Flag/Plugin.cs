using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace Customizable_Flag
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")] // Make sure to add Utilla 1.5.0 as a dependency!
    public class Plugin : BaseUnityPlugin
    {

        public static Plugin Instance { get; private set; }

        void Awake()
        {
            Utilla.Events.GameInitialized += GameInitialized;
            Instance = this;
        }

        private void GameInitialized(object sender, EventArgs e)
        {
            
            // Player instance has been created
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Customizable_Flag.flag");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);

            GameObject Flag = bundle.LoadAsset<GameObject>("Flag");
            GameObject newFlag = Instantiate(Flag);

            Transform rightHand = GameObject.Find("Local Gorilla Player/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R").transform;
            newFlag.transform.SetParent(rightHand, false);
            newFlag.transform.localScale = new Vector3(10f, 10f, 10f);
            newFlag.transform.localEulerAngles = new Vector3(281.051117f, 253.24057f, 98.3139038f);
            newFlag.transform.localPosition = new Vector3(0.0340000018f, 0.0250000004f, -0.451000005f);
            newFlag.transform.Find("FlagCloth").gameObject.AddComponent<ResizeFlag>();
            newFlag.transform.Find("Cylinder").GetComponent<Collider>().enabled = false;
        }


        public void log(string msg)
        {
            Logger.LogError(msg);
        }

    }
}