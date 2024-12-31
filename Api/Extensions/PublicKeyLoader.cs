using System.Security.Cryptography;
using System.Text;
using System.IO;

// LoadPublicKey: This method reads the .pem file, extracts the public key, and loads it into an RSA object. Make sure the .pem file contains the public key in the correct format.

namespace Api.Extensions;
public static class PublicKeyLoader
{
    public static RSA LoadPublicKey()
    {
        string key =
            "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAzTWIooNO5YOsPIBynejO\nDk5Kzx4nbrpGVg8ydMuQALZUo0bhjWtHQr/Dl9pF0MRT3y1syn6Zzpqjf0TEo87T\nrvknax2Hou+8C9eNBTOR26wXM6A+ofikGGI28NNO8VGzSxiaR4VTNDSFEuX7jY99\nZ9BLzU4ho4cs6XZW+2qcEcODN/XR1yIQ/Ej40R8E72Zr4KnFaVX9XA52Oto4dXzO\n1uD5BOBr7T/Dk4XpWIm/OuzRcMrBs6CSztbPJjHaGiqlxprInn4Y1uCTGLRHO5LI\ny2ph6KTprEKe/4VgWLFHxpzi4MmEDXMjNJhXDqZpZpKQsPgJGgiuVsxAgsKPBvn9\nwQIDAQAB\n-----END PUBLIC KEY-----\n";
        Console.WriteLine("foi");
        
        // Remove the PEM header and footer
        string publicKeyPem = key.Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\r", "")
            .Replace("\n", "");

        // Convert the base64 string to a byte array
        byte[] publicKeyBytes = Convert.FromBase64String(publicKeyPem);

        // Create an RSA object and import the public key
        RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

        return rsa;
    }
}