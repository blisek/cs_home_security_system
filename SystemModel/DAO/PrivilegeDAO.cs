﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemModel.TO;
using Dapper;

namespace SystemModel.DAO
{
    public class PrivilegeDAO : AbstractDAO<PrivilegeTO>
    {
        private static volatile PrivilegeDAO _instance;
        private static object _mutex = new object();

        private static List<PrivilegeTO> _cachedPrivileges;

        public static PrivilegeDAO GetInstance()
        {
            if(_instance == null)
            {
                lock(_mutex)
                {
                    if(_instance == null)
                        _instance = new PrivilegeDAO();
                }
            }

            return _instance;
        }

        private PrivilegeDAO() : base("privileges") { }

        /// <summary>
        /// Zwraca wszystkie poziomy dostępu i cache'uje je do późniejszego użytku.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<PrivilegeTO> GetAll()
        {
            if (_cachedPrivileges == null)
            {
                using (var connection = SQLiteDBConnection.GetInstance().GetConnection())
                {
                    _cachedPrivileges = connection.Query<PrivilegeTO>(QUERY_SELECT_ALL).ToList();
                }
            }

            return _cachedPrivileges;
        }

        /// <summary>
        /// Zwraca PrivilegeTO o danym id lub null jeśli obiekt o takowym id nie istnieje. 
        /// </summary>
        /// <param name="id">Id > 0, identyfikujące PriviligeTO.</param>
        /// <returns>Odpowiedni PrivilegeTO, bądź null.</returns>
        public PrivilegeTO GetById(int id)
        {
            return GetAll().Where(p => p.PrivilegeId == id).SingleOrDefault();
        }

        /// <summary>
        /// Zwraca obiekty PrivilegeTO, które mają poziom dostępu równy podanemu jako parametr.
        /// </summary>
        /// <param name="level">Poziom dostępu dla którego będą szukane przywileje.</param>
        /// <returns>Wszystkie przywileje</returns>
        public IEnumerable<PrivilegeTO> GetByPrivilegeLevelEq(int level)
        {
            return GetByPredicate(pTO => pTO.PrivilegeLevel == level);
        }

        /// <summary>
        /// Zwraca enumerator do elementów PrivilegeTO, które mają poziom dostępu równy bądź wyższy od danego poziomu. W tym wypadku przez "wyższy poziom dostępu"
        /// rozumie się poziom mniejszy równy danemu poziomowi.
        /// </summary>
        /// <param name="level">Dany poziom minimalny.</param>
        /// <returns>Enumerator do obiektów spełniających predykat.</returns>
        public IEnumerable<PrivilegeTO> GetByPrivilegeLevelLeEq(int level)
        {
            return GetByPredicate(pTO => pTO.PrivilegeLevel <= level);
        }

        /// <summary>
        /// Ogólna metoda filtrująca przekazanym predykatem.
        /// </summary>
        /// <param name="privilegePredicate">Predykat służący do odfiltrowania wyników.</param>
        /// <returns>Enumerator do elementów spełniających warunki.</returns>
        public IEnumerable<PrivilegeTO> GetByPredicate(Func<PrivilegeTO, bool> predicate)
        {
            return GetAll().Where(predicate);
        }
    }
}
