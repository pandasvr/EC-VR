<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idUser = $_REQUEST['idUser'];
$idRoom = $_REQUEST['idRoom'];
$isCreator = $_REQUEST['isCreator'];

$resultat = $pdo->CreateInvite($idUser, $idRoom, $isCreator);

echo $resultat;
?>