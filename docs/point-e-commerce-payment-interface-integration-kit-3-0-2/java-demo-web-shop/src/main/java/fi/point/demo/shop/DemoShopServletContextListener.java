package fi.point.demo.shop;

import java.io.IOException;
import java.io.InputStream;
import java.security.KeyStore;
import java.security.PrivateKey;
import java.security.PublicKey;
import java.security.Security;

import javax.servlet.ServletContextEvent;
import javax.servlet.ServletContextListener;

import org.bouncycastle.jce.provider.BouncyCastleProvider;

/**
 * Servlet configuration listener.
 * 
 * @author Janis Stasans
 * 
 */
public class DemoShopServletContextListener implements ServletContextListener {

    private static final String PAYMENT_SITE_URL = "https://epayment-test.point.fi/pw/payment";
    private static final String POINT_CERTIFICATE_ALIAS = "epayment.point.fi";

    //private static final String PAYMENT_SITE_URL = "http://127.0.0.1:8080/pw/payment";
    //private static final String POINT_CERTIFICATE_ALIAS = "epayment.point.fi";

    //private static final String PAYMENT_SITE_URL = "https://t1-dmz-peosweb-1/peos-payment-web/payment";
    //private static final String POINT_CERTIFICATE_ALIAS = "peossign12350597g201105";

    private static final String SHOP_SITE_URL = "http://127.0.0.1:8081/demo-shop";
    private static final String MERCHANT_AGREEMENT_CODE = "demo-merchant-agreement";

    private static final String SHOP_KEYSTORE_FILE = "demo-merchant-agreement.jks";
    
    private static final String KEYSTORE_PASSWORD = "password";
    private static final String KEY_PASSWORD = "password";
    private static final String SHOP_CERTIFICATE_ALIAS = "demo-merchant-agreement";

    private PrivateKey shopPrivateKey;
    private PublicKey paymentPagePublicKey;

    /**
     * Constructor for setting up environment.
     */
    public DemoShopServletContextListener() {
        super();

        Security.addProvider(new BouncyCastleProvider());

        try {
            final KeyStore keyStore = loadKeyStoreFomResources(SHOP_KEYSTORE_FILE, "jks", KEYSTORE_PASSWORD);
            shopPrivateKey = (PrivateKey) keyStore.getKey(SHOP_CERTIFICATE_ALIAS, KEY_PASSWORD.toCharArray());
            paymentPagePublicKey = keyStore.getCertificate(POINT_CERTIFICATE_ALIAS).getPublicKey();
        } catch (Exception e) {
            throw new RuntimeException("Failed to configure environment", e);
        }

    }

    @Override
    public final void contextInitialized(final ServletContextEvent sce) {
        sce.getServletContext().setAttribute("shop-private-key", shopPrivateKey);
        sce.getServletContext().setAttribute("payment-page-public-key", paymentPagePublicKey);
        sce.getServletContext().setAttribute("payment-page-url", PAYMENT_SITE_URL);
        sce.getServletContext().setAttribute("shop-url", SHOP_SITE_URL);
        sce.getServletContext().setAttribute("merchant-agreement-code", MERCHANT_AGREEMENT_CODE);
    }

    @Override
    public void contextDestroyed(final ServletContextEvent sce) {
    }

    /**
     * Loads keystore from path specified in resources.
     * 
     * @param filename
     *            keystore filename
     * @param keyStoretype
     *            keystore type
     * @param keyStorePassword
     *            keystore password
     * @return loaded keystore
     * @throws java.io.IOException
     *             failed to read keystore
     */
    private KeyStore loadKeyStoreFomResources(final String filename, final String keyStoretype,
            final String keyStorePassword) throws IOException {

        InputStream keyStoreInputStream = null;
        try {
            final KeyStore keyStore = KeyStore.getInstance(keyStoretype);
            keyStoreInputStream = getClass().getResourceAsStream("/" + filename);
            keyStore.load(keyStoreInputStream, keyStorePassword.toCharArray());
            return keyStore;
        } catch (Exception e) {
            throw new RuntimeException("Failed to load keystore " + filename);
        } finally {
            if (keyStoreInputStream != null) {
                keyStoreInputStream.close();
            }
        }
    }

}
