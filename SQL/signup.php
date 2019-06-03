<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userId = $_REQUEST['userId'];
$userName = $_REQUEST['userName'];
$cryptPassword = $_REQUEST['cryptPassword'];
$userEmail = $_REQUEST['userEmail'];
$userFirstName = $_REQUEST['userFirstName'];
$userLastName = $_REQUEST['userLastName'];
$userLevel = $_REQUEST['userLevel'];

$resultat = $pdo->signup($userId, $userName, $cryptPassword, $userEmail, $userLevel, $userFirstName, $userLastName);

echo $resultat;
?>