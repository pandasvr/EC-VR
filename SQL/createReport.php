<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$pathReport = $_REQUEST['pathReport'];
$dateReport = $_REQUEST['dateReport'];
$idRoom = $_REQUEST['idRoom'];

$idReport = $pdo->CreateReport($pathReport, $dateReport);


$listUser = $pdo->GetAllUsersOfRoom($idRoom);

foreach ($listUser as $user) {
    $pdo->CreateReceiver($user['idUser'], $idReport);
}
?>