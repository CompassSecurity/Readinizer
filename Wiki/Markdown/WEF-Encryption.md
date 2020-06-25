# Encryption of Event Logs
As already in the section \ref{RequirementsLimitations} \nameref{RequirementsLimitations} explained, the event logs in a Windows environment are encrypted by default using Kerberos. This section briefly explains which encryption standard is used and which strength it provides.

The following list shows all encryption types and their key strength supported for Kerberos:

| **Encryption Type** |**Description**|**Key Strength**|
| ------------------- |-------------| -----|
| DES_CBC_CRC | Data Encryption Standard with Cipher Block Chaining using the Cyclic Redundancy Check function | 56 bit |
| DES_CBC_MD5 | Data Encryption Standard with Cipher Block Chaining using the Message-Digest algorithm 5 checksum function     | 56 bit |
| RC4\_HMAC\_MD5 | Rivest Cipher 4 with Hashed Message Authentication Code using the Message-Digest algorithm 5 checksum function | 56 - 128 bit |
| AES128\_HMAC\_SHA1 | Advanced Encryption Standard in 128 bit cipher block with Hashed Message Authentication Code using the Secure Hash Algorithm (1) | 128 bit |
| AES256\_HMAC\_SHA1 | Advanced Encryption Standard in 256 bit cipher block with Hashed Message Authentication Code using the Secure Hash Algorithm (1) | 256 bit |


Since Windows 7 and Windows Server 2008, Microsoft has disabled the weak encryption types DES_CBC_CRC and DES_CBC_MD5 by default. These encryption types are since those versions deprecated but can still be activated manually for legacy support. Although, this is definitely not recommended!

The encryption type RC4_HMAC_MD5 can reach a strength of 128 bit, but both sides (client / server) must support the full-strength encryption. Otherwise the weak encryption type is used as described in RFC4757<sup>1</sup>:

> _A Kerberos client and server can negotiate over key length if they are using mutual authentication.  If the client is unable to perform full-strength encryption, it may propose a key in the "subkey" field of the authenticator, using a weaker encryption type. [...]_ <sup>2</sup>}

Thus the encryption type RC4_HMAC_MD5 does not guarantee sufficiently strong encryption. Only the two encryption types AES128_HMAC_SHA1 and AES256_HMAC_SHA1 use a minimum key length of 128 bit. In principle, however, the strongest encryption is always automatically negotiated between both parties. Nevertheless it is recommended to only allow the two encryption types AES128_HMAC_SHA1 and AES256_HMAC_SHA1.

This can be achieved with the following GPO setting (`Computer Configuration > Policies > Windows Settings > Security Settings > Local Policies > Security Options > Network security: Configure encryption types allowed for Kerberos`):

![Kerberos encryption](https://github.com/clma91/Readinizer/blob/master/Wiki/Images/WEFManual/kerberos-encryption.png)

***
<sup>1</sup> [K.Jaganathan, L.Zhu, J.Brezak.The RC4-HMAC Kerberos Encryption Types Used by Microsoft Windows, December 2006](https://tools.ietf.org/html/rfc4757)

<sup>2</sup> [Section 6, Microsoft, Network security: Configure encryption types allowed for Kerberos, April 2017](https://docs.microsoft.com/en-us/windows/security/threat-protection/security-policy-settings/network-security-configure-encryption-types-allowed-for-kerberos)