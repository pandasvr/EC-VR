<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idUser = $_REQUEST['idUser'];
$idReport = $_REQUEST['idReport'];

$resultat = $pdo->CreateReceiver($idUser, $idReport);

echo $resultat;
?>