using System.Collections;
using UnityEngine;

public class iZombieSniperPerfabManager : MonoBehaviour
{
	private GameObject m_BloodEffect;

	private GameObject m_MachineGunDead;

	private GameObject m_GunHitEffect;

	private GameObject m_BulletEffect;

	private GameObject m_GunFlightEffect;

	private GameObject m_ThrowFlightEffect;

	private GameObject m_RocketFire;

	private GameObject m_MachineFire;

	private GameObject m_RocketFireBlue;

	private GameObject m_ZombieFrame;

	private GameObject m_ZombieNurseFrame;

	private GameObject m_ZombiePoliceFrame;

	private GameObject m_Zombie;

	private GameObject m_ZombieFat;

	private GameObject m_ZombieDog;

	private GameObject m_ZombieNurse;

	private GameObject m_ZombiePolice;

	private GameObject m_ZombieSwat;

	private GameObject m_ZombiePredator;

	private GameObject m_ZombiePop;

	public Hashtable mapWeapon;

	public GameObject m_Boom01
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/burst_01");
		}
	}

	public GameObject m_Boom02
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/burst_02");
		}
	}

	public GameObject m_BoomBlue01
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/burst_blue_01");
		}
	}

	public GameObject m_BoomBlue02
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/burst_blue_02");
		}
	}

	public GameObject m_BoomThrowMine
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/burst_throwmine");
		}
	}

	public GameObject m_TruckBoom
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/truck explosion");
		}
	}

	public GameObject BloodEffect
	{
		get
		{
			if (m_BloodEffect == null)
			{
				m_BloodEffect = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/svd attack");
			}
			return m_BloodEffect;
		}
	}

	public GameObject MachineGunDead
	{
		get
		{
			if (m_MachineGunDead == null)
			{
				m_MachineGunDead = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/machine_attack");
			}
			return m_MachineGunDead;
		}
	}

	public GameObject GunHitEffect
	{
		get
		{
			if (m_GunHitEffect == null)
			{
				m_GunHitEffect = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/cement");
			}
			return m_GunHitEffect;
		}
	}

	public GameObject BulletEffect
	{
		get
		{
			if (m_BulletEffect == null)
			{
				m_BulletEffect = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/Bullet");
			}
			return m_BulletEffect;
		}
	}

	public GameObject GunFlightEffect
	{
		get
		{
			if (m_GunFlightEffect == null)
			{
				m_GunFlightEffect = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/svd fire");
			}
			return m_GunFlightEffect;
		}
	}

	public GameObject ThrowFlightEffect
	{
		get
		{
			if (m_ThrowFlightEffect == null)
			{
				m_ThrowFlightEffect = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/thrower_fire");
			}
			return m_ThrowFlightEffect;
		}
	}

	public GameObject RocketFire
	{
		get
		{
			if (m_RocketFire == null)
			{
				m_RocketFire = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/RocketFire");
			}
			return m_RocketFire;
		}
	}

	public GameObject MachineFire
	{
		get
		{
			if (m_MachineFire == null)
			{
				m_MachineFire = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/machine_fire");
			}
			return m_MachineFire;
		}
	}

	public GameObject RocketFireBlue
	{
		get
		{
			if (m_RocketFireBlue == null)
			{
				m_RocketFireBlue = (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/RocketFireBlue");
			}
			return m_RocketFireBlue;
		}
	}

	public GameObject m_CinderEffect
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/Clinder");
		}
	}

	public GameObject m_ShadowEffect
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/Shadow");
		}
	}

	public GameObject m_TruckFire
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/combustion");
		}
	}

	public GameObject m_ContainerSmoke
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/container smoke");
		}
	}

	public GameObject m_ContainerAnim
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/Zbs-container");
		}
	}

	public GameObject HelpAnim
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/help_pfb");
		}
	}

	public GameObject ZombieFrame
	{
		get
		{
			if (m_ZombieFrame == null)
			{
				m_ZombieFrame = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPCFrame/_ZombieFrame1");
			}
			return m_ZombieFrame;
		}
	}

	public GameObject ZombieNurseFrame
	{
		get
		{
			if (m_ZombieNurseFrame == null)
			{
				m_ZombieNurseFrame = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPCFrame/_ZombieNurseFrame");
			}
			return m_ZombieNurseFrame;
		}
	}

	public GameObject ZombiePoliceFrame
	{
		get
		{
			if (m_ZombiePoliceFrame == null)
			{
				m_ZombiePoliceFrame = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPCFrame/_ZombiePoliceFrame");
			}
			return m_ZombiePoliceFrame;
		}
	}

	public GameObject Zombie
	{
		get
		{
			if (m_Zombie == null)
			{
				m_Zombie = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombieBone1");
			}
			return m_Zombie;
		}
	}

	public GameObject ZombieFat
	{
		get
		{
			if (m_ZombieFat == null)
			{
				m_ZombieFat = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombieFatBone1");
			}
			return m_ZombieFat;
		}
	}

	public GameObject ZombieDog
	{
		get
		{
			if (m_ZombieDog == null)
			{
				m_ZombieDog = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombieDog");
			}
			return m_ZombieDog;
		}
	}

	public GameObject ZombieNurse
	{
		get
		{
			if (m_ZombieNurse == null)
			{
				m_ZombieNurse = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombieNurse");
			}
			return m_ZombieNurse;
		}
	}

	public GameObject ZombiePolice
	{
		get
		{
			if (m_ZombiePolice == null)
			{
				m_ZombiePolice = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombiePolice");
			}
			return m_ZombiePolice;
		}
	}

	public GameObject ZombieSwat
	{
		get
		{
			if (m_ZombieSwat == null)
			{
				m_ZombieSwat = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombieSwat");
			}
			return m_ZombieSwat;
		}
	}

	public GameObject ZombiePredator
	{
		get
		{
			if (m_ZombiePredator == null)
			{
				m_ZombiePredator = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombiePredator");
			}
			return m_ZombiePredator;
		}
	}

	public GameObject ZombiePop
	{
		get
		{
			if (m_ZombiePop == null)
			{
				m_ZombiePop = (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/ZombiePop");
			}
			return m_ZombiePop;
		}
	}

	public GameObject Innocents
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/InnocentBone1");
		}
	}

	public GameObject InnocentsDoctor
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/InnocentDoctor");
		}
	}

	public GameObject InnocentsPastor
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/InnocentPastor");
		}
	}

	public GameObject InnocentsPlumber
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/InnocentPlumber");
		}
	}

	public GameObject Rocket01
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/Rocket");
		}
	}

	public GameObject RocketBlue
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/RocketBlue");
		}
	}

	public GameObject AC130
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/AC130");
		}
	}

	public GameObject OilCan
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/NPC/OilCan");
		}
	}

	public GameObject TruckScene1
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/TruckScene1");
		}
	}

	public GameObject truck
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/truck");
		}
	}

	public GameObject truck_01
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/truck_01");
		}
	}

	public GameObject Market
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/Market");
		}
	}

	public GameObject Market_01
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/Market_01");
		}
	}

	public GameObject ThrowMine
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/ThrowMine");
		}
	}

	public GameObject CGStation
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/CustomCG/Station_CG");
		}
	}

	public GameObject CGOP
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/CustomCG/OP_CG");
		}
	}

	public GameObject PlayerPlane
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Others/PlayerPlane");
		}
	}

	public GameObject AppearPredator
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/CustomCG/AppearPredator_CG");
		}
	}

	public GameObject AppearWaWa
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/CustomCG/AppearWaWa_CG");
		}
	}

	public GameObject WaWaSquare
	{
		get
		{
			return (GameObject)Resources.Load("ZombieSniper/Perfabs/Effect/WaWaSquare");
		}
	}

	private void Awake()
	{
		GameObject gameObject = null;
		gameObject = BloodEffect;
		gameObject = GunHitEffect;
		gameObject = BulletEffect;
		gameObject = GunFlightEffect;
		gameObject = Zombie;
		gameObject = ZombieFat;
		gameObject = ZombieDog;
		gameObject = ZombieNurse;
		gameObject = ZombiePolice;
		gameObject = ZombieSwat;
		mapWeapon = new Hashtable();
		mapWeapon.Add(1, "HandSVD");
		mapWeapon.Add(2, "HandM110");
		mapWeapon.Add(3, "HandMcmillan_Tac_50");
		mapWeapon.Add(4, "HandAS50");
		mapWeapon.Add(5, "HandM99_416_Barrett");
		mapWeapon.Add(6, "HandACCURACY_INTERNATIONAL_AWSM");
		mapWeapon.Add(7, "HandM107");
		mapWeapon.Add(8, "HandM200");
		mapWeapon.Add(9, "HandM82A1");
		mapWeapon.Add(10, "HandM82A1");
		mapWeapon.Add(11, "HandMini-Uzi");
		mapWeapon.Add(12, "HandG36c");
		mapWeapon.Add(13, "HandM4A1");
		mapWeapon.Add(14, "HandG3");
		mapWeapon.Add(15, "HandMP5");
		mapWeapon.Add(16, "HandRPD");
		mapWeapon.Add(17, "HandAK47");
		mapWeapon.Add(18, "HandAK47");
		mapWeapon.Add(19, "HandM60E4");
		mapWeapon.Add(20, "HandM60E4");
		mapWeapon.Add(21, "HandM60E4");
		mapWeapon.Add(22, "HandM16A4");
		mapWeapon.Add(23, "HandRPG-7");
		mapWeapon.Add(24, "HandAT4");
		mapWeapon.Add(25, "HandM202A1");
		mapWeapon.Add(26, "HandM202A1");
		mapWeapon.Add(27, "HandM99_416_Barrett");
		mapWeapon.Add(28, "HandACCURACY_INTERNATIONAL_AWSM");
		mapWeapon.Add(29, "HandM200");
		mapWeapon.Add(30, "HandM82A1");
		mapWeapon.Add(31, "HandMP5");
		mapWeapon.Add(32, "HandRPD");
		mapWeapon.Add(33, "HandM60E4");
		mapWeapon.Add(34, "HandM16A4");
		mapWeapon.Add(35, "HandM202A1");
		mapWeapon.Add(36, "HandRPG-7_02");
		mapWeapon.Add(37, "HandAT4_02");
		mapWeapon.Add(38, "HandM202A1_02");
		mapWeapon.Add(40, "HandM2BH");
		mapWeapon.Add(41, "HandMine_Launcher");
	}

	public GameObject GetWeaponPerfab(int nWeaponID)
	{
		if (mapWeapon == null)
		{
			return null;
		}
		if (!mapWeapon.ContainsKey(nWeaponID))
		{
			return null;
		}
		return (GameObject)Resources.Load("ZombieSniper/Perfabs/Weapon/" + (string)mapWeapon[nWeaponID]);
	}
}
