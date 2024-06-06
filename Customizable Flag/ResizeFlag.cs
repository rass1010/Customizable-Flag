using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Customizable_Flag
{
    internal class ResizeFlag : MonoBehaviour
    {
        Texture2D flag;
        SkinnedMeshRenderer meshRenderer;
        string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        void Awake () 
        {
            StartCoroutine(LoadFlagImage());
        }

        IEnumerator LoadFlagImage()
        {
            Plugin.Instance.log("test 1");
            DirectoryInfo directory = new DirectoryInfo(fileLocation + "\\Flag");
            Plugin.Instance.log("directory found");

            FileInfo[] files = directory.GetFiles();
            if (files.Length == 0)
            {
                Plugin.Instance.log("No files found in directory.");
                yield break;
            }

            // Create the UnityWebRequest
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + files[0].FullName);

            // Send the request and wait for it to complete
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Plugin.Instance.log("Failed to load image: " + www.error);
                yield break;
            }

            // Get the texture from the request
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(www);
            Plugin.Instance.log("assigned image");

            // Debugging: Log the width and height of the downloaded texture
            Plugin.Instance.log("Downloaded texture width: " + downloadedTexture.width);
            Plugin.Instance.log("Downloaded texture height: " + downloadedTexture.height);

            // Ensure the texture uses the image's resolution
            flag = new Texture2D(downloadedTexture.width, downloadedTexture.height, downloadedTexture.format, false);
            flag.SetPixels(downloadedTexture.GetPixels());
            flag.Apply();

            // Debugging: Log the width and height of the new texture
            Plugin.Instance.log("New texture width: " + flag.width);
            Plugin.Instance.log("New texture height: " + flag.height);

            // Get the SkinnedMeshRenderer and set the texture and scale
            meshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            float aspectRatio = (float)flag.width / flag.height;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * aspectRatio);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, aspectRatio / 100f);
            meshRenderer.material.mainTexture = flag;
            Plugin.Instance.log("everything is working");
        }

    }
}
