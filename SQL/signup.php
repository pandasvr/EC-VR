<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$userName = $_REQUEST['userName'];
$cryptPassword = $_REQUEST['cryptPassword'];
$userEmail = $_REQUEST['userEmail'];
$userLevel = $_REQUEST['userLevel'];

$resultat = $pdo->SignUp($userName, $cryptPassword, $userEmail, $userLevel);

echo $resultat;
?>