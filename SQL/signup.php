<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$userPassword = $_REQUEST['userPassword'];

$resultat = $pdo->signup($userName, $userPassword);

if($resultat){
	$return = true;
}else{
	$return = false;
}

echo $return;
?>