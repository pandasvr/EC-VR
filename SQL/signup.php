<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$cryptPassword = $_REQUEST['cryptPassword'];
$userEmail = $_REQUEST['userEmail'];


$resultat = $pdo->signup($userName, $cryptPassword, $userEmail);

if($resultat){
	$return = true;
}else{
	$return = false;
}

echo $return;
?>