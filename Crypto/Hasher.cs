using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace ValourChain.Crypto;

public class Hasher
{
    private SHA256 _sha256 = SHA256.Create();

    public string GetRandomKeyPair()
    {
        
    }
}