<?php
class PdoUnity
{   		
      	private static $server='mysql:host=192.168.0.105';
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
	public function signin($userName)
	{
		$resultat=PdoUnity::$myPdo->prepare("SELECT users.idUser, users.userName, users.cryptPassword, users.userEmail, users.userLevel, userlevel.labelUserLevel FROM users, userlevel where users.userLevel=userlevel.idUserLevel and users.userName = :userName");
		$resultat->bindParam(':userName', $userName);
		$resultat->execute();
		$return = $resultat->fetch();
		return $return;
	}

/**
 * Fonction qui vérifie le userName et le password pour la connexion au site
 */
	public function signup($userName, $cryptPassword, $userEmail, $userLevel)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO users(userName, cryptPassword, userEmail, userLevel) VALUES (:userName, :cryptPassword, :userEmail, :userLevel)");
		$resultat->bindParam(':userName', $userName);
		$resultat->bindParam(':cryptPassword', $cryptPassword);
		$resultat->bindParam(':userEmail', $userEmail);
		$resultat->bindParam(':userLevel', $userLevel);
		$resultat->execute();
		return $resultat;
	}

/**
 * Fonction qui vérifie que le userName n'existe pas déjà
 */
	public function requestUsername($userName)
	{
		$req="SELECT * FROM users where userName = '".$userName."'";
		$resultat=PdoUnity::$myPdo->query($req)->fetch();
		return $resultat;
	}

}
?>