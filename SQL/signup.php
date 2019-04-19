<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$hashPassword = $_REQUEST['hashPassword'];
$userEmail = $_REQUEST['userEmail'];


$resultat = $pdo->signup($userName, $hashPassword, $userEmail);

if($resultat){
	$return = true;
}else{
	$return = false;
}

echo $return;
?>