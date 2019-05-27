<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$name = $_REQUEST['name'];                              

$resultat = $pdo->SignIn($name);

if($resultat != false){
	echo json_encode($resultat);
}
?>