<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();                           

$resultat = $pdo->GetAllUsers();

echo json_encode($resultat);
?>