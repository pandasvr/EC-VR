<?php
require_once("class.pdoUnity.inc.php");
$pdo = PdoUnity::getPdoUnity();

$name = $_REQUEST['userName'];                              

$resultat = $pdo->RequestUsername($name);


if (isset($resultat['userName']))
{
	$doesntExist="true";
}
else
{
	$doesntExist="false";
}


echo($doesntExist);
?>