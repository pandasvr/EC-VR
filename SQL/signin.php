<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$name = $_REQUEST['name'];                              

$resultat = $pdo->SignIn($name);
header('Content-Type: application/json');
$resultat = json_encode($resultat, JSON_UNESCAPED_SLASHES);

/**
*$resultat = preg_replace('/"([a-zA-Z]+[a-zA-Z0-9]*)":/','$1:',$resultat);
*$resultat = str_replace('"', "'", $resultat);
*/


echo $resultat;

?>