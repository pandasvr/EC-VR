<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$pathReport = $_REQUEST['pathReport'];
$dateReport = $_REQUEST['dateReport'];



$resultat = $pdo->CreateReport($pathReport, $dateReport);

echo $resultat;
?>