
<?php
include 'signatureutil.php';


 $fields  = buildfields();
 $datatosign = formatparamters($fields);
 $fields["s-t-256-256_signature-one"] = generatesignature($datatosign);
 renderorder($fields);


?>


<?php
function renderorder($fields)
{
?>

<!-- header html block-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<title>
Test Order Page
</title>
</head>
<body>
<!--header html block end-->

<?php
include 'configvars.php';

echo '<form id="integration-form" action="' .$pay_page_url .'" method="post">' ."\n";
echo "<h1>Test Shop Order</h1>\n";
echo "<table>\n";

  foreach ($fields as $key => $value)
  {
    echo  '<tr><td>' .$key .'</td><td><input type="text" name="' .$key .'"value="' .$value  .'" /></td></tr>' ."\n";
  }

echo "</table>\n";
?>

<!--footer html block -->

 <input type="submit" name="s-t-1-40_submit" value="Submit" />
</form>
</body>
</html>

<!--footer html block end -->

<?php
}
?>


<?php
function buildfields() {

 include 'configvars.php';
 $fields = array();

  //date_default_timezone_set('Europe/Helsinki');
  date_default_timezone_set('UTC');
  $orderdate = date("Y-m-d H:i:s");

    $fields['i-f-1-11_interface-version'] = "3";
    $fields['i-f-1-3_order-currency-code'] = "978";
    $fields['i-t-1-11_bi-unit-count-0'] = "1";
    $fields['i-t-1-3_delivery-address-country-code'] = "246";
    $fields['i-t-1-4_bi-discount-percentage-0'] = "0";
    $fields['i-t-1-4_bi-vat-percentage-0'] = "2300";
    $fields['i-t-1-4_order-vat-percentage'] = "2300";
    $fields['l-f-1-20_order-gross-amount'] = "1230";
    $fields['l-f-1-20_order-net-amount'] = "1000";
    $fields['l-f-1-20_order-vat-amount'] = "230";
    $fields['l-t-1-20_bi-gross-amount-0'] = "123";
    $fields['l-t-1-20_bi-net-amount-0'] = "100";
    $fields['l-t-1-20_bi-unit-cost-0'] = "100";
    $fields['locale-f-2-5_payment-locale'] = "fi_FI";
    $fields['s-f-1-100_buyer-email-address'] = "john.smith@example.com";
    $fields['s-f-1-10_software-version'] = "1.0.1";
    $fields['s-f-1-30_buyer-first-name'] = "John";
    $fields['s-f-1-30_buyer-last-name'] = "Smith";
    $fields['s-f-1-30_software'] = "My Web Shop";
    $fields['s-f-1-36_merchant-agreement-code'] = $shop_merchant_agreement_code;
    $fields['s-f-1-36_order-number'] = sprintf("%.0f",microtime(true) * 1000000);

    $tokendata =  sprintf("%s;%s;%s",
                          $fields['s-f-1-36_merchant-agreement-code'],
                          $fields['s-f-1-36_order-number'],
                          $orderdate
                        );

    $fields['s-f-32-32_payment-token'] = strtoupper(substr(hash('sha256',$tokendata),0,32));

    $fields['s-f-5-128_cancel-url'] = $shop_cancel_url;
    $fields['s-f-5-128_error-url'] = $shop_cancel_url;
    $fields['s-f-5-128_expired-url'] = $shop_cancel_url;
    $fields['s-f-5-128_rejected-url'] = $shop_cancel_url;
    $fields['s-f-5-128_success-url'] = $shop_receipt_url;
    $fields['s-t-1-30_bi-name-0'] = "test-basket-item-0";
    $fields['s-t-1-30_buyer-phone-number'] = "+358 50 234234";
    $fields['s-t-1-30_delivery-address-city'] = "City";
    $fields['s-t-1-30_delivery-address-line-one'] = "Street Address #1";
    $fields['s-t-1-30_delivery-address-line-three'] = "Street Address #3";
    $fields['s-t-1-30_delivery-address-line-two'] = "Street Address #2";
    $fields['s-t-1-30_delivery-address-postal-code'] = "00234";
    $fields['s-t-1-36_order-note'] = "x213";
    $fields['t-f-14-19_order-timestamp'] = $orderdate;
    $fields['t-f-14-19_payment-timestamp'] = $orderdate;

    return $fields;

}

?>



