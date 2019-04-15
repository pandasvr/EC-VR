<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$name = $_REQUEST['name'];
$password = $_REQUEST['password'];

$resultat = $pdo->userConnect($name, $password);
if(sizeof($resultat)>0){
	$return = "true";
}else{
	$return = "false";
}
echo $return;
?>