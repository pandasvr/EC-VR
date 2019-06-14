<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$idRoom = $_REQUEST['idRoom'];
$whiteboard = $_REQUEST['whiteboard'];
$postIt = $_REQUEST['postIt'];
$mediaProjection = $_REQUEST['mediaProjection'];
$chatNonVr = $_REQUEST['chatNonVr'];
$environnement_id = $_REQUEST['environnement_id'];


$resultat = $pdo->ModifyRoom($idRoom, $whiteboard, $postIt, $mediaProjection, $chatNonVr, $environnement_id);

echo $resultat;
?>