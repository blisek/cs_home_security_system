﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemCore.Sensors.SensorEvents.SensorEventMappers;

namespace SystemCore.Sensors.SensorEvents
{
#warning [WZORZEC] Metoda wytwórcza
    public static class SensorEventMapperFactory
    {
        private static readonly Dictionary<SensorType, SensorEventMapper> _mappers = new Dictionary<SensorType, SensorEventMapper>()
        {
            { SensorType.UNKNOWN, new DefaultSensorEventMapper() },
            { SensorType.MOVE_SENSOR, new MoveSensorEventMapper() }
        };

        /// <summary>
        /// 
        /// Zwraca odpowiedni mapper dla SensorEvent
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SensorEventMapper GetMapper(SensorType type)
        {
            SensorEventMapper sem = null;
            _mappers.TryGetValue(type, out sem);
            return sem;
        }

        /// <summary>
        /// Dodaje nowy mapper do fabryki. Jeśli wcześniej z danym typem czujnika był związany
        /// inny mapper, zostanie zwrócony, w przeciwnym wypadku metoda zwraca null.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static SensorEventMapper AddMapper(SensorType type, SensorEventMapper mapper)
        {
            SensorEventMapper sem = null;
            _mappers.TryGetValue(type, out sem);
            _mappers[type] = mapper;
            return sem;
        }
    }
}