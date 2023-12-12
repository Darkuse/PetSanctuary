using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;

public class AutoLoginCloudSave : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymous();

    }

    async Task SignInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in was a success");
            Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException e)
        {
            Debug.Log("Sign in failed: " + e);
        }
    }
}
