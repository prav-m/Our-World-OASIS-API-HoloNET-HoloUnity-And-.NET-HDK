﻿using System.Collections.Generic;
using NextGenSoftware.OASIS.API.Core.Holons;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;

namespace NextGenSoftware.OASIS.STAR.CelestialSpace
{
    public class SolarSystem : Holon, ISolarSystem
    {
        public IStar Star { get; set; }
        public List<IPlanet> Planets { get; set; }
        public List<IAsteroid> Asteroids { get; set; }
        public List<IComet> Comets { get; set; }
        public List<IMeteroid> Meteroids { get; set; }
    }
}