package fi.point.generator;

import org.bouncycastle.asn1.x509.X509Name;
import org.bouncycastle.jce.provider.BouncyCastleProvider;
import org.bouncycastle.openssl.PEMWriter;
import org.bouncycastle.x509.X509V3CertificateGenerator;

import java.io.File;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.math.BigInteger;
import java.security.KeyPair;
import java.security.KeyStore;
import java.security.Security;
import java.security.cert.Certificate;
import java.security.cert.X509Certificate;
import java.util.Calendar;
import java.util.Scanner;

/**
 * Simple key pair generation application.
 * @author  Janis Satasans, Tommi Laukkanen
 */
public class KeyPairGenerator {
     /** The asymmetric algorithm to use. */
    private static final String ASYMMETRIC_ALG = "RSA";
    /** Public key size. */
    private static final int KEY_SIZE = 1024;
    /** Certificate validity period. */
    private static final int CERTIFICATE_VALIDITY_TIME_YEARS = 99;

    /**
     * Application entry point.
     * @param args command line arguments
     * @throws IOException failed to write key files
     */
    public static void main(final String[] args) throws IOException {
        final KeyPairGenerator  generator = new KeyPairGenerator();

        System.out.println("Enter merchant agreement code:");
        final String dnName = new Scanner(System.in).next();
        System.out.println("Enter keystore password:");
        final String password = new String(System.console().readPassword());

        try {
           System.out.println("Generating key pair...");
           final String path =  getCurrentPath();
           generator.saveKeyPair(path, dnName, password);
           System.out.println("Key pair generation succeeded.");
           System.out.println("Keys saved in " + path);
        } catch (Exception e) {
            System.err.println("Key pair generation failed.");
            e.printStackTrace();
        }

        System.out.println("Press ENTER to exit");
        System.in.read();
    }

    /**
     * Saves key pair in specified location.
     * @param keyPairPath path where to save key pair
     * @param commonName certificate common name name
     * @param password key store password
     * @throws Exception key pair generation failed
     */
    public final void saveKeyPair(final String keyPairPath,
                                  final String commonName,
                                  final String password) throws Exception {

        final KeyPair keys = generateKeyPair(null, KEY_SIZE);
        final X509Certificate certificate = createCertificate(keys, commonName);
        final String keystoreFile = keyPairPath + "/" + commonName + ".p12";
        final String publicKeyFile = keyPairPath + "/" + commonName + "-public.pem";
        final String privateKeyFile = keyPairPath + "/" + commonName + "-private.pem";
        final char[] keystorePassword = password.toCharArray();

        // Create or update key store.
        final File ksFile = new File(keystoreFile);
        final KeyStore ks = KeyStore.getInstance("pkcs12");
        if (ksFile.exists()) {

            System.out.println("Keystore:");
            System.out.println(keystoreFile);
            System.out.println("Already exists overwrite Y/N?");
            final String choice = new Scanner(System.in).next();
            if (choice.equalsIgnoreCase("n")) {
                throw new RuntimeException("Aborted");
            }
            if (!ksFile.delete()) {
                System.err.println("Failed to remove existing keystore " + keystoreFile + ".");
                System.exit(1);
            }
        }

        ks.load(null, keystorePassword);

        final KeyStore.PrivateKeyEntry privateKeyEntry =
                new KeyStore.PrivateKeyEntry(keys.getPrivate(), new Certificate[]{certificate});
        ks.setEntry(commonName, privateKeyEntry, new KeyStore.PasswordProtection(keystorePassword));

        final FileOutputStream keyStoreOutputStream = new FileOutputStream(ksFile);
        ks.store(keyStoreOutputStream, keystorePassword);
        keyStoreOutputStream.close();

        PEMWriter pemWriter = null;
        // Store Public Key.
        try {
            pemWriter = new PEMWriter(new FileWriter(publicKeyFile));
            pemWriter.writeObject(keys.getPublic());
        } catch (IOException e) {
            System.err.println("Failed to write public key.");
            System.exit(1);
        } finally {
            pemWriter.close();
        }

        // Store Private Key.
        try {
            pemWriter = new PEMWriter(new FileWriter(privateKeyFile));
            pemWriter.writeObject(keys.getPrivate());
        } catch (IOException e) {
            System.err.println("Failed to write private key.");
            System.exit(1);
        } finally {
            pemWriter.close();
        }
    }

    /**
     * Generate key pair for new certificate.
     * @param securityProvider The security provider or null if default one is to be used.
     * @param asymmetricKeySize the asymmetric key size.
     * @return The KeyPair which was generated.
     * @throws Exception If key pair generation fails.
     */
    private  KeyPair generateKeyPair(final String securityProvider,
            final int asymmetricKeySize) throws Exception {
        if (Security.getProvider("BC") == null) {
            Security.addProvider(new BouncyCastleProvider());
        }

        java.security.KeyPairGenerator keyPairGen;
        if (securityProvider != null) {
            keyPairGen = java.security.KeyPairGenerator.getInstance(ASYMMETRIC_ALG, securityProvider);
        } else {
            keyPairGen = java.security.KeyPairGenerator.getInstance(ASYMMETRIC_ALG);
        }

        keyPairGen.initialize(asymmetricKeySize);
        return  keyPairGen.generateKeyPair();
    }

    /**
     * Creates certificate from key pair.
     * @param keys key pair.
     * @param commonName certificate common name
     * @return generated certificate
     * @throws Exception if exception occurs in certificate generation process.
     */
    private  X509Certificate createCertificate(final KeyPair keys, final String commonName) throws Exception {

        final X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();
        final Calendar calendar = Calendar.getInstance();
        final X509Name name = new X509Name("CN=" + commonName);
        certificateGenerator.reset();
        certificateGenerator.setIssuerDN(name);
        calendar.setTimeInMillis(System.currentTimeMillis());
        certificateGenerator.setNotBefore(calendar.getTime());
        calendar.add(Calendar.YEAR, CERTIFICATE_VALIDITY_TIME_YEARS);
        certificateGenerator.setNotAfter(calendar.getTime());
        certificateGenerator.setPublicKey(keys.getPublic());
        certificateGenerator.setSerialNumber(BigInteger.valueOf(System.currentTimeMillis()));
        certificateGenerator.setSubjectDN(name);
        certificateGenerator.setSignatureAlgorithm("SHA1withRSA");

        return certificateGenerator.generate(keys.getPrivate());

    }


    /**
     * Gets current path of running program.
     * @return Current path
     * @throws Exception failed to get current path
     */
    private static String getCurrentPath() throws Exception {
        final String currentPath =
            KeyPairGenerator.class.getProtectionDomain().getCodeSource().getLocation().getPath();
    int sep = currentPath.lastIndexOf("/");
    return currentPath.substring(0, sep);
  }

}
