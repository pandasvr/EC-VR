<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$cryptPassword = $_REQUEST['cryptPassword'];
$userEmail = $_REQUEST['userEmail'];
$userFirstName = $_REQUEST['userFirstName'];
$userLastName = $_REQUEST['userLastName'];
$userLevel = $_REQUEST['userLevel'];

$resultat = $pdo->signup($userName, $cryptPassword, $userEmail, $userLevel, $userFirstName, $userLastName);

echo $resultat;
?>