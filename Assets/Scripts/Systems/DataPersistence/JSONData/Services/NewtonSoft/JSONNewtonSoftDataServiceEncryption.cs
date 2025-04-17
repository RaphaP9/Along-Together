using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class JSONNewtonSoftDataServiceEncryption : IDataService
{
    private const string KEY = "E7IguajZyOAwwi9S4q7bMezF6x4TrZMgWcgVRJ7/R7k=";
    private const string IV = "cEasNyuERH33exDTe8SZKQ==";

    private string dirPath;
    private string filePath;

    public JSONNewtonSoftDataServiceEncryption(string dirPath, string filePath)
    {
        this.dirPath = dirPath;
        this.filePath = filePath;
    }

    #region Save Data
    public bool SaveData<T>(T data)
    {
        string path = Path.Combine(dirPath, filePath);

        try
        {
            FileStream stream = File.Create(path);        
            WriteEncryptedData(data, stream);
          
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }

    }

    private void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();

        //Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");  //To get both new Key and IV
        //Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTranfrorm = aesProvider.CreateEncryptor();

        using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTranfrorm, CryptoStreamMode.Write);
        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    public async Task<bool> SaveDataAsync<T>(T data)
    {
        string path = Path.Combine(dirPath, filePath);

        try
        {
            FileStream stream = File.Create(path);
            await WriteEncryptedDataAsync(data, stream);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public async Task WriteEncryptedDataAsync<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();

        //Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");  //To get both new Key and IV
        //Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTranfrorm = aesProvider.CreateEncryptor();

        using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTranfrorm, CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    #endregion

    #region Load Data

    public T LoadData<T>() 
    {
        string path = Path.Combine(dirPath, filePath);

        if (!File.Exists(path))
        {
            //Debug.LogError($"Cannot load file at {path}: File does not exist!");
            //throw new FileNotFoundException($"{path} does not exists");

            return default;
        }

        try
        {
            T data;          
            data = ReadEncryptedData<T>(path);
            
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    private T ReadEncryptedData<T> (string path)
    {
        byte[] fileBYtes = File.ReadAllBytes(path);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTranfrorm = aesProvider.CreateDecryptor();

        using MemoryStream decryptionStream = new MemoryStream(fileBYtes);
        using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTranfrorm, CryptoStreamMode.Read);

        using StreamReader reader = new StreamReader(cryptoStream);
        string result = reader.ReadToEnd();

        Debug.Log($"Decrypted result (if not legible, wrong Key or IV): {result}");
        return JsonConvert.DeserializeObject<T> (result);
    }

    public async Task<T> LoadDataAsync<T>()
    {
        string path = Path.Combine(dirPath, filePath);

        if (!File.Exists(path))
        {
            //Debug.LogError($"Cannot load file at {path}: File does not exist!");
            //throw new FileNotFoundException($"{path} does not exists");

            return default;
        }

        try
        {
            T data;
            data = await ReadEncryptedDataAsync<T>(path);

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    private async Task<T> ReadEncryptedDataAsync<T>(string path)
    {
        byte[] fileBYtes = File.ReadAllBytes(path);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTranfrorm = aesProvider.CreateDecryptor();

        using MemoryStream decryptionStream = new MemoryStream(fileBYtes);
        using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTranfrorm, CryptoStreamMode.Read);

        using StreamReader reader = new StreamReader(cryptoStream);
        string result = await reader.ReadToEndAsync();

        Debug.Log($"Decrypted result (if not legible, wrong Key or IV): {result}");
        return JsonConvert.DeserializeObject<T>(result);
    }
    #endregion
}
