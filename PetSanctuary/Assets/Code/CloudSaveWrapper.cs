    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Unity.Services.CloudSave;
    using UnityEngine;
    using UnityEngine.Subsystems;

    public static class CloudSaveWrapper
    {



    /*
    // INIT         in Start, preferably
    await AuthenticationService.Instance.SignInAnonymouslyAsync();

    // SAVE         if you save with the same key then it just gets overwritten
    await CloudSaveWrapper.Save<string>("greetingKey", "Hello World");
    // for example if you were to
    // SAVE("randomKey","123"); then SAVE("randomKey","321");
    // if you LOAD("randomKey"); then you'd see "321"

    // LOAD
    string greeting = await CloudSaveWrapper.Load<string>("greetingKey");

    // UPDATE
    await CloudSaveWrapper.UpdateData("greetingKey", "Hello Universe");

    // DELETE
    await CloudSaveWrapper.Delete("greetingKey");
    */

    // Save data to the cloud
    public static async Task Save<T>(string key, T data)
        {
            var dataToSave = new Dictionary<string, object> { { key, data } };
            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data: {e.Message}");
            }
        }

        // Load data from the cloud
        public static async Task<T> Load<T>(string key)
        {
            try
            {
                var results = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
                if (results.TryGetValue(key, out var item))
                {
                    return item.Value.GetAs<T>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data: {e.Message}");
            }

            return default(T);
        }

        // Delete data from the cloud
        public static async Task Delete(string key)
        {
            try
            {
                await CloudSaveService.Instance.Data.Player.DeleteAsync(key);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting data: {e.Message}");
            }
        }

        // Update data in the cloud
        public static async Task UpdateData<T>(string key, T newData)
        {
            await Save(key, newData);
        }
    }
