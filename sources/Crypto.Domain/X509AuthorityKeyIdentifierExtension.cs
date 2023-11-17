using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DustInTheWind.Crypto.Domain;

public class X509AuthorityKeyIdentifierExtension : X509Extension
{
    private static Oid AuthorityKeyIdentifierOid => new("2.5.29.35");

    public bool IsEmpty => RawData.Length == 0;

    public X509AuthorityKeyIdentifierExtension(X509Certificate2 certificateAuthority, bool critical)
        : base(AuthorityKeyIdentifierOid, BuildData(certificateAuthority), critical)
    {
    }

    private static byte[] BuildData(X509Certificate2 certificateAuthority)
    {
        // AuthorityKeyIdentifier is "KeyID=<subject key identifier>"

        X509Extension subjectKeyIdentifier = null;

        foreach (X509Extension extension in certificateAuthority.Extensions)
        {
            if (extension.Oid?.Value == OidValues.SubjectKeyIdentifier)
                subjectKeyIdentifier = extension;
        }

        if (subjectKeyIdentifier == null)
            return Array.Empty<byte>();

        byte[] issuerSubjectKey = subjectKeyIdentifier.RawData;
        ArraySegment<byte> segment = new(issuerSubjectKey, 2, issuerSubjectKey.Length - 2);
        byte[] authorityKeyIdentifier = new byte[segment.Count + 4];

        authorityKeyIdentifier[0] = 0x30;
        authorityKeyIdentifier[1] = 0x16;
        authorityKeyIdentifier[2] = 0x80;
        authorityKeyIdentifier[3] = 0x14;

        segment.CopyTo(authorityKeyIdentifier, 4);

        return authorityKeyIdentifier;
    }
}