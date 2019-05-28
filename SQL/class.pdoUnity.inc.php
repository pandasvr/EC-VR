<?php
class PdoUnity
{   		
      	private static $server='mysql:host=192.168.0.106';
      	private static $bdd='dbname=ecvr_db';
      	private static $user='ecvr_db';
      	private static $pw='!CapgeminiPandas4';
		private static $myPdo; //PHP Base de Données
		private static $myPdoUnity = null;
/**
 * Constructeur privé, crée l'instance de PDO qui sera sollicitée
 * pour toutes les méthodes de la classe
 */				
	//constructeur 
	private function __construct()
	{
    		PdoUnity::$myPdo = new PDO(PdoUnity::$server.';'.PdoUnity::$bdd, PdoUnity::$user, PdoUnity::$pw); //
			PdoUnity::$myPdo->query("SET CHARACTER SET utf8"); 
			PdoUnity::$myPdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION); 
			PdoUnity::$myPdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);//Pour faire apparaitreles différentes erreurs liées au code ou aux requêtes. 
	}

	//destructeur 
	public function _destruct()
	{
		PdoUnity::$myPdo = null;
	}
	
/**
 * Fonction statique qui crée l'unique instance de la classe
 */
	public static function getPdoUnity()
	{
		if(PdoUnity::$myPdoUnity == null)
		{
			PdoUnity::$myPdoUnity= new PdoUnity();
		}
		return PdoUnity::$myPdoUnity;
	}

	
/**
 * Fonction qui vérifie le userName et le password pour la connexion au site
 */
	public function SignIn($userName)
	{
		$resultat=PdoUnity::$myPdo->prepare("SELECT user.idUser, user.userName, user.cryptPassword, user.userEmail, user.userLevel, userlevel.labelUserLevel, user.userFirstName, user.userLastName FROM user, userlevel where user.userLevel=userlevel.idUserLevel and user.userName = :userName");
		$resultat->bindParam(':userName', $userName);
		$resultat->execute();
		$return = $resultat->fetch();
		return $return;
	}

/**
 * Fonction qui enregistre l'utilisateur
 */
	public function signup($userName, $cryptPassword, $userEmail, $userLevel, $userFirstName, $userLastName)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO user(userName, cryptPassword, userEmail, userLevel, userFirstName, userLastName) VALUES (:userName, :cryptPassword, :userEmail, :userLevel, :userFirstName, :userLastName)");
		$resultat->bindParam(':userName', $userName);
		$resultat->bindParam(':cryptPassword', $cryptPassword);
		$resultat->bindParam(':userEmail', $userEmail);
		$resultat->bindParam(':userLevel', $userLevel);
		$resultat->bindParam(':userFirstName', $userFirstName);
		$resultat->bindParam(':userLastName', $userLastName);
		$resultat->execute();
		return $resultat;
	}

/**
 * Fonction qui vérifie que le userName n'existe pas déjà
 */
	public function RequestUsername($userName)
	{
		$req="SELECT * FROM user where userName = '".$userName."'";
		$resultat=PdoUnity::$myPdo->query($req)->fetch();
		return $resultat;
	}

/**
 * Fonction qui enregistre une room
 */
	public function CreateRoom($roomName, $userNumber, $whiteboard, $postIt, $mediaProjection, $chatNonVr, $environnement_id)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO room(roomName, maxPlayerRoom, whiteboard, postIt, mediaProjection, chatNonVr, environnement_id) VALUES (:roomName, :maxPlayerRoom, :whiteboard, :postIt, :mediaProjection, :chatNonVr, :environnement_id)");
		$resultat->bindParam(':roomName', $roomName);
		$resultat->bindParam(':maxPlayerRoom', $userNumber);
		$resultat->bindParam(':whiteboard', $whiteboard);
		$resultat->bindParam(':postIt', $postIt);
		$resultat->bindParam(':mediaProjection', $mediaProjection);
		$resultat->bindParam(':chatNonVr', $chatNonVr);
		$resultat->bindParam(':environnement_id', $environnement_id);
		$resultat->execute();
		return $resultat;
	}

	public function GetAllUsers()
	{
		$resultat=PdoUnity::$myPdo->prepare("SELECT idUser, userName FROM user ");
		$resultat->execute();
		$return = $resultat->fetchAll();
		return $return;
	}

}
?>