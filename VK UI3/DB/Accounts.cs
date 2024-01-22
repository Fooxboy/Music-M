using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_UI3.DB
{
    public class AccountsDB
    {
        public class Accounts {
            [PrimaryKey]
           // [AutoIncrement]
            public long id { get; set; }
            public bool Active { get; set; } = false;
            public String Name { get; set; } = "Добавить";
            public String Token { get; set; } = null;
            public String UserPhoto { get; set; } = "null";

            public DateTimeOffset? Expiration { get; set; } = null;

            public int sortID { get; set; }
            public string ExchangeToken { get; set; } = null;

            public string AnonToken { get; set; } = null;
            public string DeviceId { get; set; } = null;

            internal void Update()
            {
                if (GetAccountsByID(id) == null)
                {
                    DatabaseHandler.getConnect().Insert(this);
                }
                else 
                {
                    DatabaseHandler.getConnect().Update(this);
                }
            }
        }

        private static Accounts? _activeAccount;

        public static Accounts activeAccount
        {
            get
            {

                if (_activeAccount == null)
                {
                    _activeAccount = AccountsDB.GetActiveAccount();
                }
                if (_activeAccount == null)
                {
                    _activeAccount = new Accounts();
                    _activeAccount.Active = true;
                }
                return _activeAccount;
            }
            set
            {
                _activeAccount = value;
            }
        }





        public static void ActivateAccount(long id)
        {
            DatabaseHandler.getConnect().Query<Accounts>("UPDATE Accounts SET Active = CASE WHEN id = "+id+" THEN 1 ELSE 0 END");
        }


        // Получить все аккаунты
        public static List<Accounts> GetAllAccounts()
        {
            return DatabaseHandler.getConnect().Table<Accounts>().ToList();
        }
        // Получить активные аккаунты
        public static List<Accounts> GetAccByID(long ID)
        {
            return DatabaseHandler.getConnect().Table<Accounts>().Where(a => a.id == ID).ToList();
        }


        // Получить активные аккаунты
        public static List<Accounts> GetActiveAccounts()
        {
            var listActiv = DatabaseHandler.getConnect().Table<Accounts>().Where(a => a.Active == true).ToList();
            if (listActiv.Count == 0)
            {
                var accs = GetAllAccounts();
                if (accs.Count > 0)
                {
                    accs[0].Active = true; ;
                    DatabaseHandler.getConnect().Update(accs[0]);
                    return GetActiveAccounts();
                }
            }

            return listActiv;
        }


        internal static Accounts GetActiveAccount()
        {
            var acs = GetActiveAccounts();

            if (acs.Count < 1) return null;
            else return acs[0];
        }


        // Получить аккаунты по имени

        public static Accounts GetAccountsByID(long id)
        {
            var a = DatabaseHandler.getConnect().Table<Accounts>().Where(a => a.id == id).ToList(); 
            if (a.Count < 1) return null;
            return a[0];
        }


        public static List<Accounts> GetAccountsByName(string name)
        {
            return DatabaseHandler.getConnect().Table<Accounts>().Where(a => a.Name == name).ToList();
        }

        // Получить аккаунты по токену
        public static List<Accounts> GetAccountsByToken(string token)
        {
            return DatabaseHandler.getConnect().Table<Accounts>().Where(a => a.Token == token).ToList();
        }

        // Получить все аккаунты, сортированные по sortID
        public static List<Accounts> GetAllAccountsSorted()
        {
            return DatabaseHandler.getConnect().Table<Accounts>().OrderBy(a => a.sortID).ToList();
        }
        public static List<Accounts> GetAllAccountsSortedDescending()
        {
            return DatabaseHandler.getConnect().Table<Accounts>().OrderByDescending(a => a.sortID).ToList();
        }

        // Функция для пересортировки аккаунтов
        public static void ReshuffleAccounts(Accounts account, int newPosition)
        {
            var accounts = GetAllAccountsSorted();
            accounts.Remove(account);
            accounts.Insert(newPosition, account);

            for (int i = 0; i < accounts.Count; i++)
            {
                accounts[i].sortID = i;
                DatabaseHandler.getConnect().Update(accounts[i]);
            }
        }

       
    }

}
