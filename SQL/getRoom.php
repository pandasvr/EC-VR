<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idRoom = $_REQUEST['idRoom'];

$resultat = $pdo->GetRoom($idRoom);

echo json_encode($resultat);
?>