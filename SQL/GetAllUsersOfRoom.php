<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idUser = $_REQUEST['idRoom'];

$resultat = $pdo->GetAllRoomOfUser($idRoom);

echo json_encode($resultat);
?>