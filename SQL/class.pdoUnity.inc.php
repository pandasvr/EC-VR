<?php
class PdoUnity
{   		
      	private static $server='mysql:host=192.168.0.101';
      	private static $bdd='dbname=ecvr_db';
      	private static $user='ecvr_db';
      	private static $pw='!CapgeminiPandas4';
      	/*private static $user='root';
      	private static $pw='';*/
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
		$return = $resultat->fetch(PDO::FETCH_ASSOC);
		return $return;
	}

/**
 * Fonction qui enregistre l'utilisateur
 */
	public function signup($idUser, $userName, $cryptPassword, $userEmail, $userLevel, $userFirstName, $userLastName)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO user(idUser, userName, cryptPassword, userEmail, userLevel, userFirstName, userLastName) VALUES (:idUser, :userName, :cryptPassword, :userEmail, :userLevel, :userFirstName, :userLastName)");
		$resultat->bindParam(':idUser', $idUser);
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
	public function CreateRoom($roomName, $userNumber, $whiteboard, $postIt, $mediaProjection, $chatNonVr, $environnement_id, $userCreator_id)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO room(roomName, maxPlayerRoom, whiteboard, postIt, mediaProjection, chatNonVr, environnement_id, userCreator_id) VALUES (:roomName, :maxPlayerRoom, :whiteboard, :postIt, :mediaProjection, :chatNonVr, :environnement_id, :userCreator_id)");
		$resultat->bindParam(':roomName', $roomName);
		$resultat->bindParam(':maxPlayerRoom', $userNumber);
		$resultat->bindParam(':whiteboard', $whiteboard);
		$resultat->bindParam(':postIt', $postIt);
		$resultat->bindParam(':mediaProjection', $mediaProjection);
		$resultat->bindParam(':chatNonVr', $chatNonVr);
		$resultat->bindParam(':environnement_id', $environnement_id);
		$resultat->bindParam(':userCreator_id', $userCreator_id);
		$resultat->execute();
		$lastId = PdoUnity::$myPdo->lastInsertId();
		return $lastId;
	}

    /**
     * Fonction qui enregistre un compte-rendu
     */
    public function CreateReport($pathReport, $dateReport)
    {
        $resultat=PdoUnity::$myPdo->prepare("INSERT INTO report(pathReport, dateReport) VALUES (:pathReport, :dateReport)");
        $resultat->bindParam(':pathReport', $pathReport);
        $resultat->bindParam(':dateReport', $dateReport);
        $resultat->execute();
        $lastId = PdoUnity::$myPdo->lastInsertId();
        return $lastId;
    }

    /**
     * Fonction qui modifie une room
     */
    public function ModifyRoom($idRoom ,$whiteboard, $postIt, $mediaProjection, $chatNonVr, $environnement_id)
    {
        $resultat=PdoUnity::$myPdo->prepare("UPDATE room SET (whiteboard = :whiteboard, postIt = :postIt, mediaProjection = :mediaProjection, chatNonVr = :chatNonVr, environnement_id = :environnement_id) WHERE idRoom = :idRoom");
        $resultat->bindParam(':whiteboard', $whiteboard);
        $resultat->bindParam(':postIt', $postIt);
        $resultat->bindParam(':mediaProjection', $mediaProjection);
        $resultat->bindParam(':chatNonVr', $chatNonVr);
        $resultat->bindParam(':environnement_id', $environnement_id);
        $resultat->bindParam(':idRoom', $idRoom);
        $resultat->execute();
        return $resultat;
    }
/**
 * Fonction qui enregiste une invitation
 */
	public function CreateInvite($idUser, $idRoom, $isCreator)
	{
		$resultat=PdoUnity::$myPdo->prepare("INSERT INTO link_user_room(idUser, idRoom, isCreator) VALUES
			(:idUser, :idRoom, :isCreator)");
		$resultat->bindParam(':idUser', $idUser);
		$resultat->bindParam(':idRoom', $idRoom);
		$resultat->bindParam(':isCreator', $isCreator);
		$resultat->execute();
		return $resultat;
	}

    /**
     * Fonction qui enregiste les récepteurs d'un compte-rendu
     */
    public function CreateReceiver($idUser, $idReport)
    {
        $resultat=PdoUnity::$myPdo->prepare("INSERT INTO reportuser(idUser, idReport) VALUES
			(:idUser, :idReport)");
        $resultat->bindParam(':idUser', $idUser);
        $resultat->bindParam(':idReport', $idReport);
        $resultat->execute();
        return $resultat;
    }

/**
 * Fonction qui récupère la liste des users
 */
	public function GetAllUsers()
	{
		$resultat=PdoUnity::$myPdo->prepare("SELECT idUser, userFirstName, userLastName FROM user");
		$resultat->execute();
		$return = $resultat->fetchAll();
		return $return;
	}

/**
 * Fonction qui récupère la liste des rooms d'un user
 */
	public function GetAllRoomOfUser($idUser)
	{
		$resultat=PdoUnity::$myPdo->prepare("SELECT * FROM link_user_room WHERE idUser = :idUser");
        $resultat->bindParam(':idUser', $idUser);
		$resultat->execute();
		$return = $resultat->fetchAll();
		return $return;
	}


    /**
     * Fonction qui récupère la liste des users d'une room
     */
    public function GetAllUsersOfRoom($idRoom)
    {
        $resultat=PdoUnity::$myPdo->prepare("SELECT * FROM link_user_room WHERE idRoom = :idRoom");
        $resultat->bindParam(':idRoom', $idRoom);
        $resultat->execute();
        $return = $resultat->fetchAll();
        return $return;
    }

/**
 * Fonction qui récupère un salon
 */
    public function GetRoom($idRoom)
    {
        $resultat=PdoUnity::$myPdo->prepare("SELECT room.idRoom, room.roomName, room.maxPlayerRoom, room.whiteboard, room.postIt, room.mediaProjection, room.chatNonVr, room.environnement_id, environnement.labelEnvironnement, user.userName AS userCreatorName FROM room, user, environnement WHERE idRoom = :idRoom AND user.idUser = room.userCreator_id AND room.environnement_id = environnement.idEnvironnement");
        $resultat->bindParam(':idRoom', $idRoom);
        $resultat->execute();
        $return = $resultat->fetchAll();
        return $return;
    }

 /**
 * Fonction qui récupère un salon par son nom
 */
    public function GetRoomByName($roomName)
    {
        $resultat=PdoUnity::$myPdo->prepare("SELECT room.idRoom, room.roomName, room.maxPlayerRoom, room.whiteboard, room.postIt, room.mediaProjection, room.chatNonVr, room.environnement_id, user.userName AS userCreatorName FROM room, user WHERE roomName = :roomName AND user.idUser = room.userCreator_id");
        $resultat->bindParam(':roomName', $roomName);
        $resultat->execute();
        $return = $resultat->fetch();
        return $return;
    }

    /**
     * Fonction qui récupère un salon par son nom
     */
    public function GetReportByUser($idUser)
    {
        $resultat=PdoUnity::$myPdo->prepare("SELECT report.idReport, report.pathReport, report.dateReport FROM report, reportuser WHERE reportuser.idUser = :idUser AND report.idReport = reportuser.idReport");
        $resultat->bindParam(':idUser', $idUser);
        $resultat->execute();
        $return = $resultat->fetchAll();
        return $return;
    }
}
?>