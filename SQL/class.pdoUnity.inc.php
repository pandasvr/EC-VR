<?php
class PdoUnity
{   		
      	private static $server='mysql:host=localhost';
      	private static $bdd='dbname=ecvr_db';
      	private static $user='root';
      	private static $pw='';
		private static $myPdo;
		private static $myPdoUnity = null;
/**
 * Constructeur privé, crée l'instance de PDO qui sera sollicitée
 * pour toutes les méthodes de la classe
 */				
	private function __construct()
	{
    		PdoUnity::$myPdo = new PDO(PdoUnity::$server.';'.PdoUnity::$bdd, PdoUnity::$user, PdoUnity::$pw); 
			PdoUnity::$myPdo->query("SET CHARACTER SET utf8");
			PdoUnity::$myPdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
			PdoUnity::$myPdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);
	}
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
		$req="SELECT * FROM users where userName = '".$userName."'";
		$resultat=PdoUnity::$myPdo->query($req)->fetch();
		return $resultat;
	}

/**
 * Fonction qui vérifie le userName et le password pour la connexion au site
 */
	public function signup($userName, $userPassword)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO users(userName, userPassword) VALUES (:userName, :userPassword)");
		$resultat->bindParam(':userName', $userName);
		$resultat->bindParam(':userPassword', $userPassword);
		$resultat->execute();
		return $resultat;
	}
}
?>