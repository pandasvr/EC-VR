<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$name = $_REQUEST['name'];                              

$resultat = $pdo->userConnect($name);

echo $resultat['passwordUser'];
?>