<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$cryptPassword = $_REQUEST['cryptPassword'];
$userEmail = $_REQUEST['userEmail'];
$userLevel = 2;

$resultat = $pdo->signup($userName, $cryptPassword, $userEmail, $userLevel);

echo $resultat;
?>