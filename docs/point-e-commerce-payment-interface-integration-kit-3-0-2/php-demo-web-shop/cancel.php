<?php
/**
 * Demo cancel page implementation
 */


include 'signatureutil.php';

unset($_POST["s-t-1-40_shop-order__phase"]);
$signedformdata = $_POST;
$renderdata = $_POST;
unset($signedformdata["s-t-256-256_signature-one"]);
unset($signedformdata["s-t-256-256_signature-two"]);

verifysignature(formatparamters($signedformdata), $_POST["s-t-256-256_signature-one"]) ? $result = "valid" : $result = 'invalid';

$renderdata["signature-one-valid"] =  $result;
renderreceipt($renderdata);

   ?>

   <?php
   function renderreceipt($fields)
   {
   ?>

   <!-- header html block-->
   <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional" "http://www.w3.org/TR/html4/loose.dtd">
   <html>
   <head>
   <title>
   Demo Cancel Page
   </title>
   </head>
   <body>
   <!--header html block end-->

   <?php
   include 'configvars.php';

   echo '<form id="integration-form" action="order.php" method="post">' ."\n";
   echo "<h1>Demo Cancel Receip</h1>\n";
   echo "<table>\n";

 foreach ($fields as $key => $value)
 {
   echo  '<tr><td>' .$key .'</td><td><input type="text" name="' .$key .'"value="' .$value  .'" /></td></tr>' ."\n";
 }

   echo "</table>\n";
   ?>

   <!--footer html block -->

<input class="forward-button" type="submit" name="submit" value="New Order" />
</form>
   </body>
   </html>

   <!--footer html block end -->

   <?php
   }
   ?>



 
