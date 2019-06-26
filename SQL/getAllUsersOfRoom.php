<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idUser = $_REQUEST['idRoom'];

$resultat = $pdo->GetAllUsersOfRoom($idRoom);

echo json_encode($resultat);
?>