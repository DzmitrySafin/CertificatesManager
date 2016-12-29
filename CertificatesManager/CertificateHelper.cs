using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace CertificatesManager
{
    static class CertificateHelper
    {
        public static X509Certificate2 GenerateCertificate(string subject, DateTime validFrom, DateTime validTo)
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));

            var keypair = keypairgen.GenerateKeyPair();

            var cn = new X509Name("CN=" + subject);
            var sn = BigInteger.ProbablePrime(120, new Random());

            var gen = new X509V3CertificateGenerator();
            gen.SetSerialNumber(sn);
            gen.SetSubjectDN(cn);
            gen.SetIssuerDN(cn);
            gen.SetNotAfter(validTo);
            gen.SetNotBefore(validFrom);
            gen.SetSignatureAlgorithm("SHA1withRSA");
            gen.SetPublicKey(keypair.Public);

            gen.AddExtension(X509Extensions.AuthorityKeyIdentifier.Id, false, new AuthorityKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keypair.Public), new GeneralNames(new GeneralName(cn)), sn));
            gen.AddExtension(X509Extensions.ExtendedKeyUsage.Id, false, new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));

            return new X509Certificate2(DotNetUtilities.ToX509Certificate(gen.Generate(keypair.Private))) { PrivateKey = ToAsymmetricKey(keypair.Private as RsaPrivateCrtKeyParameters), FriendlyName = subject };
        }

        private static AsymmetricAlgorithm ToAsymmetricKey(RsaPrivateCrtKeyParameters privateKey)
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = Guid.NewGuid().ToString(),
                KeyNumber = (int)KeyNumber.Exchange,
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            var rsaProvider = new RSACryptoServiceProvider(cspParams);

            var parameters = new RSAParameters
            {
                Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
                P = privateKey.P.ToByteArrayUnsigned(),
                Q = privateKey.Q.ToByteArrayUnsigned(),
                DP = privateKey.DP.ToByteArrayUnsigned(),
                DQ = privateKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privateKey.QInv.ToByteArrayUnsigned(),
                D = privateKey.Exponent.ToByteArrayUnsigned(),
                Exponent = privateKey.PublicExponent.ToByteArrayUnsigned()
            };

            rsaProvider.ImportParameters(parameters);
            return rsaProvider;
        }
    }
}
