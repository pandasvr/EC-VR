<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idUser = $_REQUEST['idUser'];

$resultat = $pdo->GetAllRoomOfUser($idUser);

echo json_encode($resultat);
?>