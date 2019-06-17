<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$roomName = $_REQUEST['roomName'];

$resultat = $pdo->GetRoomByName($roomName);

echo json_encode($resultat);
?>