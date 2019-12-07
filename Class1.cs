using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Project_Console
{
    class FriendlyCharacter
    {
        public enum StatusEffect
        {
            NONE,
            SLEEP,
            KO
        }

        #region Fields

        //next time, add "bool isTurn" "ConsoleColor textColor" "bool isTarget" "int damage" "bool[] statusImmunity"
        //"string chatOutput"
        //one character class w/ enums MonsterType & ClassType
        private string _name;
        private int _health;
        private int _mana;
        private int _attack;
        private int _defense;
        private int _magic;
        private int _resistance;
        private int _speed;
        private int _maxhealth;
        private int _maxmana;
        private int _baseattack;
        private int _basedefense;
        private int _basemagic;
        private int _baseresistance;
        private int _basespeed;
        private StatusEffect _status;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                if (_health <= 0)
                {
                    _health = 0;
                    _status = FriendlyCharacter.StatusEffect.KO;

                }
            }
        }

        public int Mana
        {
            get { return _mana; }
            set { _mana = value; }
        }

        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public int Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        public int Magic
        {
            get { return _magic; }
            set { _magic = value; }
        }

        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int MaxHealth
        {
            get { return _maxhealth; }
            set { _maxhealth = value; }
        }

        public int MaxMana
        {
            get { return _maxmana; }
            set { _maxmana = value; }
        }

        public int BaseAttack
        {
            get { return _baseattack; }
            set { _baseattack = value; }
        }

        public int BaseDefense
        {
            get { return _basedefense; }
            set { _basedefense = value; }
        }

        public int BaseMagic
        {
            get { return _basemagic; }
            set { _basemagic = value; }
        }

        public int BaseResistance
        {
            get { return _baseresistance; }
            set { _baseresistance = value; }
        }

        public int BaseSpeed
        {
            get { return _basespeed; }
            set { _basespeed = value; }
        }

        public StatusEffect Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        #region Public Methods

        public void InitializeCharacter()
        {
            _health = MaxHealth;
            _mana = MaxMana;
            _attack = BaseAttack;
            _defense = BaseDefense;
            _magic = BaseMagic;
            _resistance = BaseResistance;
            _speed = BaseSpeed;
            _status = StatusEffect.NONE;
        }

        public void DoAttack(EnemyCharacter target)
        {
            int damage = _attack - target.Defense;
            if (damage < 0) damage = 0;
            target.Health = (target.Health - damage);
        }

        public void DoAttack(FriendlyCharacter target)
        {
            target.Health = target.Health - 0;
        }

        public string ReportDamage(EnemyCharacter target)
        {
            int damage = _attack - target.Defense;
            if (damage < 0) damage = 0;
            return damage.ToString();
        }

        public string ReportDamage(FriendlyCharacter target)
        {
            int damage = 0;
            if (damage < 0) damage = 0;
            return damage.ToString();
        }

        #endregion

        #region Private Submethods

        public void CastFire(EnemyCharacter target)
        {
            _mana = _mana - 10;
            int damage = _magic - target.Resistance;
            if (damage < 0) damage = 0;
            target.Health = target.Health - damage;
        }

        public void CastThunder(EnemyCharacter target)
        {
            _mana = Mana - 10;
            int damage = 0;
            damage = (int)(3 * (0.5 * _magic - target.Resistance));
            if (damage < 0) damage = 0;
            target.Health = target.Health - damage;
        }

        public void CastBlizzard(EnemyCharacter target)
        {
            _mana = _mana - 10;
            int damage = (int)(_magic * 0.75);
            if (damage < 0) damage = 0;
            target.Health = target.Health - damage;
        }

        public void CastSleep(EnemyCharacter target)
        {
            _mana = _mana - 20;
            if (target.Status != EnemyCharacter.StatusEffect.KO)
            {
                target.Status = EnemyCharacter.StatusEffect.NONE;
                target.Status = EnemyCharacter.StatusEffect.SLEEP;
            }
        }

        #region SpellDamageReports

        public string ReportFire(EnemyCharacter target)
        {
            int damage = _magic - target.Resistance;
            if (damage < 0) damage = 0;
            return damage.ToString();
        }

        public string ReportThunder(EnemyCharacter target)
        {
            int damage = 0;
            damage = (int) (3 * (0.5*_magic - target.Resistance));
            if (damage < 0) damage = 0;
            return damage.ToString();
        }

        public string ReportBlizzard(EnemyCharacter target)
        {
            int damage = (int)(_magic * 0.75);
            if (damage < 0) damage = 0;
            return damage.ToString();
        }
        
        #endregion

        #endregion
    }

    class EnemyCharacter
    {
        public enum EnemyType
        {
            NONE,
            SNAKE
        }

        public enum StatusEffect
        {
            NONE,
            SLEEP,
            KO
        }

        #region Fields

        private string _name;
        private int _health;
        private int _mana;
        private int _attack;
        private int _defense;
        private int _magic;
        private int _resistance;
        private int _speed;
        private int _maxhealth;
        private int _maxmana;
        private int _baseattack;
        private int _basedefense;
        private int _basemagic;
        private int _baseresistance;
        private int _basespeed;
        private StatusEffect _status;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                if (_health <= 0)
                {
                    _health = 0;
                    _status = EnemyCharacter.StatusEffect.KO;
                }
            }
        }

        public int Mana
        {
            get { return _mana; }
            set { _mana = value; }
        }

        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public int Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        public int Magic
        {
            get { return _magic; }
            set { _magic = value; }
        }

        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int MaxHealth
        {
            get { return _maxhealth; }
            set { _maxhealth = value; }
        }

        public int MaxMana
        {
            get { return _maxmana; }
            set { _maxmana = value; }
        }

        public int BaseAttack
        {
            get { return _baseattack; }
            set { _baseattack = value; }
        }

        public int BaseDefense
        {
            get { return _basedefense; }
            set { _basedefense = value; }
        }

        public int BaseMagic
        {
            get { return _basemagic; }
            set { _basemagic = value; }
        }

        public int BaseResistance
        {
            get { return _baseresistance; }
            set { _baseresistance = value; }
        }

        public int BaseSpeed
        {
            get { return _basespeed; }
            set { _basespeed = value; }
        }

        public StatusEffect Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        #region Public Methods

        public void InitializeCharacter()
        {
            _health = MaxHealth;
            _mana = MaxMana;
            _attack = BaseAttack;
            _defense = BaseDefense;
            _magic = BaseMagic;
            _resistance = BaseResistance;
            _speed = BaseSpeed;
            _status = StatusEffect.NONE;
        }

        public void DoAttack(FriendlyCharacter target)
        {
            int damage = _attack - target.Defense;
            if (damage < 0) damage = 0;
            target.Health = (target.Health - damage);
        }

        public void DoAttack(EnemyCharacter target)
        {
            target.Health = target.Health - 0;
        }

        public string ReportDamage(FriendlyCharacter target)
        {
            int damage = _attack - target.Defense;
            if (damage < 0) damage = 0;
            return damage.ToString();
        }

        public string ReportDamage(EnemyCharacter target)
        {
            int damage = 0;
            if (damage < 0) damage = 0;
            return damage.ToString();
        }



        #endregion

        #region Private Methods



        #endregion

        #region Constructors

        public EnemyCharacter(EnemyCharacter.EnemyType monsterType)
        {
            switch (monsterType)
            {
                case EnemyType.NONE:
                    Name = "";
                    MaxHealth = 0;
                    Status = EnemyCharacter.StatusEffect.KO;
                    break;
                case EnemyType.SNAKE:
                    Name = "Snake";
                    MaxHealth = 15;
                    MaxMana = 0;
                    BaseAttack = 6;
                    BaseDefense = 2;
                    BaseMagic = 0;
                    BaseResistance = 0;
                    BaseSpeed = 10;
                    Status = EnemyCharacter.StatusEffect.NONE;
                    break;
                default:
                    break;
            }
        }

        public EnemyCharacter()
        {

        }

        #endregion
    }

    class ChatLog
    {
        #region Fields

        private string[] _chatArray = new string[6];

        #endregion

        #region Properties

        public string ChatLine
        {
            set
            {
                for (int i = 5; i > 0; i--)
                {
                    _chatArray[i] = _chatArray[i - 1];
                }
                _chatArray[0] = value;
            }
        }

        #endregion

        public void InitializeChat()
        {
            for (int i = 0; i < 6; i++)
            {
                _chatArray[i] = "";
            }
        }

        public void DisplayChat()
        {
            string asteriskBar = new string('*', 56);
            Console.WriteLine(asteriskBar);
            for (int i = 5; i >= 0; i--)
            {
                Console.WriteLine("* {0,-52} *", _chatArray[i]);
            }
            Console.WriteLine(asteriskBar);
        }
    }
}
