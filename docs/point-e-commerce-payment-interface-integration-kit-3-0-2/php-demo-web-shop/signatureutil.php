<?php
/**
 * Created by JetBrains PhpStorm.
 * User: JST
 * Date: 11.8.9
 * Time: 19:29
 * To change this template use File | Settings | File Templates.
 */

function formatparamters($parameters) {
  $formatedresult = "";
  ksort($parameters);
  foreach ($parameters as $key => $value)  {
    $formatedresult .= sprintf("%s=%s;", $key, $value);
  }
  return $formatedresult;
}


/** Creates signature for data
 * @param $data data to creta
 * @return string
 */
function generatesignature($data) {
    include('Crypt/RSA.php');
    include 'configvars.php';

    $rsa = new Crypt_RSA();
    $rsa->setSignatureMode(CRYPT_RSA_SIGNATURE_PKCS1);
    $rsa->loadKey(file_get_contents($shop_private_key_file, true));
    $signature = $rsa->sign($data);

    return strtoupper(bin2hex($signature));
}

/** Verifies the data signature
 * @param $datatoverify data to verify signature for
 * @param $signaturedata signature data as hex string
 * @return bool true - signature valid, false - invalid
 */
function verifysignature($datatoverify, $signaturedata) {
    include('Crypt/RSA.php');
    include 'configvars.php';

    $rsa = new Crypt_RSA();
    $rsa->setSignatureMode(CRYPT_RSA_SIGNATURE_PKCS1);
    $rsa->loadKey(file_get_contents($pay_page_public_key_file, true));
    return $rsa->verify($datatoverify, pack("H*" , $signaturedata)) ? true : false;
}

?>
 
