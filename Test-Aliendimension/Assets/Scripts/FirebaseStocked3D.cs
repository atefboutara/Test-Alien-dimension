using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class FirebaseStocked3D : MonoBehaviour
{
    #region Variables

    FirebaseStorage storage;
    StorageReference storage_ref;
    AssetBundle bundle;

    public RuntimeAnimatorController AnimationController;

    #region small fix

    GameObject Loaded3DLogo = null;

    #endregion

    #endregion

    #region Monobehaviour

    void Start()
    {
        StartCoroutine(LoadLogo());
    }

    void Update()
    {
        if(Loaded3DLogo != null)//This is a small fix because the loaded model from firebase was still showing (i tried on the editor) even when there was no marker
        {
            if (isTrackingMarker())
            {
                if (!Loaded3DLogo.activeInHierarchy)
                    Loaded3DLogo.SetActive(true);
            }
            else
                Loaded3DLogo.SetActive(false);
        }
    }

    #endregion

    #region Firebase

    IEnumerator LoadLogo()
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle("https://firebasestorage.googleapis.com/v0/b/testunity-496ff.appspot.com/o/Models%2Fpepsilogo.unity3d?alt=media&token=547f9885-352a-4852-8b81-77c50593e135"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Debug.Log("Downloaded object");
                bundle = DownloadHandlerAssetBundle.GetContent(uwr);

                yield return bundle;
                            
                var loadAsset = bundle.LoadAssetAsync<GameObject>("pepsi.FBX");
                yield return loadAsset;
                
                GameObject Logo = (GameObject) Instantiate(loadAsset.asset);
                Logo.transform.rotation = new Quaternion(0, 0, 0, 0);
                Logo.transform.position = new Vector3(-0.017f, 0.07900001f, 0.009000001f);
                Logo.AddComponent<Animator>().runtimeAnimatorController = AnimationController;
                Logo.transform.parent = transform;

                Loaded3DLogo = Logo;
            }
        }


    }

    #endregion

    #region Functions

    private bool isTrackingMarker()
    {
        var trackable = gameObject.GetComponent<TrackableBehaviour>();
        var status = trackable.CurrentStatus;
        return status == TrackableBehaviour.Status.TRACKED;
    }

    #endregion
}
