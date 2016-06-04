﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore.Structs;
using SystemCore.Exceptions;

namespace SystemCore.Helpers
{
    /// <summary>
    /// Klasa pomocnicza, udostępniająca metody związane ze sprawdzaniem uprawnieć.
    /// </summary>
    public static class PrivilegeHelper
    {
        private const string ACCESS_DENIED_MESSAGE = "User #{0} [{1}] has no required privilege level to access this functionality! Required level: {2}, user level: {3}.";


        /// <summary>
        /// Sprawdza, czy użytkownik ma wymagany poziom dostępu. Im poziom niższy, tym większy dostęp do funkcji systemu.
        /// </summary>
        /// <param name="user">Użytkownik zarejestrowany w systemie.</param>
        /// <param name="privilegeLevel">Poziom dostępu > 0.</param>
        /// <returns></returns>
        public static bool HasAccess(User user, int privilegeLevel)
        {
            if (privilegeLevel <= 0)
                throw new ArgumentOutOfRangeException("privilegeLevel");
            return user.PrivilegeLevel <= privilegeLevel;
        }

        /// <summary>
        /// Sprawdza, czy użytkownik ma wymagany poziom dostępu. Jeśli ów poziom jest niższy niż wymagany, rzucany jest wyjątek AccessDeniedException.
        /// </summary>
        /// <param name="user">Użytkownik, dla którego sprawdzany jest poziom dostępu.</param>
        /// <param name="privilegeLevel">Minimalny dopuszczalny poziom dostępu.</param>
        public static void AssertAccess(User user, int privilegeLevel)
        {
            if (!HasAccess(user, privilegeLevel))
                throw new AccessDeniedException(string.Format(ACCESS_DENIED_MESSAGE, user.Id, user.Name, privilegeLevel, user.PrivilegeLevel));
        }
    }
}